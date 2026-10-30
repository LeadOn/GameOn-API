// <copyright file="ValGame.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Valorant match entity.
    /// </summary>
    public class ValGame
    {
        /// <summary>
        /// Gets or sets the Valorant match UUID (primary key).
        /// </summary>
        public string MatchId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Valorant map UUID.
        /// </summary>
        public string? MapId { get; set; }

        /// <summary>
        /// Gets or sets the Valorant map display name (e.g. "Ascent", "Haven", "Bind").
        /// </summary>
        public string? MapName { get; set; }

        /// <summary>
        /// Gets or sets the Valorant game version string.
        /// </summary>
        public string? GameVersion { get; set; }

        /// <summary>
        /// Gets or sets the Valorant shard/region (e.g. "eu").
        /// </summary>
        public string? Region { get; set; }

        /// <summary>
        /// Gets or sets the server cluster (e.g. "Paris", "Frankfurt", "London").
        /// </summary>
        public string? Cluster { get; set; }

        /// <summary>
        /// Gets or sets the platform ("pc" or "console").
        /// </summary>
        public string? Platform { get; set; }

        /// <summary>
        /// Gets or sets the queue identifier (e.g. "competitive", "unrated", "swiftplay", "deathmatch", "custom").
        /// </summary>
        public string? QueueId { get; set; }

        /// <summary>
        /// Gets or sets the human-readable queue name (e.g. "Competitive", "Unrated", "Custom Game").
        /// </summary>
        public string? QueueName { get; set; }

        /// <summary>
        /// Gets or sets the queue mode type (e.g. "Standard", "Skirmish", "Deathmatch").
        /// </summary>
        public string? QueueModeType { get; set; }

        /// <summary>
        /// Gets or sets the Act/Season UUID.
        /// </summary>
        public string? SeasonId { get; set; }

        /// <summary>
        /// Gets or sets the Act/Season short code (e.g. "e11a6").
        /// </summary>
        public string? SeasonShort { get; set; }

        /// <summary>
        /// Gets or sets the UTC date and time when the match was retrieved.
        /// </summary>
        public DateTime RetrievedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the UTC date and time when the match started.
        /// </summary>
        public DateTime GameStart { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the match duration in milliseconds.
        /// </summary>
        public long GameLengthMs { get; set; }

        /// <summary>
        /// Gets or sets the UTC date and time when the match ended.
        /// </summary>
        public DateTime GameEnd { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets a value indicating whether the match completed normally.
        /// </summary>
        public bool IsCompleted { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the match ended in a remake (standard match ending in 3 or fewer rounds).
        /// </summary>
        public bool IsRemake { get; set; }

        /// <summary>
        /// Gets or sets the winning team identifier ("Red", "Blue", or player PUUID in Deathmatch). Null on a draw or remake.
        /// </summary>
        public string? WinningTeamId { get; set; }

        /// <summary>
        /// Gets or sets the MVP participant ID (highest <see cref="ValGameParticipantStat.Rating"/> on the winning team).
        /// </summary>
        public int? MvpParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the ACE participant ID (highest <see cref="ValGameParticipantStat.Rating"/> on the losing team).
        /// </summary>
        public int? AceParticipantId { get; set; }

        /// <summary>
        /// Gets the full map splash image URL from <c>media.valorant-api.com</c>, or null when no map UUID is set.
        /// </summary>
        [NotMapped]
        public string? MapSplashUrl => string.IsNullOrWhiteSpace(this.MapId)
            ? null
            : $"https://media.valorant-api.com/maps/{this.MapId}/splash.png";

        /// <summary>
        /// Gets the horizontal map list-view banner URL from <c>media.valorant-api.com</c>, or null when no map UUID is set.
        /// </summary>
        [NotMapped]
        public string? MapListViewIconUrl => string.IsNullOrWhiteSpace(this.MapId)
            ? null
            : $"https://media.valorant-api.com/maps/{this.MapId}/listviewicon.png";

        /// <summary>
        /// Gets or sets the participants in the match.
        /// </summary>
        public virtual List<ValGameParticipant> ValorantGameParticipants { get; set; } = null!;

        /// <summary>
        /// Gets or sets the teams in the match.
        /// </summary>
        public virtual List<ValGameTeam> ValorantGameTeams { get; set; } = null!;

        /// <summary>
        /// Gets or sets the round-by-round breakdown of the match.
        /// </summary>
        [JsonIgnore]
        public virtual List<ValGameRound> ValorantGameRounds { get; set; } = null!;

        /// <summary>
        /// Gets or sets the kill events of the match.
        /// </summary>
        [JsonIgnore]
        public virtual List<ValGameKill> ValorantGameKills { get; set; } = null!;

        /// <summary>
        /// Gets or sets each player's round-by-round buy, score and damage.
        /// </summary>
        [JsonIgnore]
        public virtual List<ValGameRoundPlayerStat> ValorantGameRoundPlayerStats { get; set; } = null!;
    }
}
