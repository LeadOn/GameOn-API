// <copyright file="LoLGameTimelineFrame.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// LoLGameTimelineFrame class.
    /// </summary>
    public class LoLGameTimelineFrame
    {
        /// <summary>
        /// Gets or sets Frame ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Match ID.
        /// </summary>
        public string MatchId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Timestamp.
        /// </summary>
        public int Timestamp { get; set; }

        /// <summary>
        /// Gets or sets Game.
        /// </summary>
        [JsonIgnore]
        public virtual LoLGame Game { get; set; } = null!;

        /// <summary>
        /// Gets or sets League of Legends Timeline Frames Participant.
        /// </summary>
        public virtual List<LoLGameTimelineFrameParticipant> LoLGameTimelineFrameParticipants { get; set; } = null!;
    }
}