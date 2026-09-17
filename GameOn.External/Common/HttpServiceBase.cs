// <copyright file="HttpServiceBase.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>
namespace GameOn.External.Common
{
    using System.Net;
    using GameOn.External.Common.Exceptions;
    using Newtonsoft.Json;

    /// <summary>
    /// Base HTTP Service class.
    /// </summary>
    public class HttpServiceBase
    {
        /// <summary>
        /// Send http request with the given settings.
        /// </summary>
        /// <typeparam name="TResponse">Object to parse the response content into.</typeparam>
        /// <param name="client"><see cref="HttpClient"/> handler to send the request with.</param>
        /// <param name="message"><see cref="HttpRequestMessage"/> settings of the request.</param>
        /// <param name="cancellationToken">Token to stop all async execution.</param>
        /// <returns>Retrieved data from the response body.</returns>
        /// <exception cref="ExternalApiException">The third party answered with a status we don't handle.</exception>
        protected static async Task<TResponse?> RunRequest<TResponse>(HttpClient client, HttpRequestMessage message, CancellationToken cancellationToken = default)
        {
            var requestUri = ExternalApiException.Redact(message.RequestUri?.ToString() ?? string.Empty);

            var response = await client.SendAsync(message, cancellationToken);

            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    return default;

                case HttpStatusCode.OK:
                    {
                        if (response.Content is null)
                        {
                            throw new ExternalApiException(response.StatusCode, requestUri, "Empty response content.");
                        }

                        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

                        if (responseBody is null)
                        {
                            throw new ExternalApiException(response.StatusCode, requestUri, "No content retrieved.");
                        }

                        return JsonConvert.DeserializeObject<TResponse>(responseBody);
                    }

                default:
                    {
                        // The payload is what tells a rotated key apart from a rate limit or a stale identifier,
                        // so it travels with the exception instead of being dropped on the floor.
                        var errorBody = response.Content is null
                            ? string.Empty
                            : await response.Content.ReadAsStringAsync(cancellationToken);

                        throw new ExternalApiException(response.StatusCode, requestUri, errorBody);
                    }
            }
        }
    }
}
