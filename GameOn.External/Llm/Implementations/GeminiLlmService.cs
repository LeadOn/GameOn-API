// <copyright file="GeminiLlmService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.Llm.Implementations
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.External.Llm.Exceptions;
    using GameOn.External.Llm.Interfaces;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// <see cref="ILlmService"/> implementation backed by the Google Gemini API.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Targets the Interactions API (<c>POST /v1beta/interactions</c>), not the older
    /// <c>/v1beta/models/{model}:generateContent</c>: Gemini 3.x models are simply not served on the legacy
    /// endpoint, which answers 404 for them, and 2.5 is closed to new API keys.
    /// </para>
    /// <para>
    /// Deliberately does not go through <see cref="Common.HttpServiceBase"/>: that helper throws
    /// <see cref="NotImplementedException"/> on every non-200 status, which would flatten a quota refusal
    /// (429), an overloaded model (5xx) and a malformed request (400) into the same unusable error. Telling
    /// them apart is the whole point here, since the first two are routine on the free tier.
    /// </para>
    /// </remarks>
    public class GeminiLlmService : ILlmService
    {
        /// <summary>
        /// Name of the dedicated <see cref="HttpClient"/> registration. A generation can legitimately run
        /// for minutes, well past the 100 second default of the ambient client.
        /// </summary>
        public const string HttpClientName = "GeminiLlm";

        private const string InteractionsUrl = "https://generativelanguage.googleapis.com/v1beta/interactions";
        private const string DefaultModel = "gemini-3.6-flash";

        /// <summary>
        /// Caps the whole process to one generation at a time. Nothing else in this codebase throttles calls to
        /// the provider, and the free tier's allowance is per minute: since a generation holds for roughly
        /// fifteen seconds, serialising them mechanically keeps the rate near four per minute, far below any
        /// published limit, without having to track a sliding window.
        /// </summary>
        private static readonly SemaphoreSlim GenerationLock = new SemaphoreSlim(1, 1);

        /// <summary>
        /// How long a caller waits for the slot before being turned away. Deliberately shorter than the
        /// front-end's own timeout on this endpoint: a request that would die in the browser anyway should be
        /// refused here, with a status the UI can act on, rather than holding a connection open for nothing.
        /// </summary>
        private static readonly TimeSpan QueueWait = TimeSpan.FromSeconds(30);

        private readonly IHttpClientFactory httpClientFactory;
        private readonly string apiKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeminiLlmService"/> class.
        /// </summary>
        /// <param name="httpClientFactory"><see cref="IHttpClientFactory"/>, injected.</param>
        public GeminiLlmService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
            this.apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? string.Empty;
            this.ModelName = Environment.GetEnvironmentVariable("GEMINI_MODEL") ?? DefaultModel;
        }

        /// <inheritdoc/>
        public string ModelName { get; }

        /// <inheritdoc/>
        public bool IsConfigured => !string.IsNullOrWhiteSpace(this.apiKey);

        /// <inheritdoc/>
        public async Task<string> GenerateJsonAsync(string systemPrompt, string userPrompt, string jsonSchema, CancellationToken cancellationToken = default)
        {
            if (!this.IsConfigured)
            {
                throw new LlmException("GEMINI_API_KEY is not set.");
            }

            var body = new JObject
            {
                ["model"] = this.ModelName,
                ["system_instruction"] = systemPrompt,
                ["input"] = userPrompt,
                ["response_format"] = new JObject
                {
                    ["type"] = "text",
                    ["mime_type"] = "application/json",
                    ["schema"] = JToken.Parse(jsonSchema),
                },
                ["generation_config"] = new JObject
                {
                    ["temperature"] = 0.7,
                },
            };

            if (!await GenerationLock.WaitAsync(QueueWait, cancellationToken))
            {
                throw new LlmTransientException($"Another generation is already running and did not finish within {QueueWait.TotalSeconds:0} seconds.");
            }

            try
            {
                var client = this.httpClientFactory.CreateClient(HttpClientName);
                var request = new HttpRequestMessage(HttpMethod.Post, InteractionsUrl)
                {
                    Content = new StringContent(body.ToString(Formatting.None), Encoding.UTF8, "application/json"),
                };

                // Header rather than a query string: the key must never end up in an access log or a redirect.
                request.Headers.Add("x-goog-api-key", this.apiKey);

                var response = await client.SendAsync(request, cancellationToken);
                var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

                // 429 is an exhausted quota, 5xx is a model under load - both pass on their own, so neither
                // should be reported to the caller as a defect.
                if (response.StatusCode == HttpStatusCode.TooManyRequests || (int)response.StatusCode >= 500)
                {
                    throw new LlmTransientException($"Gemini answered {(int)response.StatusCode}: {responseBody}");
                }

                if (!response.IsSuccessStatusCode)
                {
                    throw new LlmException($"Gemini answered {(int)response.StatusCode}: {responseBody}");
                }

                return ExtractJson(responseBody);
            }
            finally
            {
                GenerationLock.Release();
            }
        }

        /// <summary>
        /// Pulls the generated document out of an interaction envelope.
        /// </summary>
        /// <param name="responseBody">Raw response body.</param>
        /// <returns>The generated JSON document.</returns>
        private static string ExtractJson(string responseBody)
        {
            var parsed = JObject.Parse(responseBody);

            // An interaction carries the model's reasoning and its answer as separate steps; only the
            // model_output ones hold text meant for us.
            var text = string.Concat(parsed["steps"]?
                .Where(step => step["type"]?.ToString() == "model_output")
                .SelectMany(step => step["content"] ?? new JArray())
                .Where(content => content["type"]?.ToString() == "text")
                .Select(content => content["text"]?.ToString()) ?? Enumerable.Empty<string?>());

            if (string.IsNullOrWhiteSpace(text))
            {
                var status = parsed["status"]?.ToString() ?? "unknown";

                throw new LlmException($"Gemini returned no usable output (status: {status}).");
            }

            return text;
        }
    }
}
