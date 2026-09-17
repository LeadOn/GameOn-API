// <copyright file="ILlmService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.Llm.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Large Language Model service interface. Provider agnostic on purpose: the wire format of the
    /// underlying provider (Google Gemini today, a self-hosted OpenAI-compatible server tomorrow)
    /// must never leak past this seam.
    /// </summary>
    public interface ILlmService
    {
        /// <summary>
        /// Gets the name of the model answering requests, as it should be stamped on generated content
        /// so that a later prompt or model change can be detected and the content regenerated.
        /// </summary>
        string ModelName { get; }

        /// <summary>
        /// Gets a value indicating whether the service has everything it needs to answer. Lets callers skip
        /// their work entirely rather than queueing requests that are certain to fail.
        /// </summary>
        bool IsConfigured { get; }

        /// <summary>
        /// Generates a JSON document constrained by the given schema.
        /// </summary>
        /// <param name="systemPrompt">Instructions defining the model's role and rules.</param>
        /// <param name="userPrompt">The actual payload to reason about.</param>
        /// <param name="jsonSchema">Raw JSON schema the answer must conform to. Injected as-is into the provider request.</param>
        /// <param name="cancellationToken">Token to stop all async execution.</param>
        /// <returns>The raw JSON string produced by the model, guaranteed non-empty.</returns>
        Task<string> GenerateJsonAsync(string systemPrompt, string userPrompt, string jsonSchema, CancellationToken cancellationToken = default);
    }
}
