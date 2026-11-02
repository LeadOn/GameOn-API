// <copyright file="ValWeaponStatDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Valorant
{
    /// <summary>
    /// A player's kills with one weapon (or ability, spike, fall) over the requested games.
    /// </summary>
    public class ValWeaponStatDto
    {
        /// <summary>
        /// Gets or sets the weapon UUID (or ability slot for an ability).
        /// </summary>
        public string? WeaponId { get; set; }

        /// <summary>
        /// Gets or sets the weapon name.
        /// </summary>
        public string? WeaponName { get; set; }

        /// <summary>
        /// Gets or sets the kind of kill: Weapon, Ability, Bomb, Melee or Fall.
        /// </summary>
        public string? WeaponType { get; set; }

        /// <summary>
        /// Gets or sets the kills.
        /// </summary>
        public int Kills { get; set; }

        /// <summary>
        /// Gets or sets the share of the player's kills, in percent.
        /// </summary>
        public double KillSharePercent { get; set; }

        /// <summary>
        /// Gets or sets the average kill distance, in meters, null without positions.
        /// </summary>
        public double? AverageDistanceMeters { get; set; }

        /// <summary>
        /// Gets or sets the longest kill distance, in meters.
        /// </summary>
        public double? LongestDistanceMeters { get; set; }

        /// <summary>
        /// Gets or sets the weapon icon URL on <c>media.valorant-api.com</c>, null for abilities and other kinds.
        /// </summary>
        public string? IconUrl { get; set; }
    }
}
