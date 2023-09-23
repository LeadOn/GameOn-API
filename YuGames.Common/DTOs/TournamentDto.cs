// <copyright file="TournamentDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Common.DTOs
{
    using YuGames.Domain;

    /// <summary>
    /// TournamentDto class.
    /// </summary>
    public class TournamentDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentDto"/> class.
        /// </summary>
        public TournamentDto()
        {
            this.Players = new List<TournamentPlayerDto>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentDto"/> class.
        /// </summary>
        /// <param name="tour">Tournament entity.</param>
        public TournamentDto(Tournament tour)
        {
            this.Name = tour.Name;
            this.Description = tour.Description;
            this.State = tour.State;
            this.LogoUrl = tour.LogoUrl;
            this.PlannedFrom = tour.PlannedFrom;
            this.PlannedTo = tour.PlannedTo;
            this.Players = new List<TournamentPlayerDto>();
        }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; } = "DEFAULT";

        /// <summary>
        /// Gets or sets Description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets State.
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// Gets or sets Logo URL.
        /// </summary>
        public string? LogoUrl { get; set; }

        /// <summary>
        /// Gets or sets PlannedFrom.
        /// </summary>
        public DateTime PlannedFrom { get; set; }

        /// <summary>
        /// Gets or sets PlannedTo.
        /// </summary>
        public DateTime PlannedTo { get; set; }

        /// <summary>
        /// Gets or sets Players.
        /// </summary>
        public List<TournamentPlayerDto>? Players { get; set; }
    }
}