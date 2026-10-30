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
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using GameOn.External.HenrikDev.Models.DTOs;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// <see cref="UpsertValorantMatchesCommand"/> handler.
    /// </summary>
    public class UpsertValorantMatchesCommandHandler : IRequestHandler<UpsertValorantMatchesCommand, List<ValGame>>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsertValorantMatchesCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public UpsertValorantMatchesCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
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

                var teamsInDb = matchDto.Teams.Select(t => new ValGameTeam
                {
                    MatchId = matchId,
                    TeamId = t.TeamId,
                    Won = t.Won,
                    RoundsWon = t.Rounds?.Won ?? 0,
                    RoundsLost = t.Rounds?.Lost ?? 0,
                }).ToList();

                var roundsInDb = matchDto.Rounds.Select(r => new ValGameRound
                {
                    MatchId = matchId,
                    RoundNumber = r.Id,
                    Result = r.Result,
                    Ceremony = r.Ceremony,
                    WinningTeam = r.WinningTeam,
                    WinningTeamRole = r.WinningTeamRole,
                    FirstBloodPuuid = r.FirstBlood?.Puuid,
                    FirstBloodTeam = r.FirstBlood?.Team,
                    CeremonyPlayerPuuid = r.CeremonyPlayer?.Puuid,
                    CeremonyTeam = r.CeremonyTeam,
                    PlantTimeMs = r.Plant?.RoundTimeInMs,
                    PlantSite = r.Plant?.Site,
                    PlantPlayerPuuid = r.Plant?.Player?.Puuid,
                    DefuseTimeMs = r.Defuse?.RoundTimeInMs,
                    DefusePlayerPuuid = r.Defuse?.Player?.Puuid,
                }).ToList();

                var killsInDb = matchDto.Kills.Select(k => new ValGameKill
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
                }).ToList();

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
                        }))
                    .ToList();

                var participantsInDb = new List<ValGameParticipant>();

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
                    };

                    if (hasPlayer && rankByPlayerAndMatch.TryGetValue((linkedPlayerId, matchId.ToLowerInvariant()), out var rankEntry))
                    {
                        participantInDb.RrChange = rankEntry.LastChange;
                        participantInDb.PostGameTierId = rankEntry.TierId;
                        participantInDb.PostGameTierName = rankEntry.TierName;
                        participantInDb.PostGameRankedRating = rankEntry.RankedRating;
                    }

                    participantsInDb.Add(participantInDb);
                }

                this.context.ValorantGameTeams.AddRange(teamsInDb);
                this.context.ValorantGameRounds.AddRange(roundsInDb);
                this.context.ValorantGameKills.AddRange(killsInDb);
                this.context.ValorantGameRoundPlayerStats.AddRange(roundPlayerStatsInDb);
                this.context.ValorantGameParticipants.AddRange(participantsInDb);

                // Flush to assign SQL Server identity IDs on participantsInDb before computing stats and MVP/ACE FKs.
                await this.context.SaveChangesAsync(cancellationToken);

                ValGameParticipantStatCalculator.ComputeAll(participantsInDb, roundsInDb, killsInDb);

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

                if (participant.PlayerId is not null
                    && participant.RrChange is null
                    && rankByPlayerAndMatch.TryGetValue((participant.PlayerId.Value, participant.MatchId.ToLowerInvariant()), out var rankEntry))
                {
                    participant.RrChange = rankEntry.LastChange;
                    participant.PostGameTierId = rankEntry.TierId;
                    participant.PostGameTierName = rankEntry.TierName;
                    participant.PostGameRankedRating = rankEntry.RankedRating;
                }
            }
        }

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
