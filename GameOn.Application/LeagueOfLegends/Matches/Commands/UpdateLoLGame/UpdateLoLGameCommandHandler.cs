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
        private static readonly Dictionary<int, string> QueueTypeById = new()
        {
            [0] = "Custom games",
            [2] = "5v5 Blind Pick games",
            [4] = "5v5 Ranked Solo games",
            [6] = "5v5 Ranked Premade games",
            [7] = "Co-op vs AI games",
            [8] = "3v3 Normal games",
            [9] = "3v3 Ranked Flex games",
            [14] = "5v5 Draft Pick games",
            [16] = "5v5 Dominion Blind Pick games",
            [17] = "5v5 Dominion Draft Pick games",
            [25] = "Dominion Co-op vs AI games",
            [31] = "Co-op vs AI Intro Bot games",
            [32] = "Co-op vs AI Beginner Bot games",
            [33] = "Co-op vs AI Intermediate Bot games",
            [41] = "3v3 Ranked Team games",
            [42] = "5v5 Ranked Team games",
            [52] = "Co-op vs AI games",
            [61] = "5v5 Team Builder games",
            [65] = "5v5 ARAM games",
            [67] = "ARAM Co-op vs AI games",
            [70] = "One for All games",
            [72] = "1v1 Snowdown Showdown games",
            [73] = "2v2 Snowdown Showdown games",
            [75] = "6v6 Hexakill games",
            [76] = "Ultra Rapid Fire games",
            [78] = "One For All: Mirror Mode games",
            [83] = "Co-op vs AI Ultra Rapid Fire games",
            [91] = "Doom Bots Rank 1 games",
            [92] = "Doom Bots Rank 2 games",
            [93] = "Doom Bots Rank 5 games",
            [96] = "Ascension games",
            [98] = "6v6 Hexakill games",
            [100] = "5v5 ARAM games",
            [300] = "Legend of the Poro King games",
            [310] = "Nemesis games",
            [313] = "Black Market Brawlers games",
            [315] = "Nexus Siege games",
            [317] = "Definitely Not Dominion games",
            [318] = "ARURF games",
            [325] = "All Random games",
            [400] = "5v5 Draft Pick games",
            [410] = "5v5 Ranked Dynamic games",
            [420] = "RANKED_SOLO_DUO",
            [430] = "NORMAL_5V5",
            [440] = "RANKED_FLEX",
            [450] = "5v5 ARAM games",
            [460] = "3v3 Blind Pick games",
            [470] = "3v3 Ranked Flex games",
            [480] = "Swiftplay Games",
            [490] = "Normal (Quickplay)",
            [600] = "Blood Hunt Assassin games",
            [610] = "Dark Star: Singularity games",
            [700] = "Summoner's Rift Clash games",
            [720] = "ARAM Clash games",
            [800] = "Co-op vs. AI Intermediate Bot games",
            [810] = "Co-op vs. AI Intro Bot games",
            [820] = "Co-op vs. AI Beginner Bot games",
            [830] = "Co-op vs. AI Intro Bot games",
            [840] = "Co-op vs. AI Beginner Bot games",
            [850] = "Co-op vs. AI Intermediate Bot games",
            [870] = "Co-op vs. AI Intro Bot games",
            [880] = "Co-op vs. AI Beginner Bot games",
            [890] = "Co-op vs. AI Intermediate Bot games",
            [900] = "ARURF games",
            [910] = "Ascension games",
            [920] = "Legend of the Poro King games",
            [940] = "Nexus Siege games",
            [950] = "Doom Bots Voting games",
            [960] = "Doom Bots Standard games",
            [980] = "Star Guardian Invasion: Normal games",
            [990] = "Star Guardian Invasion: Onslaught games",
            [1000] = "PROJECT: Hunters games",
            [1010] = "Snow ARURF games",
            [1020] = "One for All games",
            [1030] = "Odyssey Extraction: Intro games",
            [1040] = "Odyssey Extraction: Cadet games",
            [1050] = "Odyssey Extraction: Crewmember games",
            [1060] = "Odyssey Extraction: Captain games",
            [1070] = "Odyssey Extraction: Onslaught games",
            [1090] = "Teamfight Tactics games",
            [1100] = "Ranked Teamfight Tactics games",
            [1110] = "Teamfight Tactics Tutorial games",
            [1111] = "Teamfight Tactics test games",
            [1200] = "Nexus Blitz games",
            [1210] = "Teamfight Tactics Choncc's Treasure Mode",
            [1300] = "Nexus Blitz games",
            [1400] = "Ultimate Spellbook games",
            [1700] = "Arena",
            [1710] = "Arena",
            [1810] = "Swarm Mode Games",
            [1820] = "Swarm",
            [1830] = "Swarm",
            [1840] = "Swarm",
            [1900] = "Pick URF games",
            [2000] = "Tutorial 1",
            [2010] = "Tutorial 2",
            [2020] = "Tutorial 3",
            [2300] = "Brawl",
            [2400] = "ARAM: Mayhem",
        };

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

        private static string MapQueueType(int queueId)
        {
            return QueueTypeById.TryGetValue(queueId, out var queueType) ? queueType : "Inconnu";
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

                if (frame.ParticipantFrames is not null)
                {
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

            matchInDb.QueueType = MapQueueType(matchFromRiot.Info.QueueId);

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
