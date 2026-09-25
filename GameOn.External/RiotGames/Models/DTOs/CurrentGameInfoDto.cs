// <copyright file="CurrentGameInfoDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// CurrentGameInfoDto class. Mirrors spectator-v5's <c>CurrentGameInfo</c>: a game in progress. Only the
    /// fields we read are mapped; the others (bans, observers, runes...) are ignored on deserialization.
    /// </summary>
    public class CurrentGameInfoDto
    {
        /// <summary>
        /// Gets or sets the game ID. match-v5 names the same game <c>{platformId}_{gameId}</c> once it is over.
        /// </summary>
        [JsonProperty("gameId")]
        public long GameId { get; set; }

        /// <summary>
        /// Gets or sets the game type (MATCHED, CUSTOM, TUTORIAL).
        /// </summary>
        [JsonProperty("gameType")]
        public string? GameType { get; set; }

        /// <summary>
        /// Gets or sets the game mode (CLASSIC, ARAM, ...).
        /// </summary>
        [JsonProperty("gameMode")]
        public string? GameMode { get; set; }

        /// <summary>
        /// Gets or sets the map ID.
        /// </summary>
        [JsonProperty("mapId")]
        public long MapId { get; set; }

        /// <summary>
        /// Gets or sets the platform the game is played on (ex: EUW1).
        /// </summary>
        [JsonProperty("platformId")]
        public string? PlatformId { get; set; }

        /// <summary>
        /// Gets or sets the queue ID, the same referential as match-v5's <c>queueId</c>. Absent (0) on some
        /// custom games.
        /// </summary>
        [JsonProperty("gameQueueConfigId")]
        public long? GameQueueConfigId { get; set; }

        /// <summary>
        /// Gets or sets the game start, in milliseconds since the Unix epoch. 0 while the players are still on
        /// the loading screen.
        /// </summary>
        [JsonProperty("gameStartTime")]
        public long GameStartTime { get; set; }

        /// <summary>
        /// Gets or sets the time elapsed in the game, in seconds. 0 (or slightly negative) while the players
        /// are still on the loading screen.
        /// </summary>
        [JsonProperty("gameLength")]
        public long GameLength { get; set; }

        /// <summary>
        /// Gets or sets the players in the game, both teams.
        /// </summary>
        [JsonProperty("participants")]
        public List<CurrentGameParticipantDto> Participants { get; set; } = new List<CurrentGameParticipantDto>();
    }
}
