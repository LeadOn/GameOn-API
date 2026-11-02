// <copyright file="ValorantPremierTeam.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System;

    /// <summary>
    /// A Valorant Premier team one of our players belongs to.
    /// </summary>
    public class ValorantPremierTeam
    {
        /// <summary>
        /// Gets or sets the Premier roster UUID.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the team name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the team tag.
        /// </summary>
        public string Tag { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the team icon UUID.
        /// </summary>
        public string? IconId { get; set; }

        /// <summary>
        /// Gets or sets the team icon image URL, rendered in the team colors.
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the primary color (hexadecimal, without #).
        /// </summary>
        public string? PrimaryColor { get; set; }

        /// <summary>
        /// Gets or sets the secondary color (hexadecimal, without #).
        /// </summary>
        public string? SecondaryColor { get; set; }

        /// <summary>
        /// Gets or sets the tertiary color (hexadecimal, without #).
        /// </summary>
        public string? TertiaryColor { get; set; }

        /// <summary>
        /// Gets or sets the UTC date the team was created.
        /// </summary>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the current Premier season UUID.
        /// </summary>
        public string? CurrentSeasonId { get; set; }

        /// <summary>
        /// Gets or sets the current Premier season name.
        /// </summary>
        public string? CurrentSeasonName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the team is enrolled in the current season.
        /// </summary>
        public bool Enrolled { get; set; }

        /// <summary>
        /// Gets or sets the conference of the current season.
        /// </summary>
        public string? Conference { get; set; }

        /// <summary>
        /// Gets or sets the division of the current season.
        /// </summary>
        public int? Division { get; set; }

        /// <summary>
        /// Gets or sets the points of the current season.
        /// </summary>
        public int? Points { get; set; }

        /// <summary>
        /// Gets or sets the wins of the current season.
        /// </summary>
        public int Wins { get; set; }

        /// <summary>
        /// Gets or sets the losses of the current season.
        /// </summary>
        public int Losses { get; set; }

        /// <summary>
        /// Gets or sets the rounds won over the current season.
        /// </summary>
        public int RoundsWon { get; set; }

        /// <summary>
        /// Gets or sets the rounds lost over the current season.
        /// </summary>
        public int RoundsLost { get; set; }

        /// <summary>
        /// Gets or sets the members (PUUID, role, join date) as a JSON array.
        /// </summary>
        public string? MembersJson { get; set; }

        /// <summary>
        /// Gets or sets every season the team played, as raw JSON.
        /// </summary>
        public string? SeasonsJson { get; set; }

        /// <summary>
        /// Gets or sets the UTC date this row was last refreshed.
        /// </summary>
        public DateTime RefreshedOn { get; set; } = DateTime.UtcNow;
    }
}
