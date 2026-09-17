// <copyright file="ExternalApiException.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.Common.Exceptions
{
    using System;
    using System.Net;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Thrown when a third party API answered with a status we don't handle. Carries the status code, the
    /// called route and the raw payload: without them, a refused quota, an expired key and a stale identifier
    /// all reach the caller as the same opaque failure, which is exactly what used to happen here.
    /// </summary>
    public class ExternalApiException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalApiException"/> class.
        /// </summary>
        /// <param name="statusCode">Status code the third party answered with.</param>
        /// <param name="requestUri">Route that was called, API key redacted.</param>
        /// <param name="responseBody">Raw response payload, kept for diagnosis.</param>
        public ExternalApiException(HttpStatusCode statusCode, string requestUri, string responseBody)
            : base($"{(int)statusCode} {statusCode} on {requestUri}: {responseBody}")
        {
            this.StatusCode = statusCode;
            this.RequestUri = requestUri;
            this.ResponseBody = responseBody;
        }

        /// <summary>
        /// Gets the status code the third party answered with.
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Gets the route that was called, API key redacted.
        /// </summary>
        public string RequestUri { get; }

        /// <summary>
        /// Gets the raw response payload.
        /// </summary>
        public string ResponseBody { get; }

        /// <summary>
        /// Strips the API key from a URL. Riot takes its key as a query parameter, so an un-redacted URL in an
        /// exception message would put a live credential in every log line and every error response.
        /// </summary>
        /// <param name="uri">URL to redact.</param>
        /// <returns>Same URL with the key replaced by a placeholder.</returns>
        public static string Redact(string uri)
        {
            return Regex.Replace(uri, "(?<=[?&]api_key=)[^&]*", "<redacted>", RegexOptions.IgnoreCase);
        }
    }
}
