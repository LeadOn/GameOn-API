// <copyright file="DeclareScoreDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs
{
    /// <summary>
    /// DeclareScoreDto class.
    /// </summary>
    public class DeclareScoreDto
    {
        /// <summary>
        /// Gets or sets Game Id.
        /// </summary>
        public int GameId { get; set; }

        /// <summary>
        /// Gets or sets Score Team 1.
        /// </summary>
        public int ScoreTeam1 { get; set; }

        /// <summary>
        /// Gets or sets Score Team 2.
        /// </summary>
        public int ScoreTeam2 { get; set; }

        /// <summary>
        /// Gets or sets Current Player ID.
        /// </summary>
        public int CurrentPlayerId { get; set; }
    }
}