// <copyright file="GetAllValorantPlayersQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Players.Queries.GetAllValorantPlayers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Application.Common.Players;
    using GameOn.Common.DTOs;
    using GameOn.Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetAllValorantPlayersQueryHandler class.
    /// </summary>
    public class GetAllValorantPlayersQueryHandler : IRequestHandler<GetAllValorantPlayersQuery, IEnumerable<PlayerDto>>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllValorantPlayersQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetAllValorantPlayersQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<PlayerDto>> Handle(GetAllValorantPlayersQuery request, CancellationToken cancellationToken)
        {
            var playersQuery = this.context.Players.Include(x => x.TournamentsWon).AsQueryable();

            if (!request.IncludeSmurfs)
            {
                playersQuery = playersQuery.PrimariesOnly();
            }

            if (!request.IncludeOutOfCrew)
            {
                playersQuery = playersQuery.InCrewOnly();
            }

            return await playersQuery
                .Where(x => x.Archived == request.Archived
                    && (x.ValorantPuuid != null || (x.RiotGamesNickname != null && x.RiotGamesTagLine != null)))
                .Select(x => new PlayerDto(x))
                .ToListAsync(cancellationToken);
        }
    }
}