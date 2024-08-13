// <copyright file="SoccerFiveVoteAnswer.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// SoccerFiveVoteAnswer class.
    /// </summary>
    public class SoccerFiveVoteAnswer
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets VoteChoiceId.
        /// </summary>
        public int VoteChoiceId { get; set; }

        /// <summary>
        /// Gets or sets PlayerId.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets Vote choice.
        /// </summary>
        [JsonIgnore]
        public virtual SoccerFiveVoteChoice VoteChoice { get; set; } = null!;

        /// <summary>
        /// Gets or sets Player.
        /// </summary>
        public virtual Player Player { get; set; } = null!;
    }
}
