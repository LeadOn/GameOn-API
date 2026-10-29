// <copyright file="ValGameRound.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Round breakdown entry for a Valorant match.
    /// </summary>
    public class ValGameRound
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
        /// Gets or sets the 0-based round index.
        /// </summary>
        public int RoundNumber { get; set; }

        /// <summary>
        /// Gets or sets the round outcome (e.g. "Elimination", "Bomb detonated", "Bomb defused").
        /// </summary>
        public string? Result { get; set; }

        /// <summary>
        /// Gets or sets the round ceremony (e.g. "CeremonyClutch", "CeremonyAce", "CeremonyThrifty", "CeremonyFlawless").
        /// </summary>
        public string? Ceremony { get; set; }

        /// <summary>
        /// Gets or sets the winning team identifier ("Red" or "Blue").
        /// </summary>
        public string? WinningTeam { get; set; }

        /// <summary>
        /// Gets or sets the winning team's side ("Attacker" or "Defender").
        /// </summary>
        public string? WinningTeamRole { get; set; }

        /// <summary>
        /// Gets or sets the PUUID of the player who scored first blood in the round.
        /// </summary>
        public string? FirstBloodPuuid { get; set; }

        /// <summary>
        /// Gets or sets the team of the player who scored first blood in the round.
        /// </summary>
        public string? FirstBloodTeam { get; set; }

        /// <summary>
        /// Gets or sets the PUUID of the player highlighted by the round ceremony (e.g. clutch or ace player).
        /// </summary>
        public string? CeremonyPlayerPuuid { get; set; }

        /// <summary>
        /// Gets or sets the team highlighted by the round ceremony.
        /// </summary>
        public string? CeremonyTeam { get; set; }

        /// <summary>
        /// Gets or sets the elapsed round time in milliseconds when the spike was planted, if any.
        /// </summary>
        public int? PlantTimeMs { get; set; }

        /// <summary>
        /// Gets or sets the site ("A", "B", "C") where the spike was planted, if any.
        /// </summary>
        public string? PlantSite { get; set; }

        /// <summary>
        /// Gets or sets the PUUID of the player who planted the spike, if any.
        /// </summary>
        public string? PlantPlayerPuuid { get; set; }

        /// <summary>
        /// Gets or sets the elapsed round time in milliseconds when the spike was defused, if any.
        /// </summary>
        public int? DefuseTimeMs { get; set; }

        /// <summary>
        /// Gets or sets the PUUID of the player who defused the spike, if any.
        /// </summary>
        public string? DefusePlayerPuuid { get; set; }

        /// <summary>
        /// Gets or sets the parent match.
        /// </summary>
        [JsonIgnore]
        public virtual ValGame Game { get; set; } = null!;
    }
}
