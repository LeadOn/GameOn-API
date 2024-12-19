// <copyright file="GetAllLeaguePlayersQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Queries.GetAllLeaguePlayers
{
    using GameOn.Application.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetAllLeaguePlayersQueryHandler class.
    /// </summary>
    public class GetAllLeaguePlayersQueryHandler : IRequestHandler<GetAllLeaguePlayersQuery, IEnumerable<Player>>
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
        public async Task<IEnumerable<Player>> Handle(GetAllLeaguePlayersQuery request, CancellationToken cancellationToken)
        {
            return await this.context.Players.Include(x => x.TournamentsWon).Where(x => x.Archived == request.Archived && x.LolSummonerId != null).ToListAsync(cancellationToken);
        }
    }
}
