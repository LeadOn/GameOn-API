// <copyright file="HomeDataDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.Common
{
    using GameOn.Domain;

    /// <summary>
    /// HomeDataDto class.
    /// </summary>
    public class HomeDataDto
    {
        /// <summary>
        /// Gets or sets latest changelog.
        /// </summary>
        public Changelog LatestChangelog { get; set; } = new Changelog();

        /// <summary>
        /// Gets or sets current season.
        /// </summary>
        public Season? CurrentSeason { get; set; }

        /// <summary>
        /// Gets or sets featured tournaments.
        /// </summary>
        public List<TournamentDto> FeaturedTournaments { get; set; } = new List<TournamentDto>();
    }
}