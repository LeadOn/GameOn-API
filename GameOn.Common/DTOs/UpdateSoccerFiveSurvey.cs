// <copyright file="UpdateSoccerFiveSurvey.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs
{
    using GameOn.Domain;

    /// <summary>
    /// Gets or sets UpdateSoccerFiveSurvey.
    /// </summary>
    public class UpdateSoccerFiveSurvey
    {
        /// <summary>
        /// Gets or sets SoccerFiveId.
        /// </summary>
        public int SoccerFiveId { get; set; }

        /// <summary>
        /// Gets or sets VoteQuestion.
        /// </summary>
        public string VoteQuestion { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets VoteChoices.
        /// </summary>
        public List<SoccerFiveVoteChoice> VotesChoices { get; set; } = new List<SoccerFiveVoteChoice>();
    }
}
