// <copyright file="HttpServiceBase.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>
namespace GameOn.External.Common
{
    using System.Net;
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
        /// <exception cref="NotImplementedException">Unknown exception.</exception>
        /// <exception cref="Exception">An unknown exception occured.</exception>
        protected static async Task<TResponse?> RunRequest<TResponse>(HttpClient client, HttpRequestMessage message, CancellationToken cancellationToken = default)
        {
            var response = await client.SendAsync(message, cancellationToken);

            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    return default;

                case HttpStatusCode.OK:
                    {
                        if (response.Content is null)
                        {
                            throw new NotImplementedException();
                        }

                        var responseBody = await response.Content.ReadAsStringAsync();

                        return JsonConvert.DeserializeObject<TResponse>(responseBody ?? throw new Exception("No content retrieved."));
                    }

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
