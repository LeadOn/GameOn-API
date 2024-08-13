// <copyright file="SoccerFiveDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs
{
    using GameOn.Domain;

    /// <summary>
    /// SoccerFiveDto class.
    /// </summary>
    public class SoccerFiveDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SoccerFiveDto"/> class.
        /// </summary>
        public SoccerFiveDto()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoccerFiveDto"/> class.
        /// </summary>
        /// <param name="soccer"><see cref="SoccerFive"/>.</param>
        public SoccerFiveDto(SoccerFive soccer)
        {
            this.Id = soccer.Id;
            this.Name = soccer.Name;
            this.Description = soccer.Description;
            this.PlannedOn = soccer.PlannedOn;
            this.CreatedBy = soccer.CreatedBy;
            this.State = soccer.State;
            this.VoteQuestion = soccer.VoteQuestion;
            this.VotesChoices = soccer.VotesChoices;
        }

        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets planned on.
        /// </summary>
        public DateTime? PlannedOn { get; set; }

        /// <summary>
        /// Gets or sets vote question.
        /// </summary>
        public string? VoteQuestion { get; set; }

        /// <summary>
        /// Gets or sets Created by.
        /// </summary>
        public Player? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets state.
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// Gets or sets VoteChoices.
        /// </summary>
        public List<SoccerFiveVoteChoice> VotesChoices { get; set; } = new List<SoccerFiveVoteChoice>();
    }
}
