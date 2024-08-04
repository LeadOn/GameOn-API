// <copyright file="TopTeamStatDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs
{
    using GameOn.Domain;

    /// <summary>
    /// TopTeamStatDto class.
    /// </summary>
    public class TopTeamStatDto
    {
        /// <summary>
        /// Gets or sets Number of Games played.
        /// </summary>
        public int NumberOfGames { get; set; }

        /// <summary>
        /// Gets or sets FifaTeam entity.
        /// </summary>
        public FifaTeam? FifaTeam { get; set; }
    }
}
