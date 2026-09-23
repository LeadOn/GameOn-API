// <copyright file="RecomputeLoLGameRankChangesResultDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    /// <summary>
    /// Result of a per-game LP recomputation.
    /// </summary>
    public class RecomputeLoLGameRankChangesResultDto
    {
        /// <summary>
        /// Gets or sets the number of ranked game participations looked at.
        /// </summary>
        public int ParticipationsScanned { get; set; }

        /// <summary>
        /// Gets or sets the number of participations that hold an LP change after the run.
        /// </summary>
        public int ParticipationsWithRankChange { get; set; }

        /// <summary>
        /// Gets or sets the number of LP changes written for the first time.
        /// </summary>
        public int Created { get; set; }

        /// <summary>
        /// Gets or sets the number of LP changes whose value moved.
        /// </summary>
        public int Updated { get; set; }

        /// <summary>
        /// Gets or sets the number of LP changes removed, because the game can no longer be told apart
        /// from another one (typically a game imported late, landing between the same two snapshots).
        /// </summary>
        public int Removed { get; set; }
    }
}
