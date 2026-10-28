// <copyright file="HenrikDevResponseDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Standard envelope returned by the HenrikDev Valorant API (<c>status</c> + <c>data</c>).
    /// </summary>
    /// <typeparam name="TData">Payload type inside the <c>data</c> property.</typeparam>
    public class HenrikDevResponseDto<TData>
    {
        /// <summary>
        /// Gets or sets the HTTP status code echoed in the payload.
        /// </summary>
        [JsonProperty("status")]
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the response data payload.
        /// </summary>
        [JsonProperty("data")]
        public TData? Data { get; set; }
    }
}