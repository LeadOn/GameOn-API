// <copyright file="GetLastGamesPlayedQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Matches.Queries.GetLastGamesPlayed
{
    using GameOn.Common.DTOs.Common;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetLastGamesPlayedQuery class.
    /// </summary>
    public class GetLastGamesPlayedQuery : IRequest<ListResultDto<LoLGame>?>
    {
        /// <summary>
        /// Gets or sets player Id.
        /// </summary>
        public int? PlayerId { get; set; }

        /// <summary>
        /// Gets or sets page.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets number of results.
        /// </summary>
        public int NumberOfResults { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a game is a ranked or not.
        /// </summary>
        public bool RankedGamesOnly { get; set; }

        /// <summary>
        /// Gets or sets the queue IDs to filter games by (Riot queueId, see LoLQueue).
        /// </summary>
        public List<int>? QueueIds { get; set; }

        /// <summary>
        /// Gets or sets the lower bound (inclusive) of the game start date range to filter by.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the upper bound (inclusive) of the game start date range to filter by.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the Riot team position (TOP, JUNGLE, MIDDLE, BOTTOM, UTILITY) the player must
        /// have played to keep a game. Null or empty means every role. Only applies when
        /// <see cref="PlayerId"/> is set: without a player there is no single role to filter on.
        /// </summary>
        public string? TeamPosition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a game played on a smurf account alone belongs to the
        /// shared history. Defaults to true. Note this decides whether a game is <em>listed</em>, not
        /// which participants it shows: a listed game always carries its full roster. Only applies when
        /// <see cref="PlayerId"/> is null -- an account's own history is about that account, whatever it
        /// is.
        /// </summary>
        public bool IncludeSmurfs { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether a game played by tracked accounts outside the crew
        /// alone belongs to the shared history. Defaults to false, for the same reason as everywhere
        /// else: this list is the crew's history. Only applies when <see cref="PlayerId"/> is null.
        /// </summary>
        public bool IncludeOutOfCrew { get; set; }
    }
}
