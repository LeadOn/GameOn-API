// <copyright file="GetValorantPlayerByIdQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Players.Queries.GetValorantPlayerById
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Common.DTOs;
    using GameOn.Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetValorantPlayerByIdQueryHandler class.
    /// </summary>
    public class GetValorantPlayerByIdQueryHandler : IRequestHandler<GetValorantPlayerByIdQuery, PlayerDto?>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetValorantPlayerByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetValorantPlayerByIdQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<PlayerDto?> Handle(GetValorantPlayerByIdQuery request, CancellationToken cancellationToken)
        {
            var playerInDb = await this.context.Players
                .Where(x => x.Id == request.PlayerId
                    && (x.ValorantPuuid != null || x.RiotGamesPUUID != null || (x.RiotGamesNickname != null && x.RiotGamesTagLine != null)))
                .Select(x => new PlayerDto(x))
                .FirstOrDefaultAsync(cancellationToken);

            if (playerInDb is not null)
            {
                playerInDb.SmurfAccounts = await this.context.Players
                    .Where(x => x.PrimaryPlayerId == playerInDb.Id)
                    .OrderBy(x => x.Id)
                    .Select(x => new PlayerDto(x))
                    .ToListAsync(cancellationToken);
            }

            return playerInDb;
        }
    }
}