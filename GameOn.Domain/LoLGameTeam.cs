// <copyright file="LoLGameTeam.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// LoLGameTeam class. Per-team objective counters for a match, mirrored as-is from Riot's
    /// match-v5 <c>teams[]</c> payload (<c>TeamDto</c>/<c>ObjectivesDto</c>), so the front doesn't
    /// have to scan the full timeline to display them and they're available even without a timeline.
    /// </summary>
    public class LoLGameTeam
    {
        /// <summary>
        /// Gets or sets ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Match ID.
        /// </summary>
        public string MatchId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Riot team ID (100 or 200).
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this team won the game.
        /// </summary>
        public bool Win { get; set; }

        /// <summary>
        /// Gets or sets the team's total champion kills.
        /// </summary>
        public int ChampionKills { get; set; }

        /// <summary>
        /// Gets or sets the team's total tower kills.
        /// </summary>
        public int TowerKills { get; set; }

        /// <summary>
        /// Gets or sets the team's total inhibitor kills.
        /// </summary>
        public int InhibitorKills { get; set; }

        /// <summary>
        /// Gets or sets the team's total dragon kills.
        /// </summary>
        public int DragonKills { get; set; }

        /// <summary>
        /// Gets or sets the team's total Rift Herald kills.
        /// </summary>
        public int RiftHeraldKills { get; set; }

        /// <summary>
        /// Gets or sets the team's total Baron kills.
        /// </summary>
        public int BaronKills { get; set; }

        /// <summary>
        /// Gets or sets the team's total Horde (void grubs) kills.
        /// </summary>
        public int HordeKills { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this team got first blood.
        /// </summary>
        public bool FirstBlood { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this team destroyed the first tower.
        /// </summary>
        public bool FirstTower { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this team destroyed the first inhibitor.
        /// </summary>
        public bool FirstInhibitor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this team took the first dragon.
        /// </summary>
        public bool FirstDragon { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this team took the first Baron.
        /// </summary>
        public bool FirstBaron { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this team took the first Rift Herald.
        /// </summary>
        public bool FirstRiftHerald { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this team took the first Horde (void grub).
        /// </summary>
        public bool FirstHorde { get; set; }

        /// <summary>
        /// Gets or sets the game this team belongs to.
        /// </summary>
        [JsonIgnore]
        public virtual LoLGame Game { get; set; } = null!;
    }
}
