// <copyright file="ValorantAgentMastery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    /// <summary>
    /// A player's mastery of one Valorant agent. Snapshot replaced on every refresh.
    /// </summary>
    public class ValorantAgentMastery
    {
        /// <summary>
        /// Gets or sets the primary key ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the player ID.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the agent UUID.
        /// </summary>
        public string AgentId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the agent display name.
        /// </summary>
        public string? AgentName { get; set; }

        /// <summary>
        /// Gets or sets the highest mastery track level reached, null when the agent was never levelled.
        /// </summary>
        public int? Level { get; set; }

        /// <summary>
        /// Gets or sets the mastery tracks (ID and level) as a JSON array.
        /// </summary>
        public string? TracksJson { get; set; }

        /// <summary>
        /// Gets or sets the short flourish level.
        /// </summary>
        public int? FlourishShortLevel { get; set; }

        /// <summary>
        /// Gets or sets the long flourish level.
        /// </summary>
        public int? FlourishLongLevel { get; set; }

        /// <summary>
        /// Gets or sets the equipped hype cosmetic UUID.
        /// </summary>
        public string? HypeId { get; set; }

        /// <summary>
        /// Gets or sets the equipped background cosmetic UUID.
        /// </summary>
        public string? BackgroundId { get; set; }

        /// <summary>
        /// Gets or sets the equipped clip cosmetic UUID.
        /// </summary>
        public string? ClipId { get; set; }

        /// <summary>
        /// Gets or sets the equipped border cosmetic UUID.
        /// </summary>
        public string? BorderId { get; set; }

        /// <summary>
        /// Gets or sets the UTC date this row was last refreshed.
        /// </summary>
        public DateTime RefreshedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets the agent icon URL on <c>media.valorant-api.com</c>.
        /// </summary>
        [NotMapped]
        public string? AgentIconUrl => string.IsNullOrWhiteSpace(this.AgentId)
            ? null
            : $"https://media.valorant-api.com/agents/{this.AgentId}/displayicon.png";

        /// <summary>
        /// Gets or sets the player.
        /// </summary>
        [JsonIgnore]
        public virtual Player Player { get; set; } = null!;
    }
}
