// <copyright file="RecomputeLoLGameRankChangesCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Matches.Commands.RecomputeLoLGameRankChanges
{
    using GameOn.Common.DTOs.LeagueOfLegends;
    using MediatR;

    /// <summary>
    /// RecomputeLoLGameRankChangesCommand class. Recomputes the LP won or lost in each ranked game from
    /// the stored rank snapshots, for the games ending within the given bounds. Reads nothing from Riot.
    /// </summary>
    public class RecomputeLoLGameRankChangesCommand : IRequest<RecomputeLoLGameRankChangesResultDto>
    {
        /// <summary>
        /// Gets or sets the player whose games are recomputed. Null for every player holding snapshots.
        /// </summary>
        public int? PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the earliest game end to recompute. Null for no lower bound.
        /// </summary>
        public DateTime? Since { get; set; }

        /// <summary>
        /// Gets or sets the latest game end to recompute. Null for no upper bound.
        /// </summary>
        public DateTime? Until { get; set; }
    }
}
