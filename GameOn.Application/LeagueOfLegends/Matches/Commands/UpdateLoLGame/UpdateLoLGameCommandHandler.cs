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

            var timelineFromRiot = await this.matchService.GetGameTimelineById(request.MatchId, cancellationToken);

            if (timelineFromRiot is null)
            {
                throw new NotImplementedException("Timeline doesn't exist in Riot Games API!");
            }

            // Removing old timeline frames (and, by DB cascade, their events) first and committing
            // before touching participants: LoLGameTimelineEvent has a Restrict FK to LoLGameParticipant,
            // so old participants can't be deleted while old events still reference them.
            var frames = await this.context.LeagueOfLegendsGameTimelineFrames.Where(x => x.MatchId == request.MatchId).ToListAsync(cancellationToken);
            this.context.LeagueOfLegendsGameTimelineFrames.RemoveRange(frames);
            await this.context.SaveChangesAsync(cancellationToken);

            // Updating game participants. Committing the removal on its own, before adding the new
            // participants (with the same MatchId+Puuid alternate key) and the new events that
            // reference them by that same key: EF Core's change tracker otherwise gets confused
            // between the about-to-be-deleted and about-to-be-added participants sharing the same
            // key, and silently clears the events' *PUUID columns instead of persisting them.
            var participants = await this.context.LeagueOfLegendsGameParticipants.Where(x => x.MatchId == request.MatchId).ToListAsync(cancellationToken);
            this.context.LeagueOfLegendsGameParticipants.RemoveRange(participants);
            await this.context.SaveChangesAsync(cancellationToken);

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
                    ChampExperience = participant.ChampExperience,
                    ChampionTransform = participant.ChampionTransform,
                    Kills = participant.Kills,
                    Deaths = participant.Deaths,
                    Assists = participant.Assists,
                    AllInPings = participant.AllInPings,
                    AssistMePings = participant.AssistMePings,
                    CommandPings = participant.CommandPings,
                    BaronKills = participant.BaronKills,
                    BountyLevel = participant.BountyLevel,
                    ConsumablesPurchased = participant.ConsumablesPurchased,
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
                            PositionX = participant.Position?.X ?? 0,
                            PositionY = participant.Position?.Y ?? 0,
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
                            AbilityHaste = participant.ChampionStats.AbilityHaste,
                            AbilityPower = participant.ChampionStats.AbilityPower,
                            Armor = participant.ChampionStats.Armor,
                            ArmorPen = participant.ChampionStats.ArmorPen,
                            ArmorPenPercent = participant.ChampionStats.ArmorPenPercent,
                            AttackDamage = participant.ChampionStats.AttackDamage,
                            AttackSpeed = participant.ChampionStats.AttackSpeed,
                            BonusArmorPenPercent = participant.ChampionStats.BonusArmorPenPercent,
                            BonusMagicPenPercent = participant.ChampionStats.BonusMagicPenPercent,
                            CcReduction = participant.ChampionStats.CcReduction,
                            CooldownReduction = participant.ChampionStats.CooldownReduction,
                            Health = participant.ChampionStats.Health,
                            HealthMax = participant.ChampionStats.HealthMax,
                            HealthRegen = participant.ChampionStats.HealthRegen,
                            Lifesteal = participant.ChampionStats.Lifesteal,
                            MagicPen = participant.ChampionStats.MagicPen,
                            MagicPenPercent = participant.ChampionStats.MagicPenPercent,
                            MagicResist = participant.ChampionStats.MagicResist,
                            MovementSpeed = participant.ChampionStats.MovementSpeed,
                            Omnivamp = participant.ChampionStats.Omnivamp,
                            PhysicalVamp = participant.ChampionStats.PhysicalVamp,
                            Power = participant.ChampionStats.Power,
                            PowerMax = participant.ChampionStats.PowerMax,
                            PowerRegen = participant.ChampionStats.PowerRegen,
                            SpellVamp = participant.ChampionStats.SpellVamp,
                        });
    #pragma warning restore CS8602 // Déréférencement d'une éventuelle référence null.
                    }
                }

                frameInDb.LoLGameTimelineEvents = frame.Events
                    .Select(evt => MapEvent(request.MatchId, evt, timelineFromRiot.Info.Participants))
                    .ToList();

                this.context.LeagueOfLegendsGameTimelineFrames.Add(frameInDb);
            }

            // Updating game
            matchInDb.GameId = matchFromRiot.Info.GameId;
            matchInDb.EndOfGameResult = matchFromRiot.Info.EndOfGameResult;
            matchInDb.GameVersion = matchFromRiot.Info.GameVersion;
            matchInDb.FrameInterval = timelineFromRiot.Info.FrameInterval;

            matchInDb.GameStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            matchInDb.GameStart = matchInDb.GameStart.AddMilliseconds(matchFromRiot.Info.GameStartTimeStamp).ToLocalTime();

            matchInDb.GameEnd = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            matchInDb.GameEnd = matchInDb.GameEnd.AddMilliseconds((double)matchFromRiot.Info.GameEndTimestamp).ToLocalTime();

            var queueExists = await this.context.LeagueOfLegendsQueues.AnyAsync(x => x.Id == matchFromRiot.Info.QueueId, cancellationToken);
            matchInDb.QueueId = queueExists ? matchFromRiot.Info.QueueId : null;

            matchInDb.IsRemake = matchFromRiot.Info.Participants.All(x => x.GameEndedInEarlySurrender);

            if (matchInDb.IsRemake)
            {
                matchInDb.WinningTeamId = null;
            }
            else
            {
                foreach (var team in matchFromRiot.Info.Teams)
                {
                    if (team.HasWon)
                    {
                        matchInDb.WinningTeamId = team.TeamId;
                    }
                }
            }

            await this.context.SaveChangesAsync(cancellationToken);
            return matchInDb;
        }

        private static LoLGameTimelineEvent MapEvent(string matchId, EventDto evt, List<ParticipantTimeLineDto> participants)
        {
            string? ResolvePuuid(int? participantId)
                => participantId is null ? null : participants.FirstOrDefault(x => x.ParticipantId == participantId)?.PUUID;

            return new LoLGameTimelineEvent
            {
                MatchId = matchId,
                Timestamp = evt.Timestamp,
                RealTimestamp = evt.RealTimestamp,
                EventType = evt.Type,
                ParticipantId = evt.ParticipantId,
                ParticipantPUUID = ResolvePuuid(evt.ParticipantId),
                KillerId = evt.KillerId,
                KillerPUUID = ResolvePuuid(evt.KillerId),
                VictimId = evt.VictimId,
                VictimPUUID = ResolvePuuid(evt.VictimId),
                KillerTeamId = evt.KillerTeamId,
                TeamId = evt.TeamId,
                Bounty = evt.Bounty,
                ShutdownBounty = evt.ShutdownBounty,
                KillStreakLength = evt.KillStreakLength,
                MultiKillLength = evt.MultiKillLength,
                KillType = evt.KillType,
                ItemId = evt.ItemId,
                BeforeId = evt.BeforeId,
                AfterId = evt.AfterId,
                GoldGain = evt.GoldGain,
                SkillSlot = evt.SkillSlot,
                LevelUpType = evt.LevelUpType,
                Level = evt.Level,
                WardType = evt.WardType,
                CreatorId = evt.CreatorId,
                CreatorPUUID = ResolvePuuid(evt.CreatorId),
                BuildingType = evt.BuildingType,
                TowerType = evt.TowerType,
                LaneType = evt.LaneType,
                MonsterType = evt.MonsterType,
                MonsterSubType = evt.MonsterSubType,
                TransformType = evt.TransformType,
                DragonSoulType = evt.DragonSoulType,
                PositionX = evt.Position?.X,
                PositionY = evt.Position?.Y,
                LoLGameTimelineEventAssists = evt.AssistingParticipantIds?
                    .Select(participantId => new LoLGameTimelineEventAssist
                    {
                        MatchId = matchId,
                        ParticipantId = participantId,
                        ParticipantPUUID = ResolvePuuid(participantId),
                    })
                    .ToList() ?? new List<LoLGameTimelineEventAssist>(),
            };
        }
    }
}
