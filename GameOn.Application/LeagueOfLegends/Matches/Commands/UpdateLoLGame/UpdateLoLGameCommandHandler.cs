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
    public class UpdateLoLGameCommandHandler : IRequestHandler<UpdateLoLGameCommand, LoLGame>
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
        public async Task<LoLGame> Handle(UpdateLoLGameCommand request, CancellationToken cancellationToken)
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
                    Win = participant.Win,
                    ChampLevel = participant.ChampLevel,
                    Kills = participant.Kills,
                    Deaths = participant.Deaths,
                    Assists = participant.Assists,
                    Item0 = participant.Item0,
                    Item1 = participant.Item1,
                    Item2 = participant.Item2,
                    Item3 = participant.Item3,
                    Item4 = participant.Item4,
                    Item5 = participant.Item5,
                    Item6 = participant.Item6,
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

            matchInDb.GameStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            matchInDb.GameStart = matchInDb.GameStart.AddMilliseconds(matchFromRiot.Info.GameStartTimeStamp).ToLocalTime();

            matchInDb.GameEnd = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            matchInDb.GameEnd = matchInDb.GameEnd.AddMilliseconds((double)matchFromRiot.Info.GameEndTimestamp).ToLocalTime();

            switch (matchFromRiot.Info.QueueId)
            {
                case 420:
                    matchInDb.QueueType = "RANKED_SOLO_DUO";
                    break;

                case 430:
                    matchInDb.QueueType = "NORMAL_5v5";
                    break;

                case 440:
                    matchInDb.QueueType = "RANKED_FLEX";
                    break;

                default:
                    matchInDb.QueueType = "Inconnu";
                    break;
            }

            foreach (var team in matchFromRiot.Info.Teams)
            {
                if (team.HasWon)
                {
                    matchInDb.WinningTeamId = team.TeamId;
                }
            }

            await this.context.SaveChangesAsync(cancellationToken);
            return matchInDb;
        }
    }
}
