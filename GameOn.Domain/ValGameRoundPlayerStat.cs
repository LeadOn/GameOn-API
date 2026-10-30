// <copyright file="ValGameRoundPlayerStat.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// One player's round in a Valorant match: buy, combat score, hits and damage dealt.
    /// </summary>
    public class ValGameRoundPlayerStat
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
        /// Gets or sets the 0-based round index, same convention as <see cref="ValGameRound.RoundNumber"/>.
        /// </summary>
        public int RoundNumber { get; set; }

        /// <summary>
        /// Gets or sets the player's Valorant PUUID.
        /// </summary>
        public string Puuid { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the player's team identifier ("Red" or "Blue").
        /// </summary>
        public string? Team { get; set; }

        /// <summary>
        /// Gets or sets the value of the player's loadout at the start of the round, in credits.
        /// </summary>
        public int LoadoutValue { get; set; }

        /// <summary>
        /// Gets or sets the credits left after the buy phase.
        /// </summary>
        public int Remaining { get; set; }

        /// <summary>
        /// Gets or sets the UUID of the main weapon carried, if any.
        /// </summary>
        public string? WeaponId { get; set; }

        /// <summary>
        /// Gets or sets the display name of the main weapon carried, if any.
        /// </summary>
        public string? WeaponName { get; set; }

        /// <summary>
        /// Gets or sets the UUID of the armor carried, if any.
        /// </summary>
        public string? ArmorId { get; set; }

        /// <summary>
        /// Gets or sets the display name of the armor carried, if any.
        /// </summary>
        public string? ArmorName { get; set; }

        /// <summary>
        /// Gets or sets the combat score earned in the round (can be negative).
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Gets or sets the number of kills in the round.
        /// </summary>
        public int Kills { get; set; }

        /// <summary>
        /// Gets or sets the number of head hits in the round.
        /// </summary>
        public int Headshots { get; set; }

        /// <summary>
        /// Gets or sets the number of body hits in the round.
        /// </summary>
        public int Bodyshots { get; set; }

        /// <summary>
        /// Gets or sets the number of leg hits in the round.
        /// </summary>
        public int Legshots { get; set; }

        /// <summary>
        /// Gets or sets the total damage dealt in the round, summed over every player hit.
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the player was AFK during the round.
        /// </summary>
        public bool WasAfk { get; set; }

        /// <summary>
        /// Gets or sets the parent match.
        /// </summary>
        [JsonIgnore]
        public virtual ValGame Game { get; set; } = null!;
    }
}
