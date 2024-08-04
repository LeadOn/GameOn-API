// <copyright file="UpdateGameDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs
{
    /// <summary>
    /// UpdateGameDto class.
    /// </summary>
    public class UpdateGameDto
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Platform Id.
        /// </summary>
        public int PlatformId { get; set; }

        /// <summary>
        /// Gets or sets Team Code 1.
        /// </summary>
        public string TeamCode1 { get; set; } = "????";

        /// <summary>
        /// Gets or sets FifaTeam 1.
        /// </summary>
        public int? FifaTeam1 { get; set; }

        /// <summary>
        /// Gets or sets Team Code 2.
        /// </summary>
        public string TeamCode2 { get; set; } = "????";

        /// <summary>
        /// Gets or sets FifaTeam 2.
        /// </summary>
        public int? FifaTeam2 { get; set; }

        /// <summary>
        /// Gets or sets Team score 1.
        /// </summary>
        public int TeamScore1 { get; set; }

        /// <summary>
        /// Gets or sets Team score 2.
        /// </summary>
        public int TeamScore2 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether game is played or not.
        /// </summary>
        public bool IsPlayed { get; set; }

        /// <summary>
        /// Gets or sets Tournament ID.
        /// </summary>
        public int? TournamentId { get; set; }

        /// <summary>
        /// Gets or sets Phase.
        /// </summary>
        public int? Phase { get; set; }
    }
}