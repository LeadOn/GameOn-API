// <copyright file="LoLCrewChampionStatDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    /// <summary>
    /// LoLCrewChampionStatDto class. A champion's numbers across the whole crew (see
    /// <see cref="LoLGlobalStatsDto.TopChampions"/>), plus who played it the most.
    /// </summary>
    /// <remarks>
    /// A subclass rather than a nullable property on <see cref="LoLChampionStatDto"/>, because that class also
    /// serves <see cref="LoLSummonerPerformanceStatsDto.ChampionStats"/>, which is about a single account: its
    /// top player would always be that account, so the property would be null there by construction. A
    /// field that is always null on one route and never null on the other is noise in the contract. This way
    /// the per-account shape doesn't move at all, and the crew entries keep every field they already had.
    /// </remarks>
    public class LoLCrewChampionStatDto : LoLChampionStatDto
    {
        /// <summary>
        /// Gets or sets the account with the most games on this champion, within the same period, queues and
        /// roster filters (smurfs, accounts outside the crew) as the rest of the entry. Ties are broken by
        /// wins on the champion, then by the lowest player ID, so the pick is stable from one call to the
        /// next. Never null: every listed champion has at least one game.
        /// </summary>
        public LoLChampionTopPlayerDto TopPlayer { get; set; } = null!;
    }
}
