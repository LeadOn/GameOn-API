// <copyright file="HenrikDevRequestFactory.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev
{
    using System;
    using System.Net.Http;
    using GameOn.External.HenrikDev.RateLimiting;

    /// <summary>
    /// Builds HenrikDev requests: base URL (<c>HENRIKDEV_API_BASE_URL</c>), API key (<c>HENRIKDEV_API_KEY</c>)
    /// and priority.
    /// </summary>
    public static class HenrikDevRequestFactory
    {
        private const string DefaultBaseUrl = "https://api.henrikdev.xyz";

        /// <summary>
        /// Creates a GET request.
        /// </summary>
        /// <param name="pathAndQuery">Path below the base URL, starting with <c>/valorant/</c>.</param>
        /// <param name="lowPriority">True for background work (backfill, daily extras), which never takes the
        /// last requests of a window.</param>
        /// <returns>The request.</returns>
        public static HttpRequestMessage Get(string pathAndQuery, bool lowPriority)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, GetBaseUrl() + pathAndQuery);
            var apiKey = Environment.GetEnvironmentVariable("HENRIKDEV_API_KEY");

            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                request.Headers.TryAddWithoutValidation("Authorization", apiKey.Trim());
            }

            request.Options.Set(HenrikDevRateLimitHandler.LowPriority, lowPriority);
            return request;
        }

        /// <summary>
        /// Escapes a path segment, defaulting an empty region to <c>eu</c>.
        /// </summary>
        /// <param name="region">Region/affinity.</param>
        /// <returns>The escaped region.</returns>
        public static string Region(string? region) =>
            Uri.EscapeDataString(string.IsNullOrWhiteSpace(region) ? "eu" : region.Trim().ToLowerInvariant());

        private static string GetBaseUrl()
        {
            var configured = Environment.GetEnvironmentVariable("HENRIKDEV_API_BASE_URL");
            return string.IsNullOrWhiteSpace(configured) ? DefaultBaseUrl : configured.Trim().TrimEnd('/');
        }
    }
}
