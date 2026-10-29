// <copyright file="ValorantMatchPlayerDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Participant entry inside a HenrikDev v4 match payload.
    /// </summary>
    public class ValorantMatchPlayerDto
    {
        /// <summary>
        /// Gets or sets the player's Valorant PUUID.
        /// </summary>
        [JsonProperty("puuid")]
        public string Puuid { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the player's Riot ID game name.
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the player's Riot ID tag line.
        /// </summary>
        [JsonProperty("tag")]
        public string? Tag { get; set; }

        /// <summary>
        /// Gets or sets the player's team identifier ("Red", "Blue", or PUUID in free-for-all modes).
        /// </summary>
        [JsonProperty("team_id")]
        public string? TeamId { get; set; }

        /// <summary>
        /// Gets or sets the platform ("pc" or "console").
        /// </summary>
        [JsonProperty("platform")]
        public string? Platform { get; set; }

        /// <summary>
        /// Gets or sets the premade party UUID. Players sharing the same PartyId queued together.
        /// </summary>
        [JsonProperty("party_id")]
        public string? PartyId { get; set; }

        /// <summary>
        /// Gets or sets the selected agent.
        /// </summary>
        [JsonProperty("agent")]
        public ValorantMatchAgentDto? Agent { get; set; }

        /// <summary>
        /// Gets or sets the player's combat statistics for the match.
        /// </summary>
        [JsonProperty("stats")]
        public ValorantMatchPlayerStatsDto? Stats { get; set; }

        /// <summary>
        /// Gets or sets the player's ability cast counts.
        /// </summary>
        [JsonProperty("ability_casts")]
        public ValorantMatchAbilityCastsDto? AbilityCasts { get; set; }

        /// <summary>
        /// Gets or sets the player's competitive rank tier at the time of the match.
        /// </summary>
        [JsonProperty("tier")]
        public ValorantTierComboDto? Tier { get; set; }

        /// <summary>
        /// Gets or sets the player's equipped card and title.
        /// </summary>
        [JsonProperty("customization")]
        public ValorantMatchCustomizationDto? Customization { get; set; }

        /// <summary>
        /// Gets or sets the player's Valorant account level at the time of the match.
        /// </summary>
        [JsonProperty("account_level")]
        public int? AccountLevel { get; set; }

        /// <summary>
        /// Gets or sets the player's session playtime in milliseconds.
        /// </summary>
        [JsonProperty("session_playtime_in_ms")]
        public long? SessionPlaytimeInMs { get; set; }

        /// <summary>
        /// Gets or sets the player's behavioral metrics (AFK rounds, spawn rounds, friendly fire).
        /// </summary>
        [JsonProperty("behavior")]
        public ValorantMatchBehaviorDto? Behavior { get; set; }

        /// <summary>
        /// Gets or sets the player's economy metrics (credits spent and loadout value).
        /// </summary>
        [JsonProperty("economy")]
        public ValorantMatchEconomyDto? Economy { get; set; }
    }
}
