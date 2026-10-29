// <copyright file="HenrikDevRateLimitHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.RateLimiting
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Keeps every HenrikDev call inside the key's quota (30 requests per rolling minute on the basic tier).
    /// </summary>
    /// <remarks>
    /// HenrikDev reports the state of the bucket on every response (<c>x-ratelimit-remaining</c>,
    /// <c>x-ratelimit-reset</c> in seconds). When the last request of the window has been spent, or when a 429
    /// comes back anyway, every following request waits for the window to reset instead of being sent to
    /// collect another refusal. Reading the headers rather than hard-coding a pace means a key on a higher tier
    /// is used at its own limit without any configuration.
    /// </remarks>
    public class HenrikDevRateLimitHandler : DelegatingHandler
    {
        /// <summary>
        /// Window length assumed when a refusal carries no reset hint.
        /// </summary>
        private static readonly TimeSpan DefaultReset = TimeSpan.FromSeconds(60);

        private readonly HenrikDevRateLimitState state;

        /// <summary>
        /// Initializes a new instance of the <see cref="HenrikDevRateLimitHandler"/> class.
        /// </summary>
        /// <param name="state">Shared quota state, injected.</param>
        public HenrikDevRateLimitHandler(HenrikDevRateLimitState state)
        {
            this.state = state;
        }

        /// <inheritdoc/>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Looped because another caller may have pushed the block further while this one was waiting.
            var wait = this.state.GetWait();
            while (wait > TimeSpan.Zero)
            {
                await Task.Delay(wait, cancellationToken);
                wait = this.state.GetWait();
            }

            var response = await base.SendAsync(request, cancellationToken);

            var reset = ReadInt(response, "x-ratelimit-reset") is { } seconds
                ? TimeSpan.FromSeconds(seconds)
                : DefaultReset;

            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                this.state.BlockFor(response.Headers.RetryAfter?.Delta ?? reset);
            }
            else if (ReadInt(response, "x-ratelimit-remaining") <= 0)
            {
                this.state.BlockFor(reset);
            }

            return response;
        }

        /// <summary>
        /// Reads an integer response header.
        /// </summary>
        /// <param name="response">Response to read from.</param>
        /// <param name="name">Header name.</param>
        /// <returns>The value, or null when the header is absent or unreadable.</returns>
        private static int? ReadInt(HttpResponseMessage response, string name)
        {
            if (response.Headers.TryGetValues(name, out var values)
                && int.TryParse(values.FirstOrDefault(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
            {
                return value;
            }

            return null;
        }
    }
}
