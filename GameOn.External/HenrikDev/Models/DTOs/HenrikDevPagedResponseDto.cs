// <copyright file="HenrikDevPagedResponseDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Envelope of HenrikDev's stored-data endpoints (<c>status</c>, <c>results</c> and <c>data</c>).
    /// </summary>
    /// <typeparam name="TData">Type of one record.</typeparam>
    public class HenrikDevPagedResponseDto<TData>
    {
        /// <summary>
        /// Gets or sets the HTTP status echoed in the payload.
        /// </summary>
        [JsonProperty("status")]
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the pagination block.
        /// </summary>
        [JsonProperty("results")]
        public HenrikDevResultsDto? Results { get; set; }

        /// <summary>
        /// Gets or sets the page.
        /// </summary>
        [JsonProperty("data")]
        public List<TData> Data { get; set; } = new List<TData>();
    }
}
