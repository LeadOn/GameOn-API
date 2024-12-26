// <copyright file="InfoDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// InfoDto class.
    /// </summary>
    public class InfoDto
    {
        /// <summary>
        /// Gets or sets match result.
        /// </summary>
        [JsonProperty("endOfGameResult")]
        public string EndOfGameResult { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets match creation timestamp.
        /// </summary>
        [JsonProperty("gameCreation")]
        public long GameCreation { get; set; }

        /// <summary>
        /// Gets or sets match duration.
        /// </summary>
        [JsonProperty("gameDuration")]
        public long GameDuration { get; set; }

        /// <summary>
        /// Gets or sets match end timestamp.
        /// </summary>
        [JsonProperty("gameEndTimestamp")]
        public long GameEndTimestamp { get; set; }

        /// <summary>
        /// Gets or sets match ID.
        /// </summary>
        [JsonProperty("gameId")]
        public long GameId { get; set; }

        /// <summary>
        /// Gets or sets match mode.
        /// </summary>
        [JsonProperty("gameMode")]
        public string GameMode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets match name.
        /// </summary>
        [JsonProperty("gameName")]
        public string GameName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets match start timestamp.
        /// </summary>
        [JsonProperty("gameStartTimestamp")]
        public long GameStartTimeStamp { get; set; }

        /// <summary>
        /// Gets or sets match type.
        /// </summary>
        [JsonProperty("gameType")]
        public string GameType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets match version.
        /// </summary>
        [JsonProperty("gameVersion")]
        public string GameVersion { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets match map ID.
        /// </summary>
        [JsonProperty("mapId")]
        public int MapId { get; set; }

        /// <summary>
        /// Gets or sets match platform ID.
        /// </summary>
        [JsonProperty("platformId")]
        public string PlatformId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets match queue ID.
        /// </summary>
        [JsonProperty("queueId")]
        public int QueueId { get; set; }

        /// <summary>
        /// Gets or sets match Tournament code.
        /// </summary>
        [JsonProperty("tournamentCode")]
        public string TournamentCode { get; set; } = string.Empty;
    }
}
