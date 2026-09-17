// <copyright file="LcuEventDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends.LeagueClient
{
    using System.Collections.Generic;

    /// <summary>
    /// LcuEventDto class. A timeline event, as served by the League client. The client only keeps
    /// CHAMPION_KILL, BUILDING_KILL and ELITE_MONSTER_KILL: no ward, item, level up or skill events,
    /// unlike match-v5.
    /// </summary>
    public class LcuEventDto
    {
        /// <summary>
        /// Gets or sets the event type.
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// Gets or sets the event timestamp, in milliseconds since the start of the game.
        /// </summary>
        public int Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the participant the event is about, if any.
        /// </summary>
        public int? ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the killer, if any.
        /// </summary>
        public int? KillerId { get; set; }

        /// <summary>
        /// Gets or sets the victim, if any.
        /// </summary>
        public int? VictimId { get; set; }

        /// <summary>
        /// Gets or sets the participants credited with an assist.
        /// </summary>
        public List<int>? AssistingParticipantIds { get; set; }

        /// <summary>
        /// Gets or sets the team the event is about, if any.
        /// </summary>
        public int? TeamId { get; set; }

        /// <summary>
        /// Gets or sets the item involved, if any.
        /// </summary>
        public int? ItemId { get; set; }

        /// <summary>
        /// Gets or sets the skill slot involved, if any.
        /// </summary>
        public int? SkillSlot { get; set; }

        /// <summary>
        /// Gets or sets the building type, for BUILDING_KILL.
        /// </summary>
        public string? BuildingType { get; set; }

        /// <summary>
        /// Gets or sets the tower type, for BUILDING_KILL.
        /// </summary>
        public string? TowerType { get; set; }

        /// <summary>
        /// Gets or sets the lane, for BUILDING_KILL.
        /// </summary>
        public string? LaneType { get; set; }

        /// <summary>
        /// Gets or sets the monster type, for ELITE_MONSTER_KILL.
        /// </summary>
        public string? MonsterType { get; set; }

        /// <summary>
        /// Gets or sets the monster sub type, for ELITE_MONSTER_KILL.
        /// </summary>
        public string? MonsterSubType { get; set; }

        /// <summary>
        /// Gets or sets where the event happened.
        /// </summary>
        public LcuPositionDto? Position { get; set; }
    }
}
