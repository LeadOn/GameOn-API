// <copyright file="Player.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.Entities
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Player class.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Gets or sets player's ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets player's Keycloak ID.
        /// </summary>
        public string KeycloakId { get; set; } = "Unknown ID";

        /// <summary>
        /// Gets or sets player's full name.
        /// </summary>
        public string FullName { get; set; } = "John Doe";

        /// <summary>
        /// Gets or sets player's nickname.
        /// </summary>
        public string? Nickname { get; set; } = "J0hnD03";

        /// <summary>
        /// Gets or sets player's profile picture URL.
        /// </summary>
        public string? ProfilePictureUrl { get; set; }

        /// <summary>
        /// Gets or sets player's creation date.
        /// </summary>
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets TeamPlayers.
        /// </summary>
        [JsonIgnore]
        public virtual List<TeamPlayer> TeamPlayers { get; set; } = null!;

        /// <summary>
        /// Gets or sets Highlights.
        /// </summary>
        [JsonIgnore]
        public virtual List<Highlight> Highlights { get; set; } = null!;
    }
}