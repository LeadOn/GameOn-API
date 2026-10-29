// <copyright file="ValorantMatchRoundDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// Round summary inside a HenrikDev v4 match payload.
    /// </summary>
    public class ValorantMatchRoundDto
    {
        /// <summary>
        /// Gets or sets the 0-based round index.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the round outcome (e.g. "Elimination", "Bomb detonated", "Bomb defused").
        /// </summary>
        [JsonProperty("result")]
        public string? Result { get; set; }

        /// <summary>
        /// Gets or sets the round ceremony (e.g. "CeremonyClutch", "CeremonyAce", "CeremonyThrifty", "CeremonyFlawless").
        /// </summary>
        [JsonProperty("ceremony")]
        public string? Ceremony { get; set; }

        /// <summary>
        /// Gets or sets the winning team identifier ("Red" or "Blue").
        /// </summary>
        [JsonProperty("winning_team")]
        public string? WinningTeam { get; set; }

        /// <summary>
        /// Gets or sets the winning team's side ("Attacker" or "Defender").
        /// </summary>
        [JsonProperty("winning_team_role")]
        public string? WinningTeamRole { get; set; }

        /// <summary>
        /// Gets or sets the player who scored first blood in the round, if any.
        /// </summary>
        [JsonProperty("first_blood")]
        public ValorantMatchPlayerRefDto? FirstBlood { get; set; }

        /// <summary>
        /// Gets or sets the player highlighted by the round ceremony (e.g. the clutch or ace player), if any.
        /// </summary>
        [JsonProperty("ceremony_player")]
        public ValorantMatchPlayerRefDto? CeremonyPlayer { get; set; }

        /// <summary>
        /// Gets or sets the team highlighted by the round ceremony, if any.
        /// </summary>
        [JsonProperty("ceremony_team")]
        public string? CeremonyTeam { get; set; }

        /// <summary>
        /// Gets or sets the spike plant event, if the spike was planted during the round.
        /// </summary>
        [JsonProperty("plant")]
        public ValorantMatchSpikeEventDto? Plant { get; set; }

        /// <summary>
        /// Gets or sets the spike defuse event, if the spike was defused during the round.
        /// </summary>
        [JsonProperty("defuse")]
        public ValorantMatchSpikeEventDto? Defuse { get; set; }
    }
}
