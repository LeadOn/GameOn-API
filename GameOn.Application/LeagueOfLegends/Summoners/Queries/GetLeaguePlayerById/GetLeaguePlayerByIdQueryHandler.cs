// <copyright file="GetLeaguePlayerByIdQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Queries.GetLeaguePlayerById
{
    using GameOn.Common.Interfaces;
    using GameOn.Common.DTOs;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetLeaguePlayerByIdQueryHandler class.
    /// </summary>
    public class GetLeaguePlayerByIdQueryHandler : IRequestHandler<GetLeaguePlayerByIdQuery, PlayerDto?>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLeaguePlayerByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetLeaguePlayerByIdQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<PlayerDto?> Handle(GetLeaguePlayerByIdQuery request, CancellationToken cancellationToken)
        {
            var playerInDb = await this.context.Players.Where(x => x.Id == request.PlayerId && x.LolSummonerId != null).Select(x => new PlayerDto(x)).FirstOrDefaultAsync(cancellationToken);

            if (playerInDb != null)
            {
                var soloRank = await this.context.LeagueOfLegendsRankHistory.OrderByDescending(x => x.CreatedOn).FirstOrDefaultAsync(x => x.PlayerId == playerInDb.Id && x.QueueType == "RANKED_SOLO_5x5", cancellationToken);

                playerInDb.LeagueOfLegendsSoloRank = soloRank;

                var flexRank = await this.context.LeagueOfLegendsRankHistory.OrderByDescending(x => x.CreatedOn).FirstOrDefaultAsync(x => x.PlayerId == playerInDb.Id && x.QueueType == "RANKED_FLEX_SR", cancellationToken);

                playerInDb.LeagueOfLegendsFlexRank = flexRank;
            }

            return playerInDb;
        }
    }
}
