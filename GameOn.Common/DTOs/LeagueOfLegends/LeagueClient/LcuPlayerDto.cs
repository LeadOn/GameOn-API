// <copyright file="LcuPlayerDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends.LeagueClient
{
    /// <summary>
    /// LcuPlayerDto class. Identity of a participant, as served by the League client.
    /// </summary>
    public class LcuPlayerDto
    {
        /// <summary>
        /// Gets or sets the PUUID. The client anonymizes it (a plain UUID, not the 78 characters
        /// PUUID of the public Riot Games API), so it can only be used to tell participants apart
        /// inside a single match, never to identify a player across matches.
        /// </summary>
        public string? Puuid { get; set; }

        /// <summary>
        /// Gets or sets the Riot ID game name (the part before the #).
        /// </summary>
        public string? GameName { get; set; }

        /// <summary>
        /// Gets or sets the Riot ID tag line (the part after the #).
        /// </summary>
        public string? TagLine { get; set; }

        /// <summary>
        /// Gets or sets the legacy summoner name. Always empty since Riot IDs replaced it.
        /// </summary>
        public string? SummonerName { get; set; }
    }
}
