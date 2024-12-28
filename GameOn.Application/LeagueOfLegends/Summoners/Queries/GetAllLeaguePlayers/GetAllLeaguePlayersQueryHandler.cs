// <copyright file="GetAllLeaguePlayersQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Queries.GetAllLeaguePlayers
{
    using GameOn.Common.Interfaces;
    using GameOn.Common.DTOs;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetAllLeaguePlayersQueryHandler class.
    /// </summary>
    public class GetAllLeaguePlayersQueryHandler : IRequestHandler<GetAllLeaguePlayersQuery, IEnumerable<PlayerDto>>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllLeaguePlayersQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetAllLeaguePlayersQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<PlayerDto>> Handle(GetAllLeaguePlayersQuery request, CancellationToken cancellationToken)
        {
            var playersInDb = await this.context.Players.Include(x => x.TournamentsWon).Where(x => x.Archived == request.Archived && x.LolSummonerId != null).Select(x => new PlayerDto(x)).ToListAsync(cancellationToken);

            foreach (var player in playersInDb)
            {
                var soloRank = await this.context.LeagueOfLegendsRankHistory.OrderByDescending(x => x.CreatedOn).FirstOrDefaultAsync(x => x.PlayerId == player.Id && x.QueueType == "RANKED_SOLO_5x5", cancellationToken);

                player.LeagueOfLegendsSoloRank = soloRank;

                var flexRank = await this.context.LeagueOfLegendsRankHistory.OrderByDescending(x => x.CreatedOn).FirstOrDefaultAsync(x => x.PlayerId == player.Id && x.QueueType == "RANKED_FLEX_SR", cancellationToken);

                player.LeagueOfLegendsFlexRank = flexRank;
            }

            return playersInDb;
        }
    }
}
