// <copyright file="ChampionStatsDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// ChampionStatsDto class.
    /// </summary>
    public class ChampionStatsDto
    {
        /// <summary>
        /// Gets or sets ability haste.
        /// </summary>
        [JsonProperty("abilityHaste")]
        public int AbilityHaste { get; set; }

        /// <summary>
        /// Gets or sets ability power.
        /// </summary>
        [JsonProperty("abilityPower")]
        public int AbilityPower { get; set; }

        /// <summary>
        /// Gets or sets armor.
        /// </summary>
        [JsonProperty("armor")]
        public int Armor { get; set; }

        /// <summary>
        /// Gets or sets armor penetration.
        /// </summary>
        [JsonProperty("armorPen")]
        public int ArmorPen { get; set; }

        /// <summary>
        /// Gets or sets armor penetration percent.
        /// </summary>
        [JsonProperty("armorPenPercent")]
        public int ArmorPenPercent { get; set; }

        /// <summary>
        /// Gets or sets attack damage.
        /// </summary>
        [JsonProperty("attackDamage")]
        public int AttackDamage { get; set; }

        /// <summary>
        /// Gets or sets attack speed.
        /// </summary>
        [JsonProperty("attackSpeed")]
        public int AttackSpeed { get; set; }

        /// <summary>
        /// Gets or sets bonus armor penetration percent.
        /// </summary>
        [JsonProperty("bonusArmorPenPercent")]
        public int BonusArmorPenPercent { get; set; }

        /// <summary>
        /// Gets or sets bonus magic penetration percent.
        /// </summary>
        [JsonProperty("bonusMagicPenPercent")]
        public int BonusMagicPenPercent { get; set; }

        /// <summary>
        /// Gets or sets crowd control reduction.
        /// </summary>
        [JsonProperty("ccReduction")]
        public int CcReduction { get; set; }

        /// <summary>
        /// Gets or sets cooldown reduction.
        /// </summary>
        [JsonProperty("cooldownReduction")]
        public int CooldownReduction { get; set; }

        /// <summary>
        /// Gets or sets health.
        /// </summary>
        [JsonProperty("health")]
        public int Health { get; set; }

        /// <summary>
        /// Gets or sets max health.
        /// </summary>
        [JsonProperty("healthMax")]
        public int HealthMax { get; set; }

        /// <summary>
        /// Gets or sets health regeneration.
        /// </summary>
        [JsonProperty("healthRegen")]
        public int HealthRegen { get; set; }

        /// <summary>
        /// Gets or sets lifesteal.
        /// </summary>
        [JsonProperty("lifesteal")]
        public int Lifesteal { get; set; }

        /// <summary>
        /// Gets or sets magic penetration.
        /// </summary>
        [JsonProperty("magicPen")]
        public int MagicPen { get; set; }

        /// <summary>
        /// Gets or sets magic penetration percent.
        /// </summary>
        [JsonProperty("magicPenPercent")]
        public int MagicPenPercent { get; set; }

        /// <summary>
        /// Gets or sets magic resist.
        /// </summary>
        [JsonProperty("magicResist")]
        public int MagicResist { get; set; }

        /// <summary>
        /// Gets or sets movement speed.
        /// </summary>
        [JsonProperty("movementSpeed")]
        public int MovementSpeed { get; set; }

        /// <summary>
        /// Gets or sets omnivamp.
        /// </summary>
        [JsonProperty("omnivamp")]
        public int Omnivamp { get; set; }

        /// <summary>
        /// Gets or sets physical vamp.
        /// </summary>
        [JsonProperty("physicalVamp")]
        public int PhysicalVamp { get; set; }

        /// <summary>
        /// Gets or sets power.
        /// </summary>
        [JsonProperty("power")]
        public int Power { get; set; }

        /// <summary>
        /// Gets or sets max power.
        /// </summary>
        [JsonProperty("powerMax")]
        public int PowerMax { get; set; }

        /// <summary>
        /// Gets or sets power regeneration.
        /// </summary>
        [JsonProperty("powerRegen")]
        public int PowerRegen { get; set; }

        /// <summary>
        /// Gets or sets spell vamp.
        /// </summary>
        [JsonProperty("spellVamp")]
        public int SpellVamp { get; set; }
    }
}
