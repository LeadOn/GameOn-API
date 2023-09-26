// <copyright file="PlatformStatsDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Common.DTOs
{
    using YuGames.Domain;

    /// <summary>
    /// Platform Stats DTO class.
    /// </summary>
    public class PlatformStatsDto
    {
        /// <summary>
        /// Gets or sets Platform.
        /// </summary>
        public Platform Platform { get; set; } = new Platform();

        /// <summary>
        /// Gets or sets wins.
        /// </summary>
        public int Wins { get; set; }

        /// <summary>
        /// Gets or sets losses.
        /// </summary>
        public int Losses { get; set; }

        /// <summary>
        /// Gets or sets draws.
        /// </summary>
        public int Draws { get; set; }

        /// <summary>
        /// Gets or sets Average Goal Given.
        /// </summary>
        public float AverageGoalGiven { get; set; }

        /// <summary>
        /// Gets or sets Average Goal Given.
        /// </summary>
        public float AverageGoalTaken { get; set; }

        /// <summary>
        /// Gets or sets Goals given.
        /// </summary>
        public int GoalsGiven { get; set; }

        /// <summary>
        /// Gets or sets Goals taken.
        /// </summary>
        public int GoalsTaken { get; set; }

        /// <summary>
        /// Gets or sets Goal difference.
        /// </summary>
        public int GoalDifference { get; set; }

        /// <summary>
        /// Gets or sets win rate.
        /// </summary>
        public float WinRate { get; set; }

        /// <summary>
        /// Gets or sets loose rate.
        /// </summary>
        public float LooseRate { get; set; }

        /// <summary>
        /// Gets or sets draw rate.
        /// </summary>
        public float DrawRate { get; set; }

        /// <summary>
        /// Gets or sets number of Match played.
        /// </summary>
        public int? MatchPlayed { get; set; }

        /// <summary>
        /// Gets or sets number of Match not played.
        /// </summary>
        public int? MatchNotPlayed { get; set; }

        /// <summary>
        /// Gets or sets number of Total Match.
        /// </summary>
        public int? TotalMatch { get; set; }

        /// <summary>
        /// Gets or sets progression.
        /// </summary>
        public float? Progression { get; set; }
    }
}
