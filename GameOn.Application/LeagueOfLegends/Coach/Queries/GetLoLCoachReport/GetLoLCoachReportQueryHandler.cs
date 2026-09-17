// <copyright file="GetLoLCoachReportQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Coach.Queries.GetLoLCoachReport
{
    using System.Linq;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetLoLCoachReportQueryHandler class.
    /// </summary>
    public class GetLoLCoachReportQueryHandler : IRequestHandler<GetLoLCoachReportQuery, LoLCoachReportDto?>
    {
        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLoLCoachReportQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetLoLCoachReportQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<LoLCoachReportDto?> Handle(GetLoLCoachReportQuery request, CancellationToken cancellationToken)
        {
            var report = await this.context.LeagueOfLegendsGameCoachReports
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.MatchId == request.MatchId && x.PlayerId == request.PlayerId, cancellationToken);

            if (report is null)
            {
                return null;
            }

            var analysis = report.ContentJson is null
                ? null
                : JsonSerializer.Deserialize<LoLCoachAnalysisDto>(report.ContentJson, SerializerOptions);

            if (analysis is null)
            {
                // A stored report that cannot be read is worse than none: let the caller ask for a fresh one.
                return null;
            }

            return new LoLCoachReportDto
            {
                MatchId = report.MatchId,
                PlayerId = report.PlayerId,
                Analysis = analysis,
                ModelName = report.ModelName,
                GeneratedOn = report.GeneratedOn,
            };
        }
    }
}
