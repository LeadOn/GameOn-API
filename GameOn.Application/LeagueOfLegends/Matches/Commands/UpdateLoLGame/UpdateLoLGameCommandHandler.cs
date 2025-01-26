// <copyright file="UpdateLoLGameCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Matches.Commands.UpdateLoLGame
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using GameOn.External.RiotGames.Interfaces;
    using GameOn.External.RiotGames.Models.DTOs;
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

            // Updating timeline
            var timelineFromRiot = await this.matchService.GetGameTimelineById(request.MatchId, cancellationToken);

            if (timelineFromRiot is null)
            {
                throw new NotImplementedException("Timeline doesn't exist in Riot Games API!");
            }

            var frames = await this.context.LeagueOfLegendsGameTimelineFrames.Where(x => x.MatchId == request.MatchId).ToListAsync(cancellationToken);
            this.context.LeagueOfLegendsGameTimelineFrames.RemoveRange(frames);

            foreach (var frame in timelineFromRiot.Info.Frames)
            {
                // Inserting / updating frame
                var frameInDb = new LoLGameTimelineFrame
                {
                    MatchId = request.MatchId,
                    Timestamp = frame.Timestamp,
                    LoLGameTimelineFrameParticipants = new List<LoLGameTimelineFrameParticipant>(),
                };

                foreach (var prop in frame.ParticipantFrames.GetType().GetProperties())
                {
                    var participant = prop.GetValue(frame.ParticipantFrames, null) as ParticipantFrameDto;
#pragma warning disable CS8602 // Déréférencement d'une éventuelle référence null.
                    var puuid = timelineFromRiot.Info.Participants.First(x => x.ParticipantId == participant.ParticipantId).PUUID;
                    frameInDb.LoLGameTimelineFrameParticipants.Add(new LoLGameTimelineFrameParticipant
                    {
                        CurrentGold = participant.CurrentGold,
                        GoldPerSecond = participant.GoldPerSecond,
                        JungleMinionsKilled = participant.JungleMinionsKilled,
                        Level = participant.Level,
                        MinionsKilled = participant.MinionsKilled,
                        ParticipantId = participant.ParticipantId,
                        ParticipantPUUID = puuid,
                        TimeEnemySpentControlled = participant.TimeEnemySpentControlled,
                        TotalGold = participant.TotalGold,
                        Xp = participant.Xp,
                        MagicDamageDone = participant.DamageStats.MagicDamageDone,
                        MagicDamageDoneToChampions = participant.DamageStats.MagicDamageDoneToChampions,
                        MagicDamageTaken = participant.DamageStats.MagicDamageTaken,
                        PhysicalDamageDone = participant.DamageStats.PhysicalDamageDone,
                        PhysicalDamageDoneToChampions = participant.DamageStats.PhysicalDamageDoneToChampions,
                        TotalDamageDone = participant.DamageStats.TotalDamageDone,
                        TotalDamageDoneToChampions = participant.DamageStats.TotalDamageDoneToChampions,
                        TotalDamageTaken = participant.DamageStats.TotalDamageTaken,
                        TrueDamageDone = participant.DamageStats.TrueDamageDone,
                        TrueDamageTaken = participant.DamageStats.TrueDamageTaken,
                        TrueDamageDoneToChampions = participant.DamageStats.TrueDamageDoneToChampions,
                        PhysicalDamageTaken = participant.DamageStats.PhysicalDamageTaken,
                    });
#pragma warning restore CS8602 // Déréférencement d'une éventuelle référence null.
                }

                this.context.LeagueOfLegendsGameTimelineFrames.Add(frameInDb);
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
