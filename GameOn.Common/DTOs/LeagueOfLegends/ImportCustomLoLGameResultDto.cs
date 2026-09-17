// <copyright file="ImportCustomLoLGameResultDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// ImportCustomLoLGameResultDto class. What a custom game import actually did, so the caller
    /// doesn't have to query the game back to find out.
    /// </summary>
    public class ImportCustomLoLGameResultDto
    {
        /// <summary>
        /// Gets or sets the match ID of the imported game.
        /// </summary>
        public string MatchId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the game was already in database and got replaced.
        /// </summary>
        public bool Replaced { get; set; }

        /// <summary>
        /// Gets or sets the start of the game.
        /// </summary>
        public DateTime GameStart { get; set; }

        /// <summary>
        /// Gets or sets the queue ID, or null when the queue is unknown to the LoLQueue referential.
        /// </summary>
        public int? QueueId { get; set; }

        /// <summary>
        /// Gets or sets the number of imported participants.
        /// </summary>
        public int ParticipantCount { get; set; }

        /// <summary>
        /// Gets or sets the number of participants matched to a GameOn! player.
        /// </summary>
        public int LinkedPlayerCount { get; set; }

        /// <summary>
        /// Gets or sets the number of imported timeline frames.
        /// </summary>
        public int TimelineFrameCount { get; set; }

        /// <summary>
        /// Gets or sets the Riot IDs of the participants that couldn't be matched to a GameOn! player.
        /// </summary>
        public List<string> UnlinkedRiotIds { get; set; } = new List<string>();
    }
}
