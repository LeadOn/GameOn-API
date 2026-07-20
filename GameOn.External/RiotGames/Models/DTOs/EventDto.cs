// <copyright file="EventDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// EventDto class.
    /// </summary>
    public class EventDto
    {
        /// <summary>
        /// Gets or sets timestamp.
        /// </summary>
        [JsonProperty("timestamp")]
        public int Timestamp { get; set; }

        /// <summary>
        /// Gets or sets real timestamp (epoch ms), only present on some event types.
        /// </summary>
        [JsonProperty("realTimestamp")]
        public long? RealTimestamp { get; set; }

        /// <summary>
        /// Gets or sets event type (e.g. CHAMPION_KILL, WARD_PLACED, ITEM_PURCHASED...).
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets participant ID.
        /// </summary>
        [JsonProperty("participantId")]
        public int? ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets killer participant ID.
        /// </summary>
        [JsonProperty("killerId")]
        public int? KillerId { get; set; }

        /// <summary>
        /// Gets or sets victim participant ID.
        /// </summary>
        [JsonProperty("victimId")]
        public int? VictimId { get; set; }

        /// <summary>
        /// Gets or sets killer team ID.
        /// </summary>
        [JsonProperty("killerTeamId")]
        public int? KillerTeamId { get; set; }

        /// <summary>
        /// Gets or sets team ID.
        /// </summary>
        [JsonProperty("teamId")]
        public int? TeamId { get; set; }

        /// <summary>
        /// Gets or sets bounty.
        /// </summary>
        [JsonProperty("bounty")]
        public int? Bounty { get; set; }

        /// <summary>
        /// Gets or sets shutdown bounty.
        /// </summary>
        [JsonProperty("shutdownBounty")]
        public int? ShutdownBounty { get; set; }

        /// <summary>
        /// Gets or sets kill streak length.
        /// </summary>
        [JsonProperty("killStreakLength")]
        public int? KillStreakLength { get; set; }

        /// <summary>
        /// Gets or sets multi kill length.
        /// </summary>
        [JsonProperty("multiKillLength")]
        public int? MultiKillLength { get; set; }

        /// <summary>
        /// Gets or sets kill type (CHAMPION_SPECIAL_KILL).
        /// </summary>
        [JsonProperty("killType")]
        public string? KillType { get; set; }

        /// <summary>
        /// Gets or sets item ID.
        /// </summary>
        [JsonProperty("itemId")]
        public int? ItemId { get; set; }

        /// <summary>
        /// Gets or sets item ID before an undo.
        /// </summary>
        [JsonProperty("beforeId")]
        public int? BeforeId { get; set; }

        /// <summary>
        /// Gets or sets item ID after an undo.
        /// </summary>
        [JsonProperty("afterId")]
        public int? AfterId { get; set; }

        /// <summary>
        /// Gets or sets gold gained/lost from an item undo.
        /// </summary>
        [JsonProperty("goldGain")]
        public int? GoldGain { get; set; }

        /// <summary>
        /// Gets or sets skill slot.
        /// </summary>
        [JsonProperty("skillSlot")]
        public int? SkillSlot { get; set; }

        /// <summary>
        /// Gets or sets level up type.
        /// </summary>
        [JsonProperty("levelUpType")]
        public string? LevelUpType { get; set; }

        /// <summary>
        /// Gets or sets level.
        /// </summary>
        [JsonProperty("level")]
        public int? Level { get; set; }

        /// <summary>
        /// Gets or sets ward type.
        /// </summary>
        [JsonProperty("wardType")]
        public string? WardType { get; set; }

        /// <summary>
        /// Gets or sets ward creator participant ID.
        /// </summary>
        [JsonProperty("creatorId")]
        public int? CreatorId { get; set; }

        /// <summary>
        /// Gets or sets building type (BUILDING_KILL).
        /// </summary>
        [JsonProperty("buildingType")]
        public string? BuildingType { get; set; }

        /// <summary>
        /// Gets or sets tower type (BUILDING_KILL).
        /// </summary>
        [JsonProperty("towerType")]
        public string? TowerType { get; set; }

        /// <summary>
        /// Gets or sets lane type.
        /// </summary>
        [JsonProperty("laneType")]
        public string? LaneType { get; set; }

        /// <summary>
        /// Gets or sets monster type (ELITE_MONSTER_KILL).
        /// </summary>
        [JsonProperty("monsterType")]
        public string? MonsterType { get; set; }

        /// <summary>
        /// Gets or sets monster sub type (ELITE_MONSTER_KILL).
        /// </summary>
        [JsonProperty("monsterSubType")]
        public string? MonsterSubType { get; set; }

        /// <summary>
        /// Gets or sets champion transform type.
        /// </summary>
        [JsonProperty("transformType")]
        public string? TransformType { get; set; }

        /// <summary>
        /// Gets or sets dragon soul type (DRAGON_SOUL_GIVEN).
        /// </summary>
        [JsonProperty("name")]
        public string? DragonSoulType { get; set; }

        /// <summary>
        /// Gets or sets assisting participant IDs.
        /// </summary>
        [JsonProperty("assistingParticipantIds")]
        public List<int>? AssistingParticipantIds { get; set; }

        /// <summary>
        /// Gets or sets position.
        /// </summary>
        [JsonProperty("position")]
        public PositionDto? Position { get; set; }
    }
}
