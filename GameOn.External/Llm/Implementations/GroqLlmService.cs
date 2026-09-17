// <copyright file="GroqLlmService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.Llm.Implementations
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.External.Llm.Exceptions;
    using GameOn.External.Llm.Interfaces;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// <see cref="ILlmService"/> implementation backed by the Groq API.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Groq speaks the OpenAI chat dialect (<c>POST /openai/v1/chat/completions</c>), so the response contract
    /// travels as a <c>json_schema</c> response format rather than as a bare schema. In strict mode decoding is
    /// constrained server-side and the answer cannot drift from the contract.
    /// </para>
    /// <para>
    /// Paces itself like <see cref="GeminiLlmService"/> and unlike <see cref="OllamaLlmService"/>, because there
    /// is a quota again - but a different one. Here the daily allowance is effectively unreachable and the
    /// per-minute token budget is what binds, which matters because this provider answers in seconds: left
    /// unpaced, the queue would drain a burst of reports far faster than the token budget refills.
    /// </para>
    /// </remarks>
    public class GroqLlmService : ILlmService
    {
        /// <summary>
        /// Name of the dedicated <see cref="HttpClient"/> registration.
        /// </summary>
        public const string HttpClientName = "GroqLlm";

        private const string CompletionsUrl = "https://api.groq.com/openai/v1/chat/completions";
        private const string DefaultModel = "openai/gpt-oss-120b";

        /// <summary>
        /// Floor on the delay between two calls leaving this process, applied whatever the outcome. Sized on the
        /// per-minute token budget rather than the request count: a coach brief is a few thousand tokens, so the
        /// tokens run out long before the requests do.
        /// </summary>
        private static readonly TimeSpan MinimumInterval = TimeSpan.FromSeconds(
            int.TryParse(Environment.GetEnvironmentVariable("LLM_MIN_INTERVAL_SECONDS"), out var interval) ? interval : 35);

        /// <summary>
        /// Caps the process to one generation at a time. <c>ProcessLoLCoachQueueJob</c> is the only caller, so
        /// this never contends; it is here to keep <see cref="nextSlotAvailableOn"/> honest.
        /// </summary>
        private static readonly SemaphoreSlim GenerationLock = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Earliest instant the next call may be sent. Only read or written under <see cref="GenerationLock"/>.
        /// </summary>
        private static DateTimeOffset nextSlotAvailableOn = DateTimeOffset.MinValue;

        private readonly IHttpClientFactory httpClientFactory;
        private readonly string apiKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroqLlmService"/> class.
        /// </summary>
        /// <param name="httpClientFactory"><see cref="IHttpClientFactory"/>, injected.</param>
        public GroqLlmService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
            this.apiKey = Environment.GetEnvironmentVariable("GROQ_API_KEY") ?? string.Empty;
            this.ModelName = Environment.GetEnvironmentVariable("GROQ_MODEL") ?? DefaultModel;
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
                throw new LlmException("GROQ_API_KEY is not set.");
            }

            var body = new JObject
            {
                ["model"] = this.ModelName,
                ["messages"] = new JArray
                {
                    new JObject { ["role"] = "system", ["content"] = systemPrompt },
                    new JObject { ["role"] = "user", ["content"] = userPrompt },
                },
                ["response_format"] = new JObject
                {
                    ["type"] = "json_schema",
                    ["json_schema"] = new JObject
                    {
                        ["name"] = "coach_report",
                        ["strict"] = true,
                        ["schema"] = Harden(JToken.Parse(jsonSchema)),
                    },
                },
                ["temperature"] = 0.7,
            };

            await GenerationLock.WaitAsync(cancellationToken);

            try
            {
                var pacing = nextSlotAvailableOn - DateTimeOffset.UtcNow;

                if (pacing > TimeSpan.Zero)
                {
                    await Task.Delay(pacing, cancellationToken);
                }

                nextSlotAvailableOn = DateTimeOffset.UtcNow + MinimumInterval;

                var client = this.httpClientFactory.CreateClient(HttpClientName);
                var request = new HttpRequestMessage(HttpMethod.Post, CompletionsUrl)
                {
                    Content = new StringContent(body.ToString(Formatting.None), Encoding.UTF8, "application/json"),
                };

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.apiKey);

                var response = await client.SendAsync(request, cancellationToken);
                var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

                if (response.StatusCode == HttpStatusCode.TooManyRequests || (int)response.StatusCode >= 500)
                {
                    // Groq states how long to stand down; carrying it into the message puts the real number in
                    // the logs instead of leaving the fixed backoff in the job to be guessed at.
                    var retryAfter = response.Headers.RetryAfter?.Delta?.TotalSeconds
                        ?? response.Headers.RetryAfter?.Date?.Subtract(DateTimeOffset.UtcNow).TotalSeconds;

                    throw new LlmTransientException(
                        $"Groq answered {(int)response.StatusCode} (retry after {retryAfter?.ToString("0") ?? "unspecified"}s): {responseBody}");
                }

                if (!response.IsSuccessStatusCode)
                {
                    throw new LlmException($"Groq answered {(int)response.StatusCode}: {responseBody}");
                }

                return ExtractJson(responseBody);
            }
            finally
            {
                GenerationLock.Release();
            }
        }

        /// <summary>
        /// Closes every object in a schema to properties it declares.
        /// </summary>
        /// <remarks>
        /// Strict mode refuses a schema whose objects leave <c>additionalProperties</c> open. Doing this here
        /// rather than in the shared contract is deliberate: the schema in the Application layer stays plain
        /// JSON Schema, and each provider's dialect stays inside that provider.
        /// </remarks>
        /// <param name="schema">Schema to walk, modified in place.</param>
        /// <returns>The same token, for chaining.</returns>
        private static JToken Harden(JToken schema)
        {
            if (schema is JObject node)
            {
                if (node["type"]?.ToString() == "object")
                {
                    node["additionalProperties"] = false;
                }

                foreach (var child in node.Properties().Select(property => property.Value).ToList())
                {
                    Harden(child);
                }
            }
            else if (schema is JArray array)
            {
                foreach (var item in array)
                {
                    Harden(item);
                }
            }

            return schema;
        }

        /// <summary>
        /// Pulls the generated document out of a completion envelope.
        /// </summary>
        /// <param name="responseBody">Raw response body.</param>
        /// <returns>The generated JSON document.</returns>
        private static string ExtractJson(string responseBody)
        {
            var parsed = JObject.Parse(responseBody);
            var choice = parsed["choices"]?.FirstOrDefault();
            var text = choice?["message"]?["content"]?.ToString();

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new LlmException("Groq returned no usable output.");
            }

            // Constrained decoding keeps a truncated answer well-formed as far as it goes, which is exactly why
            // this has to be checked rather than left to the JSON parser to notice.
            if (choice?["finish_reason"]?.ToString() == "length")
            {
                throw new LlmException("Groq hit the output limit and returned a truncated document.");
            }

            return text;
        }
    }
}
