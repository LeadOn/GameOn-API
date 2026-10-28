// <copyright file="GetValorantPlayerByIdQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Players.Queries.GetValorantPlayerById
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Common.DTOs;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
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
                var playerHistory = await this.context.ValorantRankHistory
                    .Where(x => x.PlayerId == playerInDb.Id)
                    .OrderByDescending(x => x.CreatedOn)
                    .ThenByDescending(x => x.Id)
                    .ToListAsync(cancellationToken);

                playerInDb.ValorantCurrentRank = playerHistory.FirstOrDefault();
                playerInDb.RrChange7DaysValorant = GetRrChange7Days(playerHistory, DateTime.UtcNow.AddDays(-7));
                playerInDb.SmurfAccounts = await this.GetSmurfAccounts(playerInDb.Id, cancellationToken);
            }

            return playerInDb;
        }

        private static int? GetRrChange7Days(List<ValorantRankHistory> historyDescending, DateTime sevenDaysAgo)
        {
            var current = historyDescending.FirstOrDefault(x => x.TierId >= 3 && x.Elo > 0);
            var baseline = historyDescending.FirstOrDefault(x => x.CreatedOn <= sevenDaysAgo && x.TierId >= 3 && x.Elo > 0);

            if (current is null || baseline is null)
            {
                return null;
            }

            return current.Elo - baseline.Elo;
        }

        private async Task<List<PlayerDto>> GetSmurfAccounts(int playerId, CancellationToken cancellationToken)
        {
            var smurfAccounts = await this.context.Players
                .Where(x => x.PrimaryPlayerId == playerId)
                .OrderBy(x => x.Id)
                .Select(x => new PlayerDto(x))
                .ToListAsync(cancellationToken);

            if (smurfAccounts.Count == 0)
            {
                return smurfAccounts;
            }

            var smurfIds = smurfAccounts.Select(x => x.Id).ToList();
            var ranks = await this.context.ValorantRankHistory
                .Where(x => smurfIds.Contains(x.PlayerId))
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.Id)
                .ToListAsync(cancellationToken);

            foreach (var smurf in smurfAccounts)
            {
                smurf.ValorantCurrentRank = ranks.FirstOrDefault(x => x.PlayerId == smurf.Id);
            }

            return smurfAccounts;
        }
    }
}