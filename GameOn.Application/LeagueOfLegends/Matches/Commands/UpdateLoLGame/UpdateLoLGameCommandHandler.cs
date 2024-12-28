// <copyright file="UpdateLoLGameCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Matches.Commands.UpdateLoLGame
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using GameOn.External.RiotGames.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// UpdateLoLGameCommandHandler class.
    /// </summary>
    public class UpdateLoLGameCommandHandler : IRequestHandler<UpdateLoLGameCommand, bool>
    {
        private readonly IApplicationDbContext context;
        private readonly IMatchService matchService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateLoLGameCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="matchService">Riot Games Match Service, injected.</param>
        public UpdateLoLGameCommandHandler(IApplicationDbContext context, IMatchService matchService)
        {
            this.context = context;
            this.matchService = matchService;
        }

        /// <inheritdoc />
        public async Task<bool> Handle(UpdateLoLGameCommand request, CancellationToken cancellationToken)
        {
            var matchInDb = await this.context.LeagueOfLegendsGames.FirstOrDefaultAsync(x => x.MatchId == request.MatchId);

            if (matchInDb is null)
            {
                throw new NotImplementedException("Game doesn't exist in database!");
            }

            var matchFromRiot = await this.matchService.GetGameById(request.MatchId, cancellationToken);

            if (matchFromRiot is null)
            {
                throw new NotImplementedException("Game doesn't exist in Riot Games API!");
            }

            // Updating game participants
            var participants = await this.context.LeagueOfLegendsGameParticipants.Where(x => x.MatchId == request.MatchId).ToListAsync(cancellationToken);
            this.context.LeagueOfLegendsGameParticipants.RemoveRange(participants);

            foreach (var participant in matchFromRiot.Info.Participants)
            {
                var participantInDb = new LoLGameParticipant
                {
                    MatchId = request.MatchId,
                    Puuid = participant.Puuid,
                    RiotIdTagLine = participant.RiotIdTagLine,
                    RiotIdGameName = participant.RiotIdGameName,
                    ChampionId = participant.ChampionId,
                    ChampionName = participant.ChampionName,
                    TeamId = participant.TeamId,
                };

                // Checking if participant is a GameOn! User
                var playerInDb = await this.context.Players.FirstOrDefaultAsync(x => x.RiotGamesPUUID == participant.Puuid);
                if (playerInDb is not null)
                {
                    participantInDb.PlayerId = playerInDb.Id;
                }

                this.context.LeagueOfLegendsGameParticipants.Add(participantInDb);
            }

            // Updating game
            matchInDb.GameId = matchFromRiot.Info.GameId;
            matchInDb.EndOfGameResult = matchFromRiot.Info.EndOfGameResult;
            matchInDb.GameVersion = matchFromRiot.Info.GameVersion;

            foreach (var team in matchFromRiot.Info.Teams)
            {
                if (team.HasWon)
                {
                    matchInDb.WinningTeamId = team.TeamId;
                }
            }

            await this.context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
