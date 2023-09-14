// <copyright file="TournamentDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.DTOs
{
    using YuGames.Entities;

    /// <summary>
    /// TournamentDto class.
    /// </summary>
    public class TournamentDto
    {
        public TournamentDto() {}

        public TournamentDto(Tournament tour)
        {
            this.Name = tour.Name;
            this.Description = tour.Description;
            this.State = tour.State;
            this.LogoUrl = tour.LogoUrl;
            this.PlannedFrom = tour.PlannedFrom;
            this.PlannedTo = tour.PlannedTo;
        }
        
        public string Name { get; set; }
        public string? Description { get; set; }
        public int State { get; set; }
        public string? LogoUrl { get; set; }
        public DateTime PlannedFrom { get; set; }
        public DateTime PlannedTo { get; set; }
    }
}