// <copyright file="LoLGameTimelineEventAssist.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// LoLGameTimelineEventAssist class.
    /// </summary>
    public class LoLGameTimelineEventAssist
    {
        /// <summary>
        /// Gets or sets ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets LoL Game Timeline event ID.
        /// </summary>
        public int LoLGameTimelineEventId { get; set; }

        /// <summary>
        /// Gets or sets Match ID (denormalized, needed to link to <see cref="LoLGameParticipant"/> via PUUID).
        /// </summary>
        public string MatchId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets assisting participant ID.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets assisting participant PUUID (resolved from <see cref="ParticipantId"/>).
        /// </summary>
        public string? ParticipantPUUID { get; set; }

        /// <summary>
        /// Gets or sets Event.
        /// </summary>
        [JsonIgnore]
        public virtual LoLGameTimelineEvent Event { get; set; } = null!;

        /// <summary>
        /// Gets or sets the assisting game participant (resolved via <see cref="MatchId"/> + <see cref="ParticipantPUUID"/>).
        /// </summary>
        public virtual LoLGameParticipant? Participant { get; set; }
    }
}
