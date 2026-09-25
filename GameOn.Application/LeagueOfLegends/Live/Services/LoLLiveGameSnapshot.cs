// <copyright file="LoLLiveGameSnapshot.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Live.Services
{
    using GameOn.External.RiotGames.Models.DTOs;

    /// <summary>
    /// LoLLiveGameSnapshot class. What spectator-v5 last said about one account, and when.
    /// </summary>
    public class LoLLiveGameSnapshot
    {
        /// <summary>
        /// Gets the game the account was playing, or null when it wasn't in a game. Also null when Riot
        /// couldn't answer about this account (stale PUUID, outage): unknown is shown as not playing.
        /// </summary>
        public CurrentGameInfoDto? Game { get; init; }

        /// <summary>
        /// Gets when Riot answered, in UTC.
        /// </summary>
        public DateTime RetrievedOn { get; init; }
    }
}
