// <copyright file="GetLastValGamesPlayedQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Matches.Queries.GetLastValGamesPlayed
{
    using System;
    using System.Collections.Generic;
    using GameOn.Common.DTOs.Common;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// Retrieves a paginated list of Valorant matches played by the crew or by a specific player.
    /// </summary>
    public class GetLastValGamesPlayedQuery : IRequest<ListResultDto<ValGame>?>
    {
        /// <summary>
        /// Gets or sets the optional GameOn! player ID. When null, returns the shared crew match history.
        /// </summary>
        public int? PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the 1-based page number.
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Gets or sets the number of results per page.
        /// </summary>
        public int NumberOfResults { get; set; } = 10;

        /// <summary>
        /// Gets or sets a value indicating whether only competitive (ranked) matches should be returned.
        /// </summary>
        public bool RankedGamesOnly { get; set; }

        /// <summary>
        /// Gets or sets the optional list of queue IDs to filter by (e.g. "competitive", "unrated", "swiftplay", "custom").
        /// </summary>
        public List<string>? Queues { get; set; }

        /// <summary>
        /// Gets or sets the optional agent display name filter when querying a specific player's match history.
        /// </summary>
        public string? AgentName { get; set; }

        /// <summary>
        /// Gets or sets the inclusive lower bound on <see cref="ValGame.GameStart"/>.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the inclusive upper bound on <see cref="ValGame.GameStart"/>.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether matches played by smurf accounts alone are included in the crew history.
        /// </summary>
        public bool IncludeSmurfs { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether matches played by accounts outside the crew alone are included in the crew history.
        /// </summary>
        public bool IncludeOutOfCrew { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the handler should fetch the latest 10 matches from HenrikDev before querying the database.
        /// </summary>
        public bool SyncFromApi { get; set; }
    }
}
