// <copyright file="GetValGameByIdQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Matches.Queries.GetValGameById
{
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// <see cref="GetValGameByIdQuery"/> handler.
    /// </summary>
    public class GetValGameByIdQueryHandler : IRequestHandler<GetValGameByIdQuery, ValGame?>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetValGameByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetValGameByIdQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<ValGame?> Handle(GetValGameByIdQuery request, CancellationToken cancellationToken)
        {
            return await this.context.ValorantGames
                .Include(x => x.ValorantGameParticipants)
                .ThenInclude(y => y.Player)
                .Include(x => x.ValorantGameParticipants)
                .ThenInclude(y => y.Stats)
                .Include(x => x.ValorantGameTeams)
                .FirstOrDefaultAsync(x => x.MatchId == request.MatchId, cancellationToken);
        }
    }
}
