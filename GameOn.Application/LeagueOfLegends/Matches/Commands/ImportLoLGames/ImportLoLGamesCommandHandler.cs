// <copyright file="ImportLoLGamesCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

using GameOn.Application.LeagueOfLegends.Matches.Commands.UpdateLoLGame;

namespace GameOn.Application.LeagueOfLegends.Matches.Commands.ImportLoLGames
{
    using GameOn.Common.Interfaces;
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
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportLoLGamesCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">ISender interface, injected.</param>
        public ImportLoLGamesCommandHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
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
                    // Creating game in database
                    matchInDb = new LoLGame
                    {
                        MatchId = matchId,
                        LeagueOfLegendsGameParticipants = new List<LoLGameParticipant>
                        {
                            new LoLGameParticipant
                            {
                                PlayerId = request.Player.Id,
                            },
                        },
                    };

                    this.context.LeagueOfLegendsGames.Add(matchInDb);
                    await this.context.SaveChangesAsync(cancellationToken);
                    await this.mediator.Send(new UpdateLoLGameCommand { MatchId = matchInDb.MatchId }, CancellationToken.None);
                }
            }

            return true;
        }
    }
}
