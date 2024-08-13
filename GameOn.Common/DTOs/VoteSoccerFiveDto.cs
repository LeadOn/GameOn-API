// <copyright file="VoteSoccerFiveDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs
{
    /// <summary>
    /// VoteSoccerFiveDto class.
    /// </summary>
    public class VoteSoccerFiveDto
    {
        /// <summary>
        /// Gets or sets Player ID.
        /// </summary>
        public int? PlayerId { get; set; }

        /// <summary>
        /// Gets or sets SoccerFiveId.
        /// </summary>
        public int SoccerFiveId { get; set; }

        /// <summary>
        /// Gets or sets Answers IDs.
        /// </summary>
        public List<int> ChoiceIds { get; set; } = new List<int>();
    }
}
