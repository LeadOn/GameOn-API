// <copyright file="UpsertValorantMatchesCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Matches.Commands.UpsertValorantMatches
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Application.Valorant.Matches.Services;
    using GameOn.Application.Valorant.Players.Services;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using GameOn.External.HenrikDev.Models.DTOs;
    using GameOn.External.ValorantApi.Interfaces;
    using GameOn.External.ValorantApi.Models.DTOs;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// <see cref="UpsertValorantMatchesCommand"/> handler.
    /// </summary>
    public class UpsertValorantMatchesCommandHandler : IRequestHandler<UpsertValorantMatchesCommand, List<ValGame>>
    {
        private const string EmptyGuid = "00000000-0000-0000-0000-000000000000";

        private readonly IApplicationDbContext context;
        private readonly IValorantMapService mapService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsertValorantMatchesCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mapService">Valorant map reference data (named zones), injected.</param>
        public UpsertValorantMatchesCommandHandler(IApplicationDbContext context, IValorantMapService mapService)
        {
            this.context = context;
            this.mapService = mapService;
        }

        /// <inheritdoc />
        public async Task<List<ValGame>> Handle(UpsertValorantMatchesCommand request, CancellationToken cancellationToken)
        {
            var validMatches = request.Matches
                .Where(m => m.Metadata is not null && !string.IsNullOrWhiteSpace(m.Metadata.MatchId))
                .GroupBy(m => m.Metadata!.MatchId, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.First())
                .ToList();

            if (validMatches.Count == 0)
            {
                return new List<ValGame>();
            }

            var matchIds = validMatches.Select(m => m.Metadata!.MatchId).ToList();

            var trackedPlayersByPuuid = await this.context.Players
                .Where(p => p.ValorantPuuid != null)
                .Select(p => new { p.Id, Puuid = p.ValorantPuuid! })
                .ToListAsync(cancellationToken);

            var playerIdByPuuid = trackedPlayersByPuuid
                .GroupBy(p => p.Puuid, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.First().Id, StringComparer.OrdinalIgnoreCase);

            var rankEntries = await this.context.ValorantRankHistory
                .Where(r => r.MatchId != null && matchIds.Contains(r.MatchId))
                .ToListAsync(cancellationToken);

            var rankByPlayerAndMatch = rankEntries
                .GroupBy(r => (r.PlayerId, MatchId: r.MatchId!.ToLowerInvariant()))
                .ToDictionary(g => g.Key, g => g.OrderByDescending(x => x.Id).First());

            var existingGames = await this.context.ValorantGames
                .Include(g => g.ValorantGameParticipants)
                .Where(g => matchIds.Contains(g.MatchId))
                .ToListAsync(cancellationToken);

            var existingByMatchId = existingGames.ToDictionary(g => g.MatchId, StringComparer.OrdinalIgnoreCase);
            var persistedGames = new List<ValGame>();
            var mapsById = await this.mapService.GetMapsById(cancellationToken);

            foreach (var matchDto in validMatches)
            {
                var meta = matchDto.Metadata!;
                var matchId = meta.MatchId;

                if (existingByMatchId.TryGetValue(matchId, out var existingGame) && !request.OverwriteExisting)
                {
                    BackfillParticipantLinksAndRanks(existingGame.ValorantGameParticipants, playerIdByPuuid, rankByPlayerAndMatch);
                    persistedGames.Add(existingGame);
                    continue;
                }

                ValGame gameInDb;

                if (existingGame is not null)
                {
                    existingGame.MvpParticipantId = null;
                    existingGame.AceParticipantId = null;
                    await this.context.SaveChangesAsync(cancellationToken);

                    var oldParticipants = await this.context.ValorantGameParticipants.Where(x => x.MatchId == matchId).ToListAsync(cancellationToken);
                    var oldTeams = await this.context.ValorantGameTeams.Where(x => x.MatchId == matchId).ToListAsync(cancellationToken);
                    var oldRounds = await this.context.ValorantGameRounds.Where(x => x.MatchId == matchId).ToListAsync(cancellationToken);
                    var oldKills = await this.context.ValorantGameKills.Where(x => x.MatchId == matchId).ToListAsync(cancellationToken);
                    var oldRoundPlayerStats = await this.context.ValorantGameRoundPlayerStats.Where(x => x.MatchId == matchId).ToListAsync(cancellationToken);
                    var oldLocations = await this.context.ValorantGameEventPlayerLocations.Where(x => x.MatchId == matchId).ToListAsync(cancellationToken);
                    var oldDamages = await this.context.ValorantGameRoundDamages.Where(x => x.MatchId == matchId).ToListAsync(cancellationToken);

                    // Locations first: their link to a kill does not cascade.
                    this.context.ValorantGameEventPlayerLocations.RemoveRange(oldLocations);
                    this.context.ValorantGameRoundDamages.RemoveRange(oldDamages);
                    this.context.ValorantGameParticipants.RemoveRange(oldParticipants);
                    this.context.ValorantGameTeams.RemoveRange(oldTeams);
                    this.context.ValorantGameRounds.RemoveRange(oldRounds);
                    this.context.ValorantGameKills.RemoveRange(oldKills);
                    this.context.ValorantGameRoundPlayerStats.RemoveRange(oldRoundPlayerStats);
                    await this.context.SaveChangesAsync(cancellationToken);

                    gameInDb = existingGame;
                }
                else
                {
                    gameInDb = new ValGame { MatchId = matchId };
                    this.context.ValorantGames.Add(gameInDb);
                }

                var queueId = NormalizeQueueId(meta.Queue?.Id, meta.Queue?.Name);
                var gameStart = ParseUtcDate(meta.StartedAt);
                var isRemake = (string.Equals(queueId, "competitive", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(queueId, "unrated", StringComparison.OrdinalIgnoreCase))
                    && matchDto.Rounds.Count > 0
                    && matchDto.Rounds.Count <= 3;

                var winningTeamId = isRemake
                    ? null
                    : matchDto.Teams.FirstOrDefault(t => t.Won)?.TeamId;

                gameInDb.MapId = meta.Map?.Id;
                gameInDb.MapName = meta.Map?.Name;
                gameInDb.GameVersion = meta.GameVersion;
                gameInDb.Region = meta.Region;
                gameInDb.Cluster = meta.Cluster;
                gameInDb.Platform = meta.Platform;
                gameInDb.QueueId = queueId;
                gameInDb.QueueName = meta.Queue?.Name;
                gameInDb.QueueModeType = meta.Queue?.ModeType;
                gameInDb.SeasonId = meta.Season?.Id;
                gameInDb.SeasonShort = meta.Season?.Short;
                gameInDb.RetrievedOn = DateTime.UtcNow;
                gameInDb.GameStart = gameStart;
                gameInDb.GameLengthMs = meta.GameLengthInMs;
                gameInDb.GameEnd = gameStart.AddMilliseconds(meta.GameLengthInMs);
                gameInDb.IsCompleted = meta.IsCompleted;
                gameInDb.IsRemake = isRemake;
                gameInDb.WinningTeamId = winningTeamId;
                gameInDb.RiotMvpPuuid = meta.Mvp?.Puuid;
                gameInDb.PremierInfoJson = meta.Premier is null || meta.Premier.Type == JTokenType.Null ? null : meta.Premier.ToString(Formatting.None);

                var map = meta.Map?.Id is { } mapId ? mapsById.GetValueOrDefault(mapId) : null;

                var teamsInDb = matchDto.Teams.Select(t => new ValGameTeam
                {
                    MatchId = matchId,
                    TeamId = t.TeamId,
                    Won = t.Won,
                    RoundsWon = t.Rounds?.Won ?? 0,
                    RoundsLost = t.Rounds?.Lost ?? 0,
                    Placement = t.Placement,
                    RiotMvpPuuid = t.Mvp?.Puuid,
                    PremierRosterId = t.PremierRoster?.Id,
                    PremierTeamName = t.PremierRoster?.Name,
                    PremierTeamTag = t.PremierRoster?.Tag,
                }).ToList();

                // Matches from before Riot recorded it have no first blood: it is the first kill of the round.
                var firstKillByRound = matchDto.Kills
                    .GroupBy(k => k.Round)
                    .ToDictionary(g => g.Key, g => g.OrderBy(k => k.TimeInRoundInMs).First());

                var roundsInDb = matchDto.Rounds.Select(r => new ValGameRound
                {
                    MatchId = matchId,
                    RoundNumber = r.Id,
                    Result = r.Result,
                    Ceremony = r.Ceremony,
                    WinningTeam = r.WinningTeam,
                    WinningTeamRole = r.WinningTeamRole,
                    FirstBloodPuuid = r.FirstBlood?.Puuid ?? firstKillByRound.GetValueOrDefault(r.Id)?.Killer?.Puuid,
                    FirstBloodTeam = r.FirstBlood?.Team ?? firstKillByRound.GetValueOrDefault(r.Id)?.Killer?.Team,
                    CeremonyPlayerPuuid = r.CeremonyPlayer?.Puuid,
                    CeremonyTeam = r.CeremonyTeam,
                    PlantTimeMs = r.Plant?.RoundTimeInMs,
                    PlantSite = r.Plant?.Site,
                    PlantPlayerPuuid = r.Plant?.Player?.Puuid,
                    DefuseTimeMs = r.Defuse?.RoundTimeInMs,
                    DefusePlayerPuuid = r.Defuse?.Player?.Puuid,
                    PlantX = r.Plant?.Location?.X,
                    PlantY = r.Plant?.Location?.Y,
                    PlantZone = ValMapZoneResolver.Resolve(map, r.Plant?.Location?.X, r.Plant?.Location?.Y),
                    DefuseX = r.Defuse?.Location?.X,
                    DefuseY = r.Defuse?.Location?.Y,
                }).ToList();

                var locationsInDb = matchDto.Rounds
                    .SelectMany(r => ToLocations(matchId, r.Id, "Plant", r.Plant?.PlayerLocations)
                        .Concat(ToLocations(matchId, r.Id, "Defuse", r.Defuse?.PlayerLocations)))
                    .ToList();

                var killsInDb = matchDto.Kills.Select(k => ToKill(matchId, k, map)).ToList();

                var damagesInDb = matchDto.Rounds
                    .SelectMany(r => r.Stats
                        .Where(x => !string.IsNullOrWhiteSpace(x.Player?.Puuid))
                        .SelectMany(x => x.DamageEvents
                            .Where(d => !string.IsNullOrWhiteSpace(d.Player?.Puuid))
                            .Select(d => new ValGameRoundDamage
                            {
                                MatchId = matchId,
                                RoundNumber = r.Id,
                                AttackerPuuid = x.Player!.Puuid!,
                                ReceiverPuuid = d.Player!.Puuid!,
                                Damage = d.Damage,
                                Headshots = d.Headshots,
                                Bodyshots = d.Bodyshots,
                                Legshots = d.Legshots,
                            })))
                    .ToList();

                var roundPlayerStatsInDb = matchDto.Rounds
                    .SelectMany(r => r.Stats
                        .Where(x => !string.IsNullOrWhiteSpace(x.Player?.Puuid))
                        .Select(x => new ValGameRoundPlayerStat
                        {
                            MatchId = matchId,
                            RoundNumber = r.Id,
                            Puuid = x.Player!.Puuid!,
                            Team = x.Player.Team,
                            LoadoutValue = x.Economy?.LoadoutValue ?? 0,
                            Remaining = x.Economy?.Remaining ?? 0,
                            WeaponId = x.Economy?.Weapon?.Id,
                            WeaponName = x.Economy?.Weapon?.Name,
                            ArmorId = x.Economy?.Armor?.Id,
                            ArmorName = x.Economy?.Armor?.Name,
                            Score = x.Stats?.Score ?? 0,
                            Kills = x.Stats?.Kills ?? 0,
                            Headshots = x.Stats?.Headshots ?? 0,
                            Bodyshots = x.Stats?.Bodyshots ?? 0,
                            Legshots = x.Stats?.Legshots ?? 0,
                            Damage = x.DamageEvents.Sum(d => d.Damage),
                            WasAfk = x.WasAfk,
                            ReceivedPenalty = x.ReceivedPenalty,
                            StayedInSpawn = x.StayedInSpawn,
                        }))
                    .ToList();

                var participantsInDb = new List<ValGameParticipant>();
                var partyPenaltyById = (meta.PartyRrPenalties ?? new List<ValorantMatchPartyRrPenaltyDto>())
                    .Where(x => !string.IsNullOrWhiteSpace(x.PartyId))
                    .GroupBy(x => x.PartyId!, StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(g => g.Key, g => g.First().Penalty, StringComparer.OrdinalIgnoreCase);
                var teamMvpPuuids = new HashSet<string>(
                    matchDto.Teams.Select(t => t.Mvp?.Puuid).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x!),
                    StringComparer.OrdinalIgnoreCase);

                foreach (var p in matchDto.Players.Where(x => !string.IsNullOrWhiteSpace(x.Puuid)))
                {
                    var hasPlayer = playerIdByPuuid.TryGetValue(p.Puuid, out var linkedPlayerId);
                    var won = !isRemake
                        && !string.IsNullOrWhiteSpace(winningTeamId)
                        && string.Equals(p.TeamId, winningTeamId, StringComparison.OrdinalIgnoreCase);

                    var participantInDb = new ValGameParticipant
                    {
                        MatchId = matchId,
                        PlayerId = hasPlayer ? linkedPlayerId : null,
                        Puuid = p.Puuid,
                        RiotIdGameName = p.Name,
                        RiotIdTagLine = p.Tag,
                        TeamId = p.TeamId,
                        PartyId = p.PartyId,
                        Platform = p.Platform,
                        AccountLevel = p.AccountLevel ?? 0,
                        PlayerCardId = p.Customization?.Card,
                        PlayerTitleId = p.Customization?.Title,
                        AgentId = p.Agent?.Id,
                        AgentName = p.Agent?.Name,
                        TierId = p.Tier?.Id ?? 0,
                        TierName = p.Tier?.Name,
                        Score = p.Stats?.Score ?? 0,
                        Kills = p.Stats?.Kills ?? 0,
                        Deaths = p.Stats?.Deaths ?? 0,
                        Assists = p.Stats?.Assists ?? 0,
                        Headshots = p.Stats?.Headshots ?? 0,
                        Bodyshots = p.Stats?.Bodyshots ?? 0,
                        Legshots = p.Stats?.Legshots ?? 0,
                        DamageDealt = p.Stats?.Damage?.Dealt ?? 0,
                        DamageReceived = p.Stats?.Damage?.Received ?? 0,
                        GrenadeCasts = p.AbilityCasts?.Grenade ?? 0,
                        Ability1Casts = p.AbilityCasts?.Ability1 ?? 0,
                        Ability2Casts = p.AbilityCasts?.Ability2 ?? 0,
                        UltimateCasts = p.AbilityCasts?.Ultimate ?? 0,
                        SpentOverall = p.Economy?.Spent?.Overall ?? 0,
                        SpentAverage = p.Economy?.Spent?.Average ?? 0,
                        LoadoutValueOverall = p.Economy?.LoadoutValue?.Overall ?? 0,
                        LoadoutValueAverage = p.Economy?.LoadoutValue?.Average ?? 0,
                        AfkRounds = p.Behavior?.AfkRounds ?? 0,
                        RoundsInSpawn = p.Behavior?.RoundsInSpawn ?? 0,
                        FriendlyFireIncoming = p.Behavior?.FriendlyFire?.Incoming ?? 0,
                        FriendlyFireOutgoing = p.Behavior?.FriendlyFire?.Outgoing ?? 0,
                        SessionPlaytimeMs = p.SessionPlaytimeInMs ?? 0,
                        Win = won,
                        TeamNumber = p.TeamNumber,
                        PartyRrPenalty = p.PartyId is not null && partyPenaltyById.TryGetValue(p.PartyId, out var penalty) ? penalty : null,
                        IsRiotMatchMvp = string.Equals(meta.Mvp?.Puuid, p.Puuid, StringComparison.OrdinalIgnoreCase),
                        IsRiotTeamMvp = teamMvpPuuids.Contains(p.Puuid),
                        BloomlineLevel = p.Customization?.Bloomline?.Level is > 0 ? p.Customization.Bloomline.Level : null,
                        PreferredLevelBorderId = NullIfEmptyGuid(p.Customization?.PreferredLevelBorder),
                        Performance = ToPerformance(p.Performance),
                    };

                    if (hasPlayer && rankByPlayerAndMatch.TryGetValue((linkedPlayerId, matchId.ToLowerInvariant()), out var rankEntry))
                    {
                        ValorantRankHistorySync.ApplyToParticipant(participantInDb, rankEntry);
                    }

                    participantsInDb.Add(participantInDb);
                }

                this.context.ValorantGameTeams.AddRange(teamsInDb);
                this.context.ValorantGameRounds.AddRange(roundsInDb);
                this.context.ValorantGameKills.AddRange(killsInDb);
                this.context.ValorantGameRoundPlayerStats.AddRange(roundPlayerStatsInDb);
                this.context.ValorantGameEventPlayerLocations.AddRange(locationsInDb);
                this.context.ValorantGameRoundDamages.AddRange(damagesInDb);
                this.context.ValorantGameParticipants.AddRange(participantsInDb);

                // Flush to assign SQL Server identity IDs on participantsInDb before computing stats and MVP/ACE FKs.
                await this.context.SaveChangesAsync(cancellationToken);

                ValGameAdvancedStatCalculator.ResolveSides(roundsInDb, participantsInDb, queueId);
                ValGameParticipantStatCalculator.ComputeAll(participantsInDb, roundsInDb, killsInDb);
                ValGameAdvancedStatCalculator.ComputeAll(participantsInDb, roundsInDb, killsInDb, roundPlayerStatsInDb, queueId);

                var (mvpParticipantId, aceParticipantId) = ValGameParticipantStatCalculator.DetermineMvpAndAce(
                    participantsInDb,
                    winningTeamId,
                    isRemake);
                gameInDb.MvpParticipantId = mvpParticipantId;
                gameInDb.AceParticipantId = aceParticipantId;

                persistedGames.Add(gameInDb);
            }

            await this.context.SaveChangesAsync(cancellationToken);

            return persistedGames;
        }

        private static void BackfillParticipantLinksAndRanks(
            IEnumerable<ValGameParticipant> participants,
            Dictionary<string, int> playerIdByPuuid,
            Dictionary<(int PlayerId, string MatchId), ValorantRankHistory> rankByPlayerAndMatch)
        {
            foreach (var participant in participants)
            {
                if (participant.PlayerId is null && playerIdByPuuid.TryGetValue(participant.Puuid, out var linkedPlayerId))
                {
                    participant.PlayerId = linkedPlayerId;
                }

                // Always re-applied, not only when empty: the rank entry may have gained its real RR delta since.
                if (participant.PlayerId is not null
                    && rankByPlayerAndMatch.TryGetValue((participant.PlayerId.Value, participant.MatchId.ToLowerInvariant()), out var rankEntry))
                {
                    ValorantRankHistorySync.ApplyToParticipant(participant, rankEntry);
                }
            }
        }

        private static ValGameKill ToKill(string matchId, ValorantMatchKillDto k, ValorantApiMapDto? map)
        {
            var killerLocation = k.PlayerLocations.FirstOrDefault(x =>
                !string.IsNullOrWhiteSpace(x.Player?.Puuid)
                && string.Equals(x.Player.Puuid, k.Killer?.Puuid, StringComparison.OrdinalIgnoreCase));

            var kill = new ValGameKill
            {
                MatchId = matchId,
                RoundNumber = k.Round,
                TimeInRoundMs = k.TimeInRoundInMs,
                TimeInMatchMs = k.TimeInMatchInMs,
                KillerPuuid = k.Killer?.Puuid,
                KillerTeam = k.Killer?.Team,
                VictimPuuid = k.Victim?.Puuid,
                VictimTeam = k.Victim?.Team,
                AssistantsCount = k.Assistants.Count,
                AssistantPuuids = k.Assistants.Count > 0
                    ? string.Join(",", k.Assistants.Where(a => !string.IsNullOrWhiteSpace(a.Puuid)).Select(a => a.Puuid))
                    : null,
                WeaponId = k.Weapon?.Id,
                WeaponName = k.Weapon?.Name,
                WeaponType = k.Weapon?.Type,
                SecondaryFireMode = k.SecondaryFireMode,
                VictimX = k.Location?.X,
                VictimY = k.Location?.Y,
                KillerX = killerLocation?.Location?.X,
                KillerY = killerLocation?.Location?.Y,
                KillerViewRadians = killerLocation?.ViewRadians,
                VictimZone = ValMapZoneResolver.Resolve(map, k.Location?.X, k.Location?.Y),
                KillerZone = ValMapZoneResolver.Resolve(map, killerLocation?.Location?.X, killerLocation?.Location?.Y),
            };

            // Game units are centimeters.
            if (kill.KillerX is not null && kill.VictimX is not null)
            {
                var dx = kill.KillerX.Value - kill.VictimX.Value;
                var dy = kill.KillerY!.Value - kill.VictimY!.Value;
                kill.DistanceMeters = Math.Round(Math.Sqrt((dx * (double)dx) + (dy * (double)dy)) / 100.0, 1);
            }

            kill.PlayerLocations = ToLocations(matchId, k.Round, "Kill", k.PlayerLocations).ToList();
            return kill;
        }

        private static IEnumerable<ValGameEventPlayerLocation> ToLocations(string matchId, int roundNumber, string eventType, IEnumerable<ValorantMatchPlayerLocationDto>? locations)
        {
            return (locations ?? Enumerable.Empty<ValorantMatchPlayerLocationDto>())
                .Where(x => !string.IsNullOrWhiteSpace(x.Player?.Puuid) && x.Location is not null)
                .Select(x => new ValGameEventPlayerLocation
                {
                    MatchId = matchId,
                    RoundNumber = roundNumber,
                    EventType = eventType,
                    Puuid = x.Player!.Puuid!,
                    Team = x.Player.Team,
                    X = x.Location!.X,
                    Y = x.Location.Y,
                    ViewRadians = x.ViewRadians,
                });
        }

        private static ValGameParticipantPerformance? ToPerformance(ValorantMatchPerformanceDto? performance)
        {
            if (performance is null)
            {
                return null;
            }

            return new ValGameParticipantPerformance
            {
                Score = performance.Score,
                Grade = performance.Ratings?.Grade,
                CombatKillImpact = performance.Ratings?.Combat?.KillImpact,
                CombatDeathImpact = performance.Ratings?.Combat?.DeathImpact,
                CombatDamage = performance.Ratings?.Combat?.Damage,
                CombatTrades = performance.Ratings?.Combat?.Trades,
                UtilityAssists = performance.Ratings?.Utility?.Assists,
                UtilityPlants = performance.Ratings?.Utility?.Plants,
                UtilityDefuses = performance.Ratings?.Utility?.Defuses,
                UtilityUsage = performance.Ratings?.Utility?.UtilityUsage,
                AdjustedKills = performance.Breakdown?.AdjustedKills,
                AdjustedDeaths = performance.Breakdown?.AdjustedDeaths,
                Assists = performance.Breakdown?.Assists,
                Damage = performance.Breakdown?.Damage,
                Trades = performance.Breakdown?.Trades,
                UtilityUsagePerRound = performance.Breakdown?.UtilityUsage,
                Plants = performance.Breakdown?.Plants,
                Defuses = performance.Breakdown?.Defuses,
            };
        }

        private static string? NullIfEmptyGuid(string? value) =>
            string.IsNullOrWhiteSpace(value) || value == EmptyGuid ? null : value;

        private static string NormalizeQueueId(string? queueId, string? queueName)
        {
            if (!string.IsNullOrWhiteSpace(queueId))
            {
                return queueId.Trim().ToLowerInvariant();
            }

            if (!string.IsNullOrWhiteSpace(queueName) && queueName.Contains("Custom", StringComparison.OrdinalIgnoreCase))
            {
                return "custom";
            }

            return "unknown";
        }

        private static DateTime ParseUtcDate(string? rawDate)
        {
            if (!string.IsNullOrWhiteSpace(rawDate)
                && DateTimeOffset.TryParse(rawDate, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var parsed))
            {
                return parsed.UtcDateTime;
            }

            return DateTime.UtcNow;
        }
    }
}
