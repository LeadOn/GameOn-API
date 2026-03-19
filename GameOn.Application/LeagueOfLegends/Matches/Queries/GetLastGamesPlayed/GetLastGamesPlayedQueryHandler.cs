// <copyright file="GetLastGamesPlayedQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

using GameOn.Common.DTOs.Common;

namespace GameOn.Application.LeagueOfLegends.Matches.Queries.GetLastGamesPlayed
{
    using GameOn.Application.LeagueOfLegends.Matches.Commands.ImportLoLGames;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using GameOn.External.RiotGames.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetLastGamesPlayedQueryHandler class.
    /// </summary>
    public class GetLastGamesPlayedQueryHandler : IRequestHandler<GetLastGamesPlayedQuery, ListResultDto<LoLGame>?>
    {
        private readonly IApplicationDbContext context;
        private readonly IMatchService matchService;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLastGamesPlayedQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="matchService">IMatchService, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public GetLastGamesPlayedQueryHandler(IApplicationDbContext context, IMatchService matchService, ISender mediator)
        {
            this.context = context;
            this.matchService = matchService;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<ListResultDto<LoLGame>?> Handle(GetLastGamesPlayedQuery request, CancellationToken cancellationToken)
        {
            var query = this.context.LeagueOfLegendsGames.AsQueryable();

            if (request.PlayerId is null)
            {
                query = query.Include(x => x.LeagueOfLegendsGameParticipants);

                var count = await query.CountAsync(cancellationToken);
                var resultsFromDb = await this.context.LeagueOfLegendsGames
                    .Include(x => x.LeagueOfLegendsGameParticipants)
                    .OrderByDescending(x => x.MatchId)
                    .Skip((request.Page - 1) * request.NumberOfResults)
                    .Take(request.NumberOfResults)
                    .ToListAsync(cancellationToken);

                return new ListResultDto<LoLGame>
                {
                    Page = request.Page,
                    Results = resultsFromDb,
                    ResultsPerPage = request.NumberOfResults,
                    Total = count,
                };
            }

            // Getting player in database
            var playerInDb = await this.context.Players.FirstOrDefaultAsync(x => x.Id == request.PlayerId && x.RiotGamesPUUID != null);

            if (playerInDb == null)
            {
                return null;
            }
            else
            {
                // Getting IDs from Riot Games API
                var matchesFromRiot = await this.matchService.GetLastGamesPlayed(playerInDb.RiotGamesPUUID ?? throw new NotImplementedException(), cancellationToken);

                // Updating those games in database
                await this.mediator.Send(new ImportLoLGamesCommand { MatchIDs = matchesFromRiot.ToList(), Player = playerInDb });

                query = query.Include(x => x.LeagueOfLegendsGameParticipants)
                    .Where(x => x.LeagueOfLegendsGameParticipants.Any(y => y.PlayerId == request.PlayerId));

                var count = await query.CountAsync(cancellationToken);
                var resultsFromDb = await query.OrderByDescending(x => x.MatchId)
                    .Skip((request.Page - 1) * request.NumberOfResults)
                    .Take(request.NumberOfResults)
                    .ToListAsync(cancellationToken);

                return new ListResultDto<LoLGame>
                {
                    Page = request.Page,
                    Results = resultsFromDb,
                    ResultsPerPage = request.NumberOfResults,
                    Total = count,
                };
            }
        }
    }
}
