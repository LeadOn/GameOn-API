// <copyright file="GetAllPlayersQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Players.Queries.GetAllPlayers
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetAllPlayersQueryHandler class.
    /// </summary>
    public class GetAllPlayersQueryHandler : IRequestHandler<GetAllPlayersQuery, IEnumerable<Player>>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllPlayersQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetAllPlayersQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Player>> Handle(GetAllPlayersQuery request, CancellationToken cancellationToken)
        {
            // Every account by default, smurfs included and tagged with PrimaryPlayerId: a list that
            // silently drops rows is how a linked account looks like lost data everywhere it was
            // displayed. Callers that need people rather than accounts ask for it explicitly.
            var playersQuery = this.context.Players.Include(x => x.TournamentsWon).AsQueryable();

            if (!request.IncludeSmurfs)
            {
                playersQuery = playersQuery.PrimariesOnly();
            }

            return await playersQuery.Where(x => x.Archived == request.Archived).ToListAsync(cancellationToken);
        }
    }
}
