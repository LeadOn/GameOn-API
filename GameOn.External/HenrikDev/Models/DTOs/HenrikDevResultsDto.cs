// <copyright file="HenrikDevResultsDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Pagination block of HenrikDev's stored-data endpoints.
    /// </summary>
    public class HenrikDevResultsDto
    {
        /// <summary>
        /// Gets or sets the number of records matching the request.
        /// </summary>
        [JsonProperty("total")]
        public int Total { get; set; }

        /// <summary>
        /// Gets or sets the number of records in this page.
        /// </summary>
        [JsonProperty("returned")]
        public int Returned { get; set; }

        /// <summary>
        /// Gets or sets the number of records before this page.
        /// </summary>
        [JsonProperty("before")]
        public int Before { get; set; }

        /// <summary>
        /// Gets or sets the number of records after this page.
        /// </summary>
        [JsonProperty("after")]
        public int After { get; set; }
    }
}
