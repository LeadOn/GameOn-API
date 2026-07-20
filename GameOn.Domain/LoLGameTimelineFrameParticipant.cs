// <copyright file="LoLGameTimelineFrameParticipant.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// LoLGameTimelineFrameParticipant class.
    /// </summary>
    public class LoLGameTimelineFrameParticipant
    {
        /// <summary>
        /// Gets or sets ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets LoL Game Timeline frame.
        /// </summary>
        public int LoLGameTimelineFrameId { get; set; }

        /// <summary>
        /// Gets or sets current gold.
        /// </summary>
        public int CurrentGold { get; set; }

        /// <summary>
        /// Gets or sets gold per second.
        /// </summary>
        public int GoldPerSecond { get; set; }

        /// <summary>
        /// Gets or sets jungle minions killed.
        /// </summary>
        public int JungleMinionsKilled { get; set; }

        /// <summary>
        /// Gets or sets Level.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets Minions killed.
        /// </summary>
        public int MinionsKilled { get; set; }

        /// <summary>
        /// Gets or sets participant ID.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets position X.
        /// </summary>
        public int PositionX { get; set; }

        /// <summary>
        /// Gets or sets position Y.
        /// </summary>
        public int PositionY { get; set; }

        /// <summary>
        /// Gets or sets Time enemy spent controlled.
        /// </summary>
        public int TimeEnemySpentControlled { get; set; }

        /// <summary>
        /// Gets or sets Total Gold.
        /// </summary>
        public int TotalGold { get; set; }

        /// <summary>
        /// Gets or sets XP.
        /// </summary>
        public int Xp { get; set; }

        /// <summary>
        /// Gets or sets participant PUUID.
        /// </summary>
        public string ParticipantPUUID { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets magic damage done.
        /// </summary>
        public int MagicDamageDone { get; set; }

        /// <summary>
        /// Gets or sets magic damage done to champions.
        /// </summary>
        public int MagicDamageDoneToChampions { get; set; }

        /// <summary>
        /// Gets or sets magic damage taken.
        /// </summary>
        public int MagicDamageTaken { get; set; }

        /// <summary>
        /// Gets or sets physical damage done.
        /// </summary>
        public int PhysicalDamageDone { get; set; }

        /// <summary>
        /// Gets or sets physical damage done to champions.
        /// </summary>
        public int PhysicalDamageDoneToChampions { get; set; }

        /// <summary>
        /// Gets or sets physical damage taken.
        /// </summary>
        public int PhysicalDamageTaken { get; set; }

        /// <summary>
        /// Gets or sets total damage done.
        /// </summary>
        public int TotalDamageDone { get; set; }

        /// <summary>
        /// Gets or sets total damage done to champions.
        /// </summary>
        public int TotalDamageDoneToChampions { get; set; }

        /// <summary>
        /// Gets or sets total damage taken.
        /// </summary>
        public int TotalDamageTaken { get; set; }

        /// <summary>
        /// Gets or sets true damage done.
        /// </summary>
        public int TrueDamageDone { get; set; }

        /// <summary>
        /// Gets or sets true damage done to champions.
        /// </summary>
        public int TrueDamageDoneToChampions { get; set; }

        /// <summary>
        /// Gets or sets true damage taken.
        /// </summary>
        public int TrueDamageTaken { get; set; }

        /// <summary>
        /// Gets or sets ability haste.
        /// </summary>
        public int AbilityHaste { get; set; }

        /// <summary>
        /// Gets or sets ability power.
        /// </summary>
        public int AbilityPower { get; set; }

        /// <summary>
        /// Gets or sets armor.
        /// </summary>
        public int Armor { get; set; }

        /// <summary>
        /// Gets or sets armor penetration.
        /// </summary>
        public int ArmorPen { get; set; }

        /// <summary>
        /// Gets or sets armor penetration percent.
        /// </summary>
        public int ArmorPenPercent { get; set; }

        /// <summary>
        /// Gets or sets attack damage.
        /// </summary>
        public int AttackDamage { get; set; }

        /// <summary>
        /// Gets or sets attack speed.
        /// </summary>
        public int AttackSpeed { get; set; }

        /// <summary>
        /// Gets or sets bonus armor penetration percent.
        /// </summary>
        public int BonusArmorPenPercent { get; set; }

        /// <summary>
        /// Gets or sets bonus magic penetration percent.
        /// </summary>
        public int BonusMagicPenPercent { get; set; }

        /// <summary>
        /// Gets or sets crowd control reduction.
        /// </summary>
        public int CcReduction { get; set; }

        /// <summary>
        /// Gets or sets cooldown reduction.
        /// </summary>
        public int CooldownReduction { get; set; }

        /// <summary>
        /// Gets or sets health.
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// Gets or sets max health.
        /// </summary>
        public int HealthMax { get; set; }

        /// <summary>
        /// Gets or sets health regeneration.
        /// </summary>
        public int HealthRegen { get; set; }

        /// <summary>
        /// Gets or sets lifesteal.
        /// </summary>
        public int Lifesteal { get; set; }

        /// <summary>
        /// Gets or sets magic penetration.
        /// </summary>
        public int MagicPen { get; set; }

        /// <summary>
        /// Gets or sets magic penetration percent.
        /// </summary>
        public int MagicPenPercent { get; set; }

        /// <summary>
        /// Gets or sets magic resist.
        /// </summary>
        public int MagicResist { get; set; }

        /// <summary>
        /// Gets or sets movement speed.
        /// </summary>
        public int MovementSpeed { get; set; }

        /// <summary>
        /// Gets or sets omnivamp.
        /// </summary>
        public int Omnivamp { get; set; }

        /// <summary>
        /// Gets or sets physical vamp.
        /// </summary>
        public int PhysicalVamp { get; set; }

        /// <summary>
        /// Gets or sets power.
        /// </summary>
        public int Power { get; set; }

        /// <summary>
        /// Gets or sets max power.
        /// </summary>
        public int PowerMax { get; set; }

        /// <summary>
        /// Gets or sets power regeneration.
        /// </summary>
        public int PowerRegen { get; set; }

        /// <summary>
        /// Gets or sets spell vamp.
        /// </summary>
        public int SpellVamp { get; set; }

        /// <summary>
        /// Gets or sets Frame.
        /// </summary>
        [JsonIgnore]
        public virtual LoLGameTimelineFrame TimelineFrame { get; set; } = null!;
    }
}