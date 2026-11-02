// <copyright file="ValorantApiResponseDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.ValorantApi.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Envelope of every <c>valorant-api.com</c> response.
    /// </summary>
    /// <typeparam name="T">Payload type.</typeparam>
    public class ValorantApiResponseDto<T>
    {
        /// <summary>
        /// Gets or sets the HTTP status echoed by the API.
        /// </summary>
        [JsonProperty("status")]
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the payload.
        /// </summary>
        [JsonProperty("data")]
        public T? Data { get; set; }
    }
}
