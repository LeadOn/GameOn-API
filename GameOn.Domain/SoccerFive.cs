// <copyright file="SoccerFive.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    /// <summary>
    /// SoccerFive class.
    /// </summary>
    public class SoccerFive
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets created by ID.
        /// </summary>
        public int CreatedById { get; set; }

        /// <summary>
        /// Gets or sets planned date.
        /// </summary>
        public DateTime? PlannedOn { get; set; }

        /// <summary>
        /// Gets or sets vote question.
        /// </summary>
        public string? VoteQuestion { get; set; }

        /// <summary>
        /// Gets or sets State.
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// Gets or sets Created By.
        /// </summary>
        public virtual Player CreatedBy { get; set; } = null!;

        /// <summary>
        /// Gets or sets Vote choices.
        /// </summary>
        public virtual List<SoccerFiveVoteChoice> VotesChoices { get; set; } = null!;
    }
}
