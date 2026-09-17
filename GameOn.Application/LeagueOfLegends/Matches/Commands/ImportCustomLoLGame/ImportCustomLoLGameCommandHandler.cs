// <copyright file="ImportCustomLoLGameCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Matches.Commands.ImportCustomLoLGame
{
    using GameOn.Application.LeagueOfLegends.Matches.Services;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Common.DTOs.LeagueOfLegends.LeagueClient;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using GameOn.External.CommunityDragon.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// ImportCustomLoLGameCommandHandler class.
    /// </summary>
    /// <remarks>
    /// Deliberately not routed through <c>UpdateLoLGameCommandHandler</c>: that one starts by calling
    /// Riot, which has no data at all on custom games. What the League client sends instead is the
    /// legacy match-v4 shape, which is poorer than match-v5 in three ways worth knowing about:
    /// <list type="bullet">
    /// <item><description>no challenges object, so <see cref="LoLGameParticipant.Challenges"/> stays null and the KDA / kill participation fall back to the manual formulas;</description></item>
    /// <item><description>no damage or champion stats on timeline frames, so the persisted frames only carry gold, experience, level and creep score;</description></item>
    /// <item><description>only CHAMPION_KILL, BUILDING_KILL and ELITE_MONSTER_KILL events: no ward, item, level up or skill events.</description></item>
    /// </list>
    /// Derived stats are therefore computed from the participants' own end of game totals (which the
    /// client does send, and which are authoritative) rather than from the last timeline frame.
    /// </remarks>
    public class ImportCustomLoLGameCommandHandler : IRequestHandler<ImportCustomLoLGameCommand, ImportCustomLoLGameResultDto>
    {
        private const int SmiteSpellId = 11;

        private readonly IApplicationDbContext context;
        private readonly ICommunityDragonChampionService championService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportCustomLoLGameCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="championService">Community Dragon champion service, injected.</param>
        public ImportCustomLoLGameCommandHandler(IApplicationDbContext context, ICommunityDragonChampionService championService)
        {
            this.context = context;
            this.championService = championService;
        }

        /// <inheritdoc />
        public async Task<ImportCustomLoLGameResultDto> Handle(ImportCustomLoLGameCommand request, CancellationToken cancellationToken)
        {
            var game = request.Game ?? throw new NotImplementedException("No game payload provided!");

            if (game.GameId <= 0 || string.IsNullOrWhiteSpace(game.PlatformId))
            {
                throw new NotImplementedException("Game payload has no game ID or no platform ID!");
            }

            if (game.Participants.Count == 0)
            {
                throw new NotImplementedException("Game payload has no participant!");
            }

            var matchId = $"{game.PlatformId.ToUpperInvariant()}_{game.GameId}";
            var championNames = await this.championService.GetChampionNamesById(cancellationToken);

            var identities = game.ParticipantIdentities
                .Where(x => x.Player is not null)
                .GroupBy(x => x.ParticipantId)
                .ToDictionary(x => x.Key, x => x.First().Player!);

            var positions = DeterminePositions(game.Participants);
            var puuids = await this.ResolvePuuids(game.Participants, identities, cancellationToken);

            var matchInDb = await this.context.LeagueOfLegendsGames.FirstOrDefaultAsync(x => x.MatchId == matchId, cancellationToken);
            var replaced = matchInDb is not null;

            if (matchInDb is null)
            {
                matchInDb = new LoLGame { MatchId = matchId };
                this.context.LeagueOfLegendsGames.Add(matchInDb);
            }

            // Same removal dance as UpdateLoLGameCommandHandler, and for the same reasons: MVP/ACE are
            // Restrict FKs to LoLGameParticipant, timeline events hold Restrict FKs to it too, and the
            // change tracker gets confused when old and new participants share the (MatchId, Puuid)
            // alternate key. Hence: null the FKs, commit, drop the frames, commit, drop the
            // participants and teams, commit, and only then insert the new ones.
            matchInDb.MvpParticipantId = null;
            matchInDb.AceParticipantId = null;
            await this.context.SaveChangesAsync(cancellationToken);

            if (replaced)
            {
                var oldFrames = await this.context.LeagueOfLegendsGameTimelineFrames.Where(x => x.MatchId == matchId).ToListAsync(cancellationToken);
                this.context.LeagueOfLegendsGameTimelineFrames.RemoveRange(oldFrames);
                await this.context.SaveChangesAsync(cancellationToken);

                var oldParticipants = await this.context.LeagueOfLegendsGameParticipants.Where(x => x.MatchId == matchId).ToListAsync(cancellationToken);
                this.context.LeagueOfLegendsGameParticipants.RemoveRange(oldParticipants);

                var oldTeams = await this.context.LeagueOfLegendsGameTeams.Where(x => x.MatchId == matchId).ToListAsync(cancellationToken);
                this.context.LeagueOfLegendsGameTeams.RemoveRange(oldTeams);

                await this.context.SaveChangesAsync(cancellationToken);
            }

            var participantsInDb = new List<LoLGameParticipant>();
            var linkedPlayers = 0;
            var unlinkedRiotIds = new List<string>();

            foreach (var participant in game.Participants)
            {
                identities.TryGetValue(participant.ParticipantId, out var identity);
                var stats = participant.Stats ?? new LcuParticipantStatsDto();
                var resolved = puuids[participant.ParticipantId];

                var participantInDb = new LoLGameParticipant
                {
                    MatchId = matchId,
                    Puuid = resolved.Puuid,
                    RiotIdGameName = identity?.GameName ?? string.Empty,
                    RiotIdTagLine = identity?.TagLine ?? string.Empty,
                    ChampionId = participant.ChampionId,
                    ChampionName = championNames.GetValueOrDefault(participant.ChampionId, string.Empty),
                    TeamId = participant.TeamId,
                    Win = stats.Win,
                    ChampLevel = stats.ChampLevel,
                    Kills = stats.Kills,
                    Deaths = stats.Deaths,
                    Assists = stats.Assists,
                    Item0 = stats.Item0,
                    Item1 = stats.Item1,
                    Item2 = stats.Item2,
                    Item3 = stats.Item3,
                    Item4 = stats.Item4,
                    Item5 = stats.Item5,
                    Item6 = stats.Item6,
                    VisionScore = stats.VisionScore,
                    GameEndedInEarlySurrender = stats.GameEndedInEarlySurrender,
                    TeamPosition = positions.GetValueOrDefault(participant.ParticipantId, string.Empty),
                    IndividualPosition = positions.GetValueOrDefault(participant.ParticipantId, string.Empty),
                    PlayerId = resolved.PlayerId,
                };

                if (resolved.PlayerId is not null)
                {
                    linkedPlayers++;
                }
                else
                {
                    unlinkedRiotIds.Add($"{identity?.GameName}#{identity?.TagLine}");
                }

                this.context.LeagueOfLegendsGameParticipants.Add(participantInDb);
                participantsInDb.Add(participantInDb);
            }

            var puuidsByParticipantId = puuids.ToDictionary(x => x.Key, x => x.Value.Puuid);
            var statsByParticipantId = game.Participants
                .GroupBy(x => x.ParticipantId)
                .ToDictionary(x => x.Key, x => x.First().Stats ?? new LcuParticipantStatsDto());

            var framesInDb = MapFrames(matchId, request.Timeline, puuidsByParticipantId, statsByParticipantId);

            // The client doesn't send champion experience with the end of game stats, but the last
            // timeline frame has it.
            var lastFrameByPuuid = framesInDb
                .OrderByDescending(x => x.Timestamp)
                .FirstOrDefault()?
                .LoLGameTimelineFrameParticipants
                .ToDictionary(x => x.ParticipantPUUID, x => x);

            foreach (var participant in participantsInDb)
            {
                if (lastFrameByPuuid is not null && lastFrameByPuuid.TryGetValue(participant.Puuid, out var lastFrame))
                {
                    participant.ChampExperience = lastFrame.Xp;
                }
            }

            foreach (var frame in framesInDb)
            {
                this.context.LeagueOfLegendsGameTimelineFrames.Add(frame);
            }

            // Flushing now assigns the participants' identity IDs, which DetermineMvpAndAce needs below.
            await this.context.SaveChangesAsync(cancellationToken);

            ComputeParticipantStats(participantsInDb, game);

            matchInDb.GameId = game.GameId;
            matchInDb.EndOfGameResult = game.EndOfGameResult;
            matchInDb.GameVersion = game.GameVersion;
            matchInDb.GameStart = DateTime.UnixEpoch.AddMilliseconds(game.GameCreation);
            matchInDb.GameEnd = DateTime.UnixEpoch.AddMilliseconds(game.GameCreation + (game.GameDuration * 1000));
            matchInDb.FrameInterval = DetermineFrameInterval(framesInDb);
            matchInDb.IsRemake = game.Participants.All(x => x.Stats?.GameEndedInEarlySurrender ?? false);

            var queueExists = await this.context.LeagueOfLegendsQueues.AnyAsync(x => x.Id == game.QueueId, cancellationToken);
            matchInDb.QueueId = queueExists ? game.QueueId : null;

            matchInDb.WinningTeamId = matchInDb.IsRemake
                ? null
                : game.Teams.FirstOrDefault(x => string.Equals(x.Win, "Win", StringComparison.OrdinalIgnoreCase))?.TeamId;

            foreach (var team in game.Teams)
            {
                this.context.LeagueOfLegendsGameTeams.Add(MapTeam(matchId, team, game.Participants));
            }

            var (mvpParticipantId, aceParticipantId) = LoLGameParticipantStatCalculator.DetermineMvpAndAce(
                participantsInDb,
                matchInDb.WinningTeamId,
                matchInDb.IsRemake);
            matchInDb.MvpParticipantId = mvpParticipantId;
            matchInDb.AceParticipantId = aceParticipantId;

            await this.context.SaveChangesAsync(cancellationToken);

            return new ImportCustomLoLGameResultDto
            {
                MatchId = matchId,
                Replaced = replaced,
                GameStart = matchInDb.GameStart,
                QueueId = matchInDb.QueueId,
                ParticipantCount = participantsInDb.Count,
                LinkedPlayerCount = linkedPlayers,
                TimelineFrameCount = framesInDb.Count,
                UnlinkedRiotIds = unlinkedRiotIds,
            };
        }

        /// <summary>
        /// Computes each participant's derived stats from their end of game totals. The Riot import
        /// path reads those from the last timeline frame instead, which the client's timeline doesn't
        /// carry: a throwaway frame is built here to feed the shared calculator the very same values.
        /// </summary>
        /// <param name="participants">The participants, already persisted (their IDs are needed).</param>
        /// <param name="game">The client payload they were built from.</param>
        private static void ComputeParticipantStats(List<LoLGameParticipant> participants, LcuGameDto game)
        {
            var statsByParticipantId = game.Participants
                .GroupBy(x => x.ParticipantId)
                .ToDictionary(x => x.Key, x => x.First().Stats ?? new LcuParticipantStatsDto());

            // Participants are built in payload order, so the two lists line up.
            var payloadByEntity = participants
                .Zip(game.Participants, (entity, payload) => (Entity: entity, Payload: payload))
                .ToDictionary(x => x.Entity, x => statsByParticipantId[x.Payload.ParticipantId]);

            var durationMs = (int)Math.Min(int.MaxValue, game.GameDuration * 1000);

            foreach (var team in participants.GroupBy(x => x.TeamId))
            {
                var teamKills = team.Sum(x => x.Kills);
                var teamDamage = team.Sum(x => payloadByEntity[x].TotalDamageDealtToChampions);

                foreach (var participant in team)
                {
                    var stats = payloadByEntity[participant];

                    var lastFrame = new LoLGameTimelineFrameParticipant
                    {
                        ParticipantPUUID = participant.Puuid,
                        MinionsKilled = stats.TotalMinionsKilled,
                        JungleMinionsKilled = stats.NeutralMinionsKilled,
                        TotalGold = stats.GoldEarned,
                        TotalDamageDoneToChampions = stats.TotalDamageDealtToChampions,
                        PhysicalDamageDoneToChampions = stats.PhysicalDamageDealtToChampions,
                        MagicDamageDoneToChampions = stats.MagicDamageDealtToChampions,
                        TrueDamageDoneToChampions = stats.TrueDamageDealtToChampions,
                        TotalDamageTaken = stats.TotalDamageTaken,
                        TimeEnemySpentControlled = stats.TimeCCingOthers * 1000,
                    };

                    participant.Stats = LoLGameParticipantStatCalculator.Compute(
                        participant,
                        teamKills,
                        teamDamage,
                        lastFrame,
                        durationMs,
                        stats.WardsPlaced,
                        stats.WardsKilled,
                        participant.Stats);
                }
            }
        }

        /// <summary>
        /// Guesses each participant's position. The client's own lane and role are too unreliable to
        /// copy as is (it routinely reports two junglers per team in custom games), so the jungler is
        /// taken to be whoever brought Smite and the support whoever finished with the fewest minions,
        /// the remaining three being placed by lane. Teams that aren't five players are left blank.
        /// </summary>
        /// <param name="participants">All participants of the game.</param>
        /// <returns>Position, by participant ID.</returns>
        private static Dictionary<int, string> DeterminePositions(List<LcuParticipantDto> participants)
        {
            var positions = new Dictionary<int, string>();

            foreach (var team in participants.GroupBy(x => x.TeamId))
            {
                var remaining = team.OrderBy(x => x.ParticipantId).ToList();

                if (remaining.Count != 5)
                {
                    continue;
                }

                var jungler = remaining.FirstOrDefault(x => x.Spell1Id == SmiteSpellId || x.Spell2Id == SmiteSpellId)
                    ?? remaining.OrderByDescending(x => x.Stats?.NeutralMinionsKilled ?? 0).First();
                positions[jungler.ParticipantId] = "JUNGLE";
                remaining.Remove(jungler);

                var support = remaining.FirstOrDefault(x => string.Equals(x.Timeline?.Role, "SUPPORT", StringComparison.OrdinalIgnoreCase))
                    ?? remaining.OrderBy(TotalCreepScore).First();
                positions[support.ParticipantId] = "UTILITY";
                remaining.Remove(support);

                var lanes = new List<string> { "TOP", "MIDDLE", "BOTTOM" };

                foreach (var lane in lanes.ToList())
                {
                    var onLane = remaining.FirstOrDefault(x => string.Equals(x.Timeline?.Lane, lane, StringComparison.OrdinalIgnoreCase));

                    if (onLane is not null)
                    {
                        positions[onLane.ParticipantId] = lane;
                        remaining.Remove(onLane);
                        lanes.Remove(lane);
                    }
                }

                // Whoever the client couldn't place gets what's left, in a stable order.
                foreach (var (participant, lane) in remaining.Zip(lanes, (participant, lane) => (participant, lane)))
                {
                    positions[participant.ParticipantId] = lane;
                }
            }

            return positions;
        }

        private static int TotalCreepScore(LcuParticipantDto participant)
            => (participant.Stats?.TotalMinionsKilled ?? 0) + (participant.Stats?.NeutralMinionsKilled ?? 0);

        /// <summary>
        /// Maps the client's timeline. Its frames carry gold, experience, level and creep score but
        /// no damage at all, so the last frame -- the one everything downstream reads as the end of
        /// game snapshot -- gets its damage and crowd control filled from the participants' end of
        /// game totals. Earlier frames stay at zero: the client simply doesn't know the damage curve.
        /// </summary>
        /// <param name="matchId">Match ID.</param>
        /// <param name="timeline">The client timeline, if the caller sent one.</param>
        /// <param name="puuids">PUUID, by participant ID.</param>
        /// <param name="stats">End of game stats, by participant ID.</param>
        /// <returns>The frames to persist.</returns>
        private static List<LoLGameTimelineFrame> MapFrames(
            string matchId,
            LcuTimelineDto? timeline,
            Dictionary<int, string> puuids,
            Dictionary<int, LcuParticipantStatsDto> stats)
        {
            var frames = new List<LoLGameTimelineFrame>();

            if (timeline is null)
            {
                return frames;
            }

            var lastTimestamp = timeline.Frames.Count == 0 ? 0 : timeline.Frames.Max(x => x.Timestamp);

            foreach (var frame in timeline.Frames)
            {
                var frameInDb = new LoLGameTimelineFrame
                {
                    MatchId = matchId,
                    Timestamp = frame.Timestamp,
                    LoLGameTimelineFrameParticipants = new List<LoLGameTimelineFrameParticipant>(),
                    LoLGameTimelineEvents = new List<LoLGameTimelineEvent>(),
                };

                foreach (var participantFrame in frame.ParticipantFrames?.Values ?? Enumerable.Empty<LcuFrameParticipantDto>())
                {
                    if (!puuids.TryGetValue(participantFrame.ParticipantId, out var puuid))
                    {
                        continue;
                    }

                    var frameParticipant = new LoLGameTimelineFrameParticipant
                    {
                        ParticipantId = participantFrame.ParticipantId,
                        ParticipantPUUID = puuid,
                        CurrentGold = participantFrame.CurrentGold,
                        TotalGold = participantFrame.TotalGold,
                        Level = participantFrame.Level,
                        MinionsKilled = participantFrame.MinionsKilled,
                        JungleMinionsKilled = participantFrame.JungleMinionsKilled,
                        Xp = participantFrame.Xp,
                        PositionX = participantFrame.Position?.X ?? 0,
                        PositionY = participantFrame.Position?.Y ?? 0,
                    };

                    // Champion stats (AD, armor, movement speed...) have no end of game equivalent and
                    // stay at 0 on every frame, last one included.
                    if (frame.Timestamp == lastTimestamp && stats.TryGetValue(participantFrame.ParticipantId, out var endOfGame))
                    {
                        frameParticipant.TotalDamageDoneToChampions = endOfGame.TotalDamageDealtToChampions;
                        frameParticipant.PhysicalDamageDoneToChampions = endOfGame.PhysicalDamageDealtToChampions;
                        frameParticipant.MagicDamageDoneToChampions = endOfGame.MagicDamageDealtToChampions;
                        frameParticipant.TrueDamageDoneToChampions = endOfGame.TrueDamageDealtToChampions;
                        frameParticipant.TotalDamageDone = endOfGame.TotalDamageDealt;
                        frameParticipant.PhysicalDamageDone = endOfGame.PhysicalDamageDealt;
                        frameParticipant.MagicDamageDone = endOfGame.MagicDamageDealt;
                        frameParticipant.TrueDamageDone = endOfGame.TrueDamageDealt;
                        frameParticipant.TotalDamageTaken = endOfGame.TotalDamageTaken;
                        frameParticipant.PhysicalDamageTaken = endOfGame.PhysicalDamageTaken;
                        frameParticipant.MagicDamageTaken = endOfGame.MagicalDamageTaken;
                        frameParticipant.TrueDamageTaken = endOfGame.TrueDamageTaken;
                        frameParticipant.TimeEnemySpentControlled = endOfGame.TimeCCingOthers * 1000;
                    }

                    frameInDb.LoLGameTimelineFrameParticipants.Add(frameParticipant);
                }

                foreach (var evt in frame.Events ?? new List<LcuEventDto>())
                {
                    frameInDb.LoLGameTimelineEvents.Add(MapEvent(matchId, evt, puuids));
                }

                frames.Add(frameInDb);
            }

            return frames;
        }

        private static LoLGameTimelineEvent MapEvent(string matchId, LcuEventDto evt, Dictionary<int, string> puuids)
        {
            // The client sends 0 rather than omitting the participant fields it doesn't use.
            int? Identify(int? participantId)
                => participantId is null or 0 ? null : participantId;

            string? ResolvePuuid(int? participantId)
                => Identify(participantId) is int id && puuids.TryGetValue(id, out var puuid) ? puuid : null;

            return new LoLGameTimelineEvent
            {
                MatchId = matchId,
                Timestamp = evt.Timestamp,
                EventType = evt.Type ?? string.Empty,
                ParticipantId = Identify(evt.ParticipantId),
                ParticipantPUUID = ResolvePuuid(evt.ParticipantId),
                KillerId = Identify(evt.KillerId),
                KillerPUUID = ResolvePuuid(evt.KillerId),
                VictimId = Identify(evt.VictimId),
                VictimPUUID = ResolvePuuid(evt.VictimId),
                TeamId = Identify(evt.TeamId),
                ItemId = Identify(evt.ItemId),
                SkillSlot = Identify(evt.SkillSlot),
                BuildingType = NullIfEmpty(evt.BuildingType),
                TowerType = NullIfEmpty(evt.TowerType),
                LaneType = NullIfEmpty(evt.LaneType),
                MonsterType = NullIfEmpty(evt.MonsterType),
                MonsterSubType = NullIfEmpty(evt.MonsterSubType),
                PositionX = evt.Position?.X,
                PositionY = evt.Position?.Y,
                LoLGameTimelineEventAssists = (evt.AssistingParticipantIds ?? new List<int>())
                    .Select(participantId => new LoLGameTimelineEventAssist
                    {
                        MatchId = matchId,
                        ParticipantId = participantId,
                        ParticipantPUUID = ResolvePuuid(participantId),
                    })
                    .ToList(),
            };
        }

        private static string? NullIfEmpty(string? value)
            => string.IsNullOrWhiteSpace(value) ? null : value;

        private static long? DetermineFrameInterval(List<LoLGameTimelineFrame> frames)
            => frames.Count < 2 ? null : frames[1].Timestamp - frames[0].Timestamp;

        private static LoLGameTeam MapTeam(string matchId, LcuTeamDto team, List<LcuParticipantDto> participants)
        {
            return new LoLGameTeam
            {
                MatchId = matchId,
                TeamId = team.TeamId,
                Win = string.Equals(team.Win, "Win", StringComparison.OrdinalIgnoreCase),

                // The client has no team kill counter, unlike match-v5's objectives.champion.kills.
                ChampionKills = participants.Where(x => x.TeamId == team.TeamId).Sum(x => x.Stats?.Kills ?? 0),
                TowerKills = team.TowerKills,
                InhibitorKills = team.InhibitorKills,
                DragonKills = team.DragonKills,
                RiftHeraldKills = team.RiftHeraldKills,
                BaronKills = team.BaronKills,
                HordeKills = team.HordeKills,
                FirstBlood = team.FirstBlood,
                FirstTower = team.FirstTower,
                FirstInhibitor = team.FirstInhibitor,
                FirstDragon = team.FirstDargon,
                FirstBaron = team.FirstBaron,

                // Not sent by the client, and not worth deriving from the events: first rift herald
                // and first horde stay false on custom games.
                FirstRiftHerald = false,
                FirstHorde = false,
            };
        }

        /// <summary>
        /// Works out which PUUID to store for each participant, and which GameOn! player they are.
        /// The client anonymizes PUUIDs, so players are matched on their Riot ID instead, and the
        /// ones that match get their real PUUID stored: everything downstream (awards, duo stats,
        /// timeline joins) keys on it.
        /// </summary>
        /// <param name="participants">All participants of the game.</param>
        /// <param name="identities">Riot IDs, by participant ID.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>PUUID and GameOn! player ID, by participant ID.</returns>
        private async Task<Dictionary<int, (string Puuid, int? PlayerId)>> ResolvePuuids(
            List<LcuParticipantDto> participants,
            Dictionary<int, LcuPlayerDto> identities,
            CancellationToken cancellationToken)
        {
            var players = await this.context.Players
                .Where(x => x.RiotGamesNickname != null && x.RiotGamesTagLine != null && x.RiotGamesPUUID != null)
                .Select(x => new { x.Id, x.RiotGamesNickname, x.RiotGamesTagLine, x.RiotGamesPUUID })
                .ToListAsync(cancellationToken);

            var resolved = new Dictionary<int, (string Puuid, int? PlayerId)>();
            var usedPuuids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var participant in participants)
            {
                identities.TryGetValue(participant.ParticipantId, out var identity);

                var player = identity is null ? null : players.FirstOrDefault(x =>
                    string.Equals(x.RiotGamesNickname!.Trim(), identity.GameName?.Trim(), StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(x.RiotGamesTagLine!.Trim(), identity.TagLine?.Trim(), StringComparison.OrdinalIgnoreCase));

                // Falling back on the anonymized PUUID, then on the participant ID: (MatchId, Puuid)
                // is an alternate key, so it has to be there and it has to be unique within the match.
                var puuid = player?.RiotGamesPUUID ?? identity?.Puuid;

                if (string.IsNullOrWhiteSpace(puuid) || !usedPuuids.Add(puuid))
                {
                    puuid = $"LCU_{participant.ParticipantId}";
                    usedPuuids.Add(puuid);
                    player = null;
                }

                resolved[participant.ParticipantId] = (puuid, player?.Id);
            }

            return resolved;
        }
    }
}
