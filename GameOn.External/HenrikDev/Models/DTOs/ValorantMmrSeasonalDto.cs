// <copyright file="ValorantMmrSeasonalDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// End-of-act result inside the <c>seasonal</c> array of MMR v3.
    /// </summary>
    public class ValorantMmrSeasonalDto
    {
        /// <summary>
        /// Gets or sets the act.
        /// </summary>
        [JsonProperty("season")]
        public ValorantSeasonComboDto? Season { get; set; }

        /// <summary>
        /// Gets or sets the ranked wins in the act.
        /// </summary>
        [JsonProperty("wins")]
        public int Wins { get; set; }

        /// <summary>
        /// Gets or sets the ranked games played in the act.
        /// </summary>
        [JsonProperty("games")]
        public int Games { get; set; }

        /// <summary>
        /// Gets or sets the tier held at the end of the act.
        /// </summary>
        [JsonProperty("end_tier")]
        public ValorantTierComboDto? EndTier { get; set; }

        /// <summary>
        /// Gets or sets the RR held at the end of the act.
        /// </summary>
        [JsonProperty("end_rr")]
        public int EndRr { get; set; }

        /// <summary>
        /// Gets or sets the ranking schema of the act (base or ascendant).
        /// </summary>
        [JsonProperty("ranking_schema")]
        public string? RankingSchema { get; set; }

        /// <summary>
        /// Gets or sets the final leaderboard placement, null when not ranked on it.
        /// </summary>
        [JsonProperty("leaderboard_placement")]
        public ValorantLeaderboardPlacementDto? LeaderboardPlacement { get; set; }

        /// <summary>
        /// Gets or sets the tier of each win counted in the act rank badge.
        /// </summary>
        [JsonProperty("act_wins")]
        public List<ValorantTierComboDto> ActWins { get; set; } = new List<ValorantTierComboDto>();

        /// <summary>
        /// Gets or sets the wins including placement matches.
        /// </summary>
        [JsonProperty("wins_with_placements")]
        public int WinsWithPlacements { get; set; }

        /// <summary>
        /// Gets or sets the act rank (badge tier).
        /// </summary>
        [JsonProperty("act_rank")]
        public ValorantTierComboDto? ActRank { get; set; }

        /// <summary>
        /// Gets or sets the placement games that were still needed.
        /// </summary>
        [JsonProperty("games_needed_for_rating")]
        public int GamesNeededForRating { get; set; }

        /// <summary>
        /// Gets or sets the act prestige, kept as raw JSON.
        /// </summary>
        [JsonProperty("prestige")]
        public JToken? Prestige { get; set; }
    }
}
