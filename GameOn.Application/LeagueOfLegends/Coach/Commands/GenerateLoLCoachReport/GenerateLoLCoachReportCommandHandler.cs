// <copyright file="GenerateLoLCoachReportCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Coach.Commands.GenerateLoLCoachReport
{
    using System;
    using System.Linq;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Application.LeagueOfLegends.Coach.Services;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using GameOn.External.Llm.Exceptions;
    using GameOn.External.Llm.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GenerateLoLCoachReportCommandHandler class.
    /// </summary>
    /// <remarks>
    /// Runs inside the caller's request and blocks for as long as the model takes, which is the point: a report
    /// is only ever written because somebody asked for it. The stored row then acts as a cache, so the same
    /// analysis is never paid for twice.
    /// Deliberately has no try/catch: a provider failure must reach the global middleware, not be swallowed
    /// into an empty report.
    /// </remarks>
    public class GenerateLoLCoachReportCommandHandler : IRequestHandler<GenerateLoLCoachReportCommand, LoLCoachReportDto?>
    {
        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        private readonly IApplicationDbContext context;
        private readonly ILlmService llmService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateLoLCoachReportCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="llmService">ILlmService interface, injected.</param>
        public GenerateLoLCoachReportCommandHandler(IApplicationDbContext context, ILlmService llmService)
        {
            this.context = context;
            this.llmService = llmService;
        }

        /// <inheritdoc />
        public async Task<LoLCoachReportDto?> Handle(GenerateLoLCoachReportCommand request, CancellationToken cancellationToken)
        {
            var game = await this.context.LeagueOfLegendsGames
                .Include(x => x.Queue)
                .Include(x => x.LeagueOfLegendsGameTeams)
                .Include(x => x.LeagueOfLegendsGameParticipants)
                    .ThenInclude(x => x.Stats)
                .Include(x => x.LeagueOfLegendsGameParticipants)
                    .ThenInclude(x => x.Challenges)
                .FirstOrDefaultAsync(x => x.MatchId == request.MatchId, cancellationToken);

            var target = game?.LeagueOfLegendsGameParticipants?.FirstOrDefault(x => x.PlayerId == request.PlayerId);

            if (game is null || target is null)
            {
                // Either the match is unknown, or that player simply did not play it.
                return null;
            }

            var report = await this.context.LeagueOfLegendsGameCoachReports
                .FirstOrDefaultAsync(x => x.MatchId == game.MatchId && x.Puuid == target.Puuid, cancellationToken);

            if (report is not null && !request.ForceRegenerate)
            {
                var stored = Deserialize(report.ContentJson);

                if (stored is not null)
                {
                    return ToDto(report, stored);
                }
            }

            if (!this.llmService.IsConfigured)
            {
                throw new LlmException("No LLM provider is configured: set GEMINI_API_KEY.");
            }

            var frames = await this.context.LeagueOfLegendsGameTimelineFrames
                .Where(x => x.MatchId == game.MatchId)
                .Include(x => x.LoLGameTimelineFrameParticipants)
                .ToListAsync(cancellationToken);

            // Only the coached player's own deaths: the other 40-odd kills of a game would add pages of context
            // for nothing, since the analysis is about them.
            var deaths = await this.context.LeagueOfLegendsGameTimelineEvents
                .Where(x => x.MatchId == game.MatchId && x.EventType == "CHAMPION_KILL" && x.VictimPUUID == target.Puuid)
                .ToListAsync(cancellationToken);

            var brief = LoLCoachContextBuilder.Build(game, target, frames, deaths);
            var answer = await this.llmService.GenerateJsonAsync(LoLCoachPrompt.SystemPrompt, brief, LoLCoachPrompt.ResponseSchema, cancellationToken);

            var analysis = Deserialize(answer)
                ?? throw new LlmException("The model answered with a document that does not match the coach schema.");

            var isNew = report is null;
            report ??= new LoLGameCoachReport { MatchId = game.MatchId, Puuid = target.Puuid };

            report.PlayerId = target.PlayerId;
            report.Summary = Truncate(analysis.Synthese, 2000);
            report.Rating = analysis.NoteSur10;
            report.ContentJson = answer;
            report.ModelName = this.llmService.ModelName;
            report.PromptVersion = LoLCoachPrompt.Version;
            report.GeneratedOn = DateTime.UtcNow;

            if (isNew)
            {
                this.context.LeagueOfLegendsGameCoachReports.Add(report);
            }
            else
            {
                this.context.LeagueOfLegendsGameCoachReports.Update(report);
            }

            await this.context.SaveChangesAsync(cancellationToken);

            return ToDto(report, analysis);
        }

        /// <summary>
        /// Reads a stored or freshly generated analysis.
        /// </summary>
        /// <param name="json">The JSON document.</param>
        /// <returns>The analysis, or null when it cannot be read.</returns>
        private static LoLCoachAnalysisDto? Deserialize(string? json)
        {
            return string.IsNullOrWhiteSpace(json) ? null : JsonSerializer.Deserialize<LoLCoachAnalysisDto>(json, SerializerOptions);
        }

        /// <summary>
        /// Maps a report and its analysis to the API shape.
        /// </summary>
        /// <param name="report">The stored report.</param>
        /// <param name="analysis">Its analysis.</param>
        /// <returns>The DTO.</returns>
        private static LoLCoachReportDto ToDto(LoLGameCoachReport report, LoLCoachAnalysisDto analysis)
        {
            return new LoLCoachReportDto
            {
                MatchId = report.MatchId,
                PlayerId = report.PlayerId,
                Analysis = analysis,
                ModelName = report.ModelName,
                GeneratedOn = report.GeneratedOn,
            };
        }

        /// <summary>
        /// Cuts a string to the column width, so that an over-long verdict cannot fail the whole save.
        /// </summary>
        /// <param name="value">Value to cut.</param>
        /// <param name="maxLength">Maximum length.</param>
        /// <returns>The truncated value.</returns>
        private static string Truncate(string value, int maxLength)
        {
            return value.Length <= maxLength ? value : value[..maxLength];
        }
    }
}
