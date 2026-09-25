// <copyright file="LoLChampionTopPlayerDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    /// <summary>
    /// LoLChampionTopPlayerDto class. The account that played a champion the most, within the same games as
    /// the <see cref="LoLCrewChampionStatDto"/> holding it.
    /// </summary>
    public class LoLChampionTopPlayerDto
    {
        /// <summary>
        /// Gets or sets the account. Identity fields only: ranks, recent form and performance stats are left
        /// empty here. Per account: with smurfs included, a member's main and smurf compete separately.
        /// </summary>
        public PlayerDto Player { get; set; } = null!;

        /// <summary>
        /// Gets or sets how many of the champion's games this account played. Always at least 1.
        /// </summary>
        public int GamesPlayed { get; set; }
    }
}
