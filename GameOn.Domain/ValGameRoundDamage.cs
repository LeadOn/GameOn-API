// <copyright file="ValGameRoundDamage.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Damage one player dealt to another during a round of a Valorant match.
    /// </summary>
    public class ValGameRoundDamage
    {
        /// <summary>
        /// Gets or sets the primary key ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the parent match UUID.
        /// </summary>
        public string MatchId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the 0-based round index.
        /// </summary>
        public int RoundNumber { get; set; }

        /// <summary>
        /// Gets or sets the PUUID of the player who dealt the damage.
        /// </summary>
        public string AttackerPuuid { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the PUUID of the player who took the damage.
        /// </summary>
        public string ReceiverPuuid { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the damage dealt.
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// Gets or sets the head hits.
        /// </summary>
        public int Headshots { get; set; }

        /// <summary>
        /// Gets or sets the body hits.
        /// </summary>
        public int Bodyshots { get; set; }

        /// <summary>
        /// Gets or sets the leg hits.
        /// </summary>
        public int Legshots { get; set; }

        /// <summary>
        /// Gets or sets the parent match.
        /// </summary>
        [JsonIgnore]
        public virtual ValGame Game { get; set; } = null!;
    }
}
