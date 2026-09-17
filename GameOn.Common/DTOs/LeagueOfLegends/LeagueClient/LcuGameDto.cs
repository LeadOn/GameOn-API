// <copyright file="LcuGameDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends.LeagueClient
{
    using System.Collections.Generic;

    /// <summary>
    /// LcuGameDto class. A game as served by the League client's own match history
    /// (<c>/lol-match-history/v1/games/{gameId}</c>), which is the legacy match-v4 shape and the only
    /// source of custom game data: match-v5 doesn't list custom games and returns 404, or an empty
    /// <c>Abort_Unexpected</c> stub, when asked for one by ID.
    /// </summary>
    public class LcuGameDto
    {
        /// <summary>
        /// Gets or sets the game ID.
        /// </summary>
        public long GameId { get; set; }

        /// <summary>
        /// Gets or sets the platform ID (EUW1, ...). Combined with <see cref="GameId"/> it forms the match ID.
        /// </summary>
        public string? PlatformId { get; set; }

        /// <summary>
        /// Gets or sets the game creation timestamp, in epoch milliseconds.
        /// </summary>
        public long GameCreation { get; set; }

        /// <summary>
        /// Gets or sets the game duration, in seconds.
        /// </summary>
        public long GameDuration { get; set; }

        /// <summary>
        /// Gets or sets the queue ID (3100 blind custom, 3110 draft custom, 3140 practice tool, ...).
        /// </summary>
        public int QueueId { get; set; }

        /// <summary>
        /// Gets or sets the map ID.
        /// </summary>
        public int MapId { get; set; }

        /// <summary>
        /// Gets or sets the game version.
        /// </summary>
        public string? GameVersion { get; set; }

        /// <summary>
        /// Gets or sets the game mode (CLASSIC, PRACTICETOOL, ...).
        /// </summary>
        public string? GameMode { get; set; }

        /// <summary>
        /// Gets or sets the game type. Custom games are CUSTOM_GAME.
        /// </summary>
        public string? GameType { get; set; }

        /// <summary>
        /// Gets or sets the end of game result (GameComplete, Abort_TooFewPlayers, ...).
        /// </summary>
        public string? EndOfGameResult { get; set; }

        /// <summary>
        /// Gets or sets the teams.
        /// </summary>
        public List<LcuTeamDto> Teams { get; set; } = new List<LcuTeamDto>();

        /// <summary>
        /// Gets or sets the participants.
        /// </summary>
        public List<LcuParticipantDto> Participants { get; set; } = new List<LcuParticipantDto>();

        /// <summary>
        /// Gets or sets the participant identities. Kept apart from the participants by Riot, and
        /// joined on <see cref="LcuParticipantDto.ParticipantId"/>.
        /// </summary>
        public List<LcuParticipantIdentityDto> ParticipantIdentities { get; set; } = new List<LcuParticipantIdentityDto>();
    }
}
