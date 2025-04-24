﻿// <copyright file="TournamentDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs
{
    using GameOn.Domain;

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
            this.Id = tour.Id;
            this.Name = tour.Name;
            this.Description = tour.Description;
            this.State = tour.State;
            this.LogoUrl = tour.LogoUrl;
            this.PlannedFrom = tour.PlannedFrom;
            this.PlannedTo = tour.PlannedTo;
            this.Phase2ChallongeUrl = tour.Phase2ChallongeUrl;
            this.Players = new List<TournamentPlayerDto>();
            this.WinnerId = tour.WinnerId;
            this.Winner = tour.Winner;
            this.Rules = tour.Rules;
            this.WinPoints = tour.WinPoints;
            this.LoosePoints = tour.LoosePoints;
            this.DrawPoints = tour.DrawPoints;
            this.Featured = tour.Featured;
        }

        /// <summary>
        /// Gets or sets Tournament ID.
        /// </summary>
        public int Id { get; set; } = 0;

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; } = "DEFAULT";

        /// <summary>
        /// Gets or sets Description.
        /// </summary>
        public string Description { get; set; } = "Aucune description renseignée.";

        /// <summary>
        /// Gets or sets State.
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// Gets or sets Logo URL.
        /// </summary>
        public string? LogoUrl { get; set; }

        /// <summary>
        /// Gets or sets Phase 2 challonge URL.
        /// </summary>
        public string? Phase2ChallongeUrl { get; set; }

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

        /// <summary>
        /// Gets or sets Winner ID.
        /// </summary>
        public int? WinnerId { get; set; }

        /// <summary>
        /// Gets or sets Tournament Rules.
        /// </summary>
        public string? Rules { get; set; }

        /// <summary>
        /// Gets or sets Tournament Win Points.
        /// </summary>
        public int WinPoints { get; set; }

        /// <summary>
        /// Gets or sets Loose Win Points.
        /// </summary>
        public int LoosePoints { get; set; }

        /// <summary>
        /// Gets or sets Draw Win Points.
        /// </summary>
        public int DrawPoints { get; set; }

        /// <summary>
        /// Gets or sets Winner.
        /// </summary>
        public Player? Winner { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a tournament is featured or not.
        /// </summary>
        public bool Featured { get; set; }
    }
}