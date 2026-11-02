// <copyright file="ValGameParticipantAccolade.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// An accolade (end-of-game gold star) a participant earned in a Valorant match.
    /// </summary>
    public class ValGameParticipantAccolade
    {
        /// <summary>
        /// Gets or sets the primary key ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the participant ID.
        /// </summary>
        public int ValGameParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the accolade UUID.
        /// </summary>
        public string AccoladeId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the accolade type (e.g. kills, clutches, top_frag).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the value that earned the accolade.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the value is the participant's record of the act.
        /// </summary>
        public bool IsActRecord { get; set; }

        /// <summary>
        /// Gets or sets the participant.
        /// </summary>
        [JsonIgnore]
        public virtual ValGameParticipant Participant { get; set; } = null!;
    }
}
