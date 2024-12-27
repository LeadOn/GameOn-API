// <copyright file="ImportLoLGamesCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Matches.Commands.ImportLoLGames
{
    using GameOn.Application.Common.Interfaces;
    using GameOn.Domain;
    using GameOn.External.RiotGames.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// ImportLoLGamesCommandHandler class.
    /// </summary>
    public class ImportLoLGamesCommandHandler : IRequestHandler<ImportLoLGamesCommand, bool>
    {
        private readonly IApplicationDbContext context;
        private readonly IMatchService matchService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportLoLGamesCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="matchService">Match service, injected.</param>
        public ImportLoLGamesCommandHandler(IApplicationDbContext context, IMatchService matchService)
        {
            this.context = context;
            this.matchService = matchService;
        }

        /// <inheritdoc />
        public async Task<bool> Handle(ImportLoLGamesCommand request, CancellationToken cancellationToken)
        {
            foreach (var matchId in request.MatchIDs)
            {
                // Only executing if match isn't already in database
                var matchInDb = await this.context.LeagueOfLegendsGames.FirstOrDefaultAsync(x => x.MatchId == matchId);

                if (matchInDb is null)
                {
                    // Getting game from Riot Games API
                    var gameFromRiot = await this.matchService.GetGameById(matchId, cancellationToken);

                    // Creating game in database
                    matchInDb = new LoLGame
                    {
                        GameId = gameFromRiot.Info.GameId,
                        MatchId = matchId,
                        EndOfGameResult = gameFromRiot.Info.EndOfGameResult,
                        GameVersion = gameFromRiot.Info.GameVersion,
                        LeagueOfLegendsGameParticipants = new List<LoLGameParticipant>(),
                    };

                    // Creating game participants
                    foreach (var participant in gameFromRiot.Metadata.Participants)
                    {
                        // Finding player in database
                        var playerInDb = await this.context.Players.FirstOrDefaultAsync(x => x.RiotGamesPUUID == participant);

                        if (playerInDb is not null)
                        {
                            var participantInDb = new LoLGameParticipant
                            {
                                PlayerId = playerInDb.Id,
                            };

                            matchInDb.LeagueOfLegendsGameParticipants.Add(participantInDb);
                        }
                    }

                    this.context.LeagueOfLegendsGames.Add(matchInDb);
                }
                else
                {
                    // TODO
                }
            }

            await this.context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
