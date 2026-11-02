// <copyright file="RefreshValorantPlayerExtrasCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Background.Commands.RefreshValorantPlayerExtras
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Application.Valorant.Players.Services;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using GameOn.External.HenrikDev.Interfaces;
    using GameOn.External.HenrikDev.Models.DTOs;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// <see cref="RefreshValorantPlayerExtrasCommand"/> handler.
    /// </summary>
    /// <remarks>
    /// The accolades endpoint only covers the last 11 to 20 ranked matches, so this has to run at least daily
    /// for no match to roll out of the window before its accolades were read. It returns them for the 10
    /// players of each match: the other crew members of a premade get theirs on this pass too.
    /// </remarks>
    public class RefreshValorantPlayerExtrasCommandHandler : IRequestHandler<RefreshValorantPlayerExtrasCommand, string>
    {
        private readonly IApplicationDbContext context;
        private readonly IValorantPlayerExtrasService extrasService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshValorantPlayerExtrasCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="extrasService">HenrikDev accolades, mastery and Premier, injected.</param>
        public RefreshValorantPlayerExtrasCommandHandler(IApplicationDbContext context, IValorantPlayerExtrasService extrasService)
        {
            this.context = context;
            this.extrasService = extrasService;
        }

        /// <inheritdoc />
        public async Task<string> Handle(RefreshValorantPlayerExtrasCommand request, CancellationToken cancellationToken)
        {
            var player = await this.context.Players.FirstOrDefaultAsync(x => x.Id == request.PlayerId, cancellationToken);

            if (string.IsNullOrWhiteSpace(player?.ValorantPuuid))
            {
                return $"Player {request.PlayerId} has no Valorant account.";
            }

            var region = string.IsNullOrWhiteSpace(player.ValorantRegion) ? "eu" : player.ValorantRegion;

            var accolades = await this.extrasService.GetAccolades(region, player.ValorantPuuid, cancellationToken);
            var (summaries, matchAccolades) = await this.SyncAccolades(player.Id, accolades, cancellationToken);

            var mastery = await this.extrasService.GetAgentMastery(region, player.ValorantPuuid, cancellationToken);
            var agents = await this.SyncMastery(player.Id, mastery, cancellationToken);

            var premier = await this.extrasService.GetPremierTeamByPlayer(region, player.ValorantPuuid, cancellationToken);
            await this.SyncPremier(player, premier, cancellationToken);

            player.ValorantExtrasRefreshedOn = DateTime.UtcNow;
            await this.context.SaveChangesAsync(cancellationToken);

            return $"Player {player.Id}: {summaries} accolade summaries, {matchAccolades} match accolades, {agents} agent masteries, "
                + (premier is null ? "no Premier team." : $"Premier team {premier.Name}#{premier.Tag}.");
        }

        private static string? Raw(JToken? token) =>
            token is null || token.Type == JTokenType.Null ? null : token.ToString(Formatting.None);

        private static string? NullIfEmptyGuid(string? value) =>
            string.IsNullOrWhiteSpace(value) || value == "00000000-0000-0000-0000-000000000000" ? null : value;

        private static ValorantAccoladeSummary ToSummary(int playerId, string? seasonId, string? seasonShort, ValorantAccoladeTotalDto total, DateTime now) => new ValorantAccoladeSummary
        {
            PlayerId = playerId,
            SeasonId = seasonId,
            SeasonShort = seasonShort,
            AccoladeId = total.Id,
            Type = total.Type ?? string.Empty,
            Count = total.Count,
            BestValue = total.BestValue,
            RefreshedOn = now,
        };

        private async Task<(int Summaries, int MatchAccolades)> SyncAccolades(int playerId, ValorantAccoladesDto? accolades, CancellationToken cancellationToken)
        {
            if (accolades is null)
            {
                return (0, 0);
            }

            // Summaries: a snapshot, replaced as a whole.
            var oldSummaries = await this.context.ValorantAccoladeSummaries.Where(x => x.PlayerId == playerId).ToListAsync(cancellationToken);
            this.context.ValorantAccoladeSummaries.RemoveRange(oldSummaries);

            var summaries = new List<ValorantAccoladeSummary>();
            var now = DateTime.UtcNow;

            foreach (var total in accolades.Summary?.AllTime ?? new List<ValorantAccoladeTotalDto>())
            {
                summaries.Add(ToSummary(playerId, null, null, total, now));
            }

            foreach (var season in accolades.Summary?.Seasons ?? new List<ValorantAccoladeSeasonDto>())
            {
                summaries.AddRange(season.Accolades.Select(total => ToSummary(playerId, season.Season?.Id, season.Season?.Short, total, now)));
            }

            this.context.ValorantAccoladeSummaries.AddRange(summaries);

            // Per match: only kept for matches in database, every player of the match. Upserted, never deleted:
            // a match that rolled out of HenrikDev's window keeps the accolades read while it was in.
            var matchIds = accolades.Matches.Select(x => x.MatchId).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x!).ToList();
            var participants = await this.context.ValorantGameParticipants
                .Include(x => x.Accolades)
                .Where(x => matchIds.Contains(x.MatchId))
                .ToListAsync(cancellationToken);

            var participantByKey = participants
                .GroupBy(x => (MatchId: x.MatchId.ToLowerInvariant(), Puuid: x.Puuid.ToLowerInvariant()))
                .ToDictionary(g => g.Key, g => g.First());

            var written = 0;

            foreach (var match in accolades.Matches.Where(x => !string.IsNullOrWhiteSpace(x.MatchId)))
            {
                foreach (var matchPlayer in match.Players)
                {
                    if (!participantByKey.TryGetValue((match.MatchId!.ToLowerInvariant(), matchPlayer.Puuid.ToLowerInvariant()), out var participant))
                    {
                        continue;
                    }

                    foreach (var accolade in matchPlayer.Accolades.Where(x => !string.IsNullOrWhiteSpace(x.Id)))
                    {
                        var row = participant.Accolades.FirstOrDefault(x => string.Equals(x.AccoladeId, accolade.Id, StringComparison.OrdinalIgnoreCase));

                        if (row is null)
                        {
                            row = new ValGameParticipantAccolade { AccoladeId = accolade.Id };
                            participant.Accolades.Add(row);
                        }

                        row.Type = accolade.Type ?? string.Empty;
                        row.Value = accolade.Value;
                        row.IsActRecord = accolade.IsActRecord;
                        written++;
                    }
                }
            }

            return (summaries.Count, written);
        }

        private async Task<int> SyncMastery(int playerId, ValorantAgentMasteryListDto? mastery, CancellationToken cancellationToken)
        {
            if (mastery is null)
            {
                return 0;
            }

            var old = await this.context.ValorantAgentMasteries.Where(x => x.PlayerId == playerId).ToListAsync(cancellationToken);
            this.context.ValorantAgentMasteries.RemoveRange(old);

            var rows = mastery.Agents
                .Where(x => !string.IsNullOrWhiteSpace(x.Agent?.Id))
                .GroupBy(x => x.Agent!.Id!, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.First())
                .Select(x => new ValorantAgentMastery
                {
                    PlayerId = playerId,
                    AgentId = x.Agent!.Id!,
                    AgentName = x.Agent.Name,
                    Level = x.Tracks.Max(t => t.Level),
                    TracksJson = JsonConvert.SerializeObject(x.Tracks.Select(t => new { t.Id, t.Level })),
                    FlourishShortLevel = x.Flourish?.ShortLevel,
                    FlourishLongLevel = x.Flourish?.LongLevel,
                    HypeId = NullIfEmptyGuid(x.Customization?.Hype?.Id),
                    BackgroundId = NullIfEmptyGuid(x.Customization?.Background?.Id),
                    ClipId = NullIfEmptyGuid(x.Customization?.Clip?.Id),
                    BorderId = NullIfEmptyGuid(x.Customization?.Border?.Id),
                    RefreshedOn = DateTime.UtcNow,
                })
                .ToList();

            this.context.ValorantAgentMasteries.AddRange(rows);
            return rows.Count;
        }

        private async Task SyncPremier(Player player, ValorantPremierTeamDto? premier, CancellationToken cancellationToken)
        {
            if (premier is null || string.IsNullOrWhiteSpace(premier.Id))
            {
                player.ValorantPremierTeamId = null;
                player.ValorantPremierRole = null;
                player.ValorantPremierJoinedOn = null;
                return;
            }

            var team = await this.context.ValorantPremierTeams.FirstOrDefaultAsync(x => x.Id == premier.Id, cancellationToken);

            if (team is null)
            {
                team = new ValorantPremierTeam { Id = premier.Id };
                this.context.ValorantPremierTeams.Add(team);
            }

            team.Name = premier.Name ?? string.Empty;
            team.Tag = premier.Tag ?? string.Empty;
            team.IconId = premier.Customization?.Icon;
            team.ImageUrl = premier.Customization?.Image;
            team.PrimaryColor = premier.Customization?.Primary;
            team.SecondaryColor = premier.Customization?.Secondary;
            team.TertiaryColor = premier.Customization?.Tertiary;
            team.CreatedOn = string.IsNullOrWhiteSpace(premier.CreatedAt) ? null : ValorantRankHistorySync.ParseUtcDate(premier.CreatedAt);
            team.CurrentSeasonId = premier.CurrentSeason?.Id;
            team.CurrentSeasonName = premier.CurrentSeason?.Name;
            team.Enrolled = premier.CurrentSeason?.Enrolled ?? false;
            team.Conference = string.IsNullOrWhiteSpace(premier.CurrentSeason?.Placement?.Conference) ? null : premier.CurrentSeason.Placement.Conference;
            team.Division = premier.CurrentSeason?.Placement?.Division;
            team.Points = premier.CurrentSeason?.Placement?.Points;
            team.Wins = premier.CurrentSeason?.Stats?.Wins ?? 0;
            team.Losses = premier.CurrentSeason?.Stats?.Losses ?? 0;
            team.RoundsWon = premier.CurrentSeason?.Stats?.Rounds?.Won ?? 0;
            team.RoundsLost = premier.CurrentSeason?.Stats?.Rounds?.Lost ?? 0;
            team.MembersJson = JsonConvert.SerializeObject(premier.Members.Select(m => new { m.Puuid, Role = m.Role?.Name, m.JoinedAt }));
            team.SeasonsJson = Raw(premier.Seasons);
            team.RefreshedOn = DateTime.UtcNow;

            var membership = premier.Members.FirstOrDefault(m => string.Equals(m.Puuid, player.ValorantPuuid, StringComparison.OrdinalIgnoreCase));
            player.ValorantPremierTeamId = premier.Id;
            player.ValorantPremierRole = membership?.Role?.Name;
            player.ValorantPremierJoinedOn = string.IsNullOrWhiteSpace(membership?.JoinedAt) ? null : ValorantRankHistorySync.ParseUtcDate(membership.JoinedAt);
        }
    }
}
