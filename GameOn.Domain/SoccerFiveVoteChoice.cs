// <copyright file="SoccerFiveVoteChoice.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// SoccerFiveVoteChoice class.
    /// </summary>
    public class SoccerFiveVoteChoice
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets SoccerFiveId.
        /// </summary>
        public int SoccerFiveId { get; set; }

        /// <summary>
        /// Gets or sets label.
        /// </summary>
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets order.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets Created By.
        /// </summary>
        [JsonIgnore]
        public virtual SoccerFive? SoccerFive { get; set; } = null!;

        /// <summary>
        /// Gets or sets Answers.
        /// </summary>
        public virtual List<SoccerFiveVoteAnswer>? Answers { get; set; } = null!;
    }
}
