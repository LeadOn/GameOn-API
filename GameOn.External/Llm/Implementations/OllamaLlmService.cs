// <copyright file="OllamaLlmService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.Llm.Implementations
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.External.Llm.Exceptions;
    using GameOn.External.Llm.Interfaces;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// <see cref="ILlmService"/> implementation backed by a self-hosted Ollama server.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Targets <c>POST /api/chat</c> with <c>stream: false</c>. Its <c>format</c> field takes a standard JSON
    /// schema and constrains decoding itself, so the contract in <c>LoLCoachPrompt.ResponseSchema</c> goes
    /// across as-is, exactly as it does for Gemini - no second dialect to maintain.
    /// </para>
    /// <para>
    /// Carries no pacing of its own, unlike <see cref="GeminiLlmService"/>: there is no quota to respect here,
    /// and the box can only ever run one generation at a decent speed anyway. The single consumer in
    /// <c>ProcessLoLCoachQueueJob</c> is all the serialisation this needs.
    /// </para>
    /// </remarks>
    public class OllamaLlmService : ILlmService
    {
        /// <summary>
        /// Name of the dedicated <see cref="HttpClient"/> registration. A generation on CPU runs for minutes,
        /// and the very first one also pays for loading the weights off disk.
        /// </summary>
        public const string HttpClientName = "OllamaLlm";

        private const string DefaultModel = "qwen3.5:9b";

        /// <summary>
        /// How long the server keeps the weights resident after answering. Loading a twenty gigabyte model
        /// costs far more than a generation does, so paying it once per session beats paying it per report.
        /// </summary>
        private static readonly string KeepAlive = Environment.GetEnvironmentVariable("OLLAMA_KEEP_ALIVE") ?? "30m";

        private readonly IHttpClientFactory httpClientFactory;
        private readonly string baseUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="OllamaLlmService"/> class.
        /// </summary>
        /// <param name="httpClientFactory"><see cref="IHttpClientFactory"/>, injected.</param>
        public OllamaLlmService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
            this.baseUrl = (Environment.GetEnvironmentVariable("OLLAMA_BASE_URL") ?? string.Empty).TrimEnd('/');
            this.ModelName = Environment.GetEnvironmentVariable("OLLAMA_MODEL") ?? DefaultModel;
        }

        /// <inheritdoc/>
        public string ModelName { get; }

        /// <inheritdoc/>
        public bool IsConfigured => !string.IsNullOrWhiteSpace(this.baseUrl);

        /// <inheritdoc/>
        public async Task<string> GenerateJsonAsync(string systemPrompt, string userPrompt, string jsonSchema, CancellationToken cancellationToken = default)
        {
            if (!this.IsConfigured)
            {
                throw new LlmException("OLLAMA_BASE_URL is not set.");
            }

            var options = new JObject
            {
                ["temperature"] = 0.7,

                // Ollama defaults to a context far shorter than a coach brief, and silently drops what does not
                // fit - which would cut the head off the system prompt rather than fail loudly.
                ["num_ctx"] = ReadInt("LLM_NUM_CTX", 8192),
                ["num_predict"] = ReadInt("LLM_NUM_PREDICT", 1024),
            };

            // Left to itself inside an LXC, llama.cpp counts the host's logical cores and oversubscribes the
            // container. Hyper-threads do not help a memory-bound workload anyway.
            var threads = ReadInt("LLM_NUM_THREADS", 0);

            if (threads > 0)
            {
                options["num_thread"] = threads;
            }

            var body = new JObject
            {
                ["model"] = this.ModelName,
                ["messages"] = new JArray
                {
                    new JObject { ["role"] = "system", ["content"] = systemPrompt },
                    new JObject { ["role"] = "user", ["content"] = userPrompt },
                },
                ["format"] = JToken.Parse(jsonSchema),
                ["stream"] = false,
                ["keep_alive"] = KeepAlive,
                ["options"] = options,
            };

            var client = this.httpClientFactory.CreateClient(HttpClientName);
            HttpResponseMessage response;

            try
            {
                response = await client.PostAsync(
                    $"{this.baseUrl}/api/chat",
                    new StringContent(body.ToString(Formatting.None), Encoding.UTF8, "application/json"),
                    cancellationToken);
            }
            catch (HttpRequestException ex)
            {
                // The server is a container next door: unreachable almost always means restarting, not gone.
                throw new LlmTransientException($"Ollama at {this.baseUrl} could not be reached: {ex.Message}");
            }
            catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                // Our own timeout, not the caller's. A cold model can spend minutes just being read off disk,
                // so the next attempt has a real chance of landing on a warm one.
                throw new LlmTransientException($"Ollama did not answer within the timeout while running {this.ModelName}.");
            }

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if ((int)response.StatusCode >= 500)
            {
                throw new LlmTransientException($"Ollama answered {(int)response.StatusCode}: {responseBody}");
            }

            if (!response.IsSuccessStatusCode)
            {
                // A 404 here means the model was never pulled - worth reading as-is in the logs.
                throw new LlmException($"Ollama answered {(int)response.StatusCode}: {responseBody}");
            }

            return ExtractJson(responseBody, this.ModelName);
        }

        /// <summary>
        /// Reads an integer tuning knob from the environment.
        /// </summary>
        /// <param name="variable">Environment variable name.</param>
        /// <param name="fallback">Value to use when unset or unparseable.</param>
        /// <returns>The configured value, or <paramref name="fallback"/>.</returns>
        private static int ReadInt(string variable, int fallback)
            => int.TryParse(Environment.GetEnvironmentVariable(variable), out var value) ? value : fallback;

        /// <summary>
        /// Pulls the generated document out of a chat response.
        /// </summary>
        /// <param name="responseBody">Raw response body.</param>
        /// <param name="modelName">Model that answered, for the error message.</param>
        /// <returns>The generated JSON document.</returns>
        private static string ExtractJson(string responseBody, string modelName)
        {
            var parsed = JObject.Parse(responseBody);
            var text = parsed["message"]?["content"]?.ToString();

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new LlmException($"Ollama returned no usable output from {modelName}.");
            }

            // Hitting the token ceiling truncates the document mid-object: schema-constrained decoding keeps it
            // well-formed as far as it goes, but what comes back is not the contract.
            if (parsed["done_reason"]?.ToString() == "length")
            {
                throw new LlmException($"Ollama hit the output limit and returned a truncated document from {modelName}. Raise LLM_NUM_PREDICT.");
            }

            return text;
        }
    }
}
