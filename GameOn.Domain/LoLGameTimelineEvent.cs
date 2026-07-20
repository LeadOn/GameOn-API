// <copyright file="LoLGameTimelineEvent.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// LoLGameTimelineEvent class.
    /// </summary>
    public class LoLGameTimelineEvent
    {
        /// <summary>
        /// Gets or sets ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets LoL Game Timeline frame ID.
        /// </summary>
        public int LoLGameTimelineFrameId { get; set; }

        /// <summary>
        /// Gets or sets Match ID (denormalized from the frame, needed to link to <see cref="LoLGameParticipant"/> via PUUID).
        /// </summary>
        public string MatchId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets timestamp.
        /// </summary>
        public int Timestamp { get; set; }

        /// <summary>
        /// Gets or sets real timestamp (epoch ms), only present on some event types.
        /// </summary>
        public long? RealTimestamp { get; set; }

        /// <summary>
        /// Gets or sets event type (e.g. CHAMPION_KILL, WARD_PLACED, ITEM_PURCHASED...).
        /// </summary>
        public string EventType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets participant ID.
        /// </summary>
        public int? ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets participant PUUID (resolved from <see cref="ParticipantId"/>).
        /// </summary>
        public string? ParticipantPUUID { get; set; }

        /// <summary>
        /// Gets or sets killer participant ID.
        /// </summary>
        public int? KillerId { get; set; }

        /// <summary>
        /// Gets or sets killer PUUID (resolved from <see cref="KillerId"/>).
        /// </summary>
        public string? KillerPUUID { get; set; }

        /// <summary>
        /// Gets or sets victim participant ID.
        /// </summary>
        public int? VictimId { get; set; }

        /// <summary>
        /// Gets or sets victim PUUID (resolved from <see cref="VictimId"/>).
        /// </summary>
        public string? VictimPUUID { get; set; }

        /// <summary>
        /// Gets or sets killer team ID.
        /// </summary>
        public int? KillerTeamId { get; set; }

        /// <summary>
        /// Gets or sets team ID.
        /// </summary>
        public int? TeamId { get; set; }

        /// <summary>
        /// Gets or sets bounty.
        /// </summary>
        public int? Bounty { get; set; }

        /// <summary>
        /// Gets or sets shutdown bounty.
        /// </summary>
        public int? ShutdownBounty { get; set; }

        /// <summary>
        /// Gets or sets kill streak length.
        /// </summary>
        public int? KillStreakLength { get; set; }

        /// <summary>
        /// Gets or sets multi kill length.
        /// </summary>
        public int? MultiKillLength { get; set; }

        /// <summary>
        /// Gets or sets kill type (CHAMPION_SPECIAL_KILL).
        /// </summary>
        public string? KillType { get; set; }

        /// <summary>
        /// Gets or sets item ID.
        /// </summary>
        public int? ItemId { get; set; }

        /// <summary>
        /// Gets or sets item ID before an undo.
        /// </summary>
        public int? BeforeId { get; set; }

        /// <summary>
        /// Gets or sets item ID after an undo.
        /// </summary>
        public int? AfterId { get; set; }

        /// <summary>
        /// Gets or sets gold gained/lost from an item undo.
        /// </summary>
        public int? GoldGain { get; set; }

        /// <summary>
        /// Gets or sets skill slot.
        /// </summary>
        public int? SkillSlot { get; set; }

        /// <summary>
        /// Gets or sets level up type.
        /// </summary>
        public string? LevelUpType { get; set; }

        /// <summary>
        /// Gets or sets level.
        /// </summary>
        public int? Level { get; set; }

        /// <summary>
        /// Gets or sets ward type.
        /// </summary>
        public string? WardType { get; set; }

        /// <summary>
        /// Gets or sets ward creator participant ID.
        /// </summary>
        public int? CreatorId { get; set; }

        /// <summary>
        /// Gets or sets ward creator PUUID (resolved from <see cref="CreatorId"/>).
        /// </summary>
        public string? CreatorPUUID { get; set; }

        /// <summary>
        /// Gets or sets building type (BUILDING_KILL).
        /// </summary>
        public string? BuildingType { get; set; }

        /// <summary>
        /// Gets or sets tower type (BUILDING_KILL).
        /// </summary>
        public string? TowerType { get; set; }

        /// <summary>
        /// Gets or sets lane type.
        /// </summary>
        public string? LaneType { get; set; }

        /// <summary>
        /// Gets or sets monster type (ELITE_MONSTER_KILL).
        /// </summary>
        public string? MonsterType { get; set; }

        /// <summary>
        /// Gets or sets monster sub type (ELITE_MONSTER_KILL).
        /// </summary>
        public string? MonsterSubType { get; set; }

        /// <summary>
        /// Gets or sets champion transform type.
        /// </summary>
        public string? TransformType { get; set; }

        /// <summary>
        /// Gets or sets dragon soul type (DRAGON_SOUL_GIVEN).
        /// </summary>
        public string? DragonSoulType { get; set; }

        /// <summary>
        /// Gets or sets position X.
        /// </summary>
        public int? PositionX { get; set; }

        /// <summary>
        /// Gets or sets position Y.
        /// </summary>
        public int? PositionY { get; set; }

        /// <summary>
        /// Gets or sets Frame.
        /// </summary>
        [JsonIgnore]
        public virtual LoLGameTimelineFrame TimelineFrame { get; set; } = null!;

        /// <summary>
        /// Gets or sets the game participant this event pertains to (resolved via <see cref="MatchId"/> + <see cref="ParticipantPUUID"/>).
        /// </summary>
        public virtual LoLGameParticipant? Participant { get; set; }

        /// <summary>
        /// Gets or sets the game participant credited as killer (resolved via <see cref="MatchId"/> + <see cref="KillerPUUID"/>).
        /// </summary>
        public virtual LoLGameParticipant? Killer { get; set; }

        /// <summary>
        /// Gets or sets the game participant who is the victim (resolved via <see cref="MatchId"/> + <see cref="VictimPUUID"/>).
        /// </summary>
        public virtual LoLGameParticipant? Victim { get; set; }

        /// <summary>
        /// Gets or sets the game participant who created the ward (resolved via <see cref="MatchId"/> + <see cref="CreatorPUUID"/>).
        /// </summary>
        public virtual LoLGameParticipant? Creator { get; set; }

        /// <summary>
        /// Gets or sets assisting participants.
        /// </summary>
        public virtual List<LoLGameTimelineEventAssist> LoLGameTimelineEventAssists { get; set; } = null!;
    }
}
