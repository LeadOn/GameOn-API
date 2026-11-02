// <copyright file="GetValorantPlayerAccoladesQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Players.Queries.GetValorantPlayerAccolades
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// <see cref="GetValorantPlayerAccoladesQuery"/> handler.
    /// </summary>
    public class GetValorantPlayerAccoladesQueryHandler : IRequestHandler<GetValorantPlayerAccoladesQuery, List<ValorantAccoladeSummary>>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetValorantPlayerAccoladesQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetValorantPlayerAccoladesQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<List<ValorantAccoladeSummary>> Handle(GetValorantPlayerAccoladesQuery request, CancellationToken cancellationToken)
        {
            return await this.context.ValorantAccoladeSummaries
                .AsNoTracking()
                .Where(x => x.PlayerId == request.PlayerId)
                .OrderBy(x => x.SeasonId != null)
                .ThenBy(x => x.SeasonShort)
                .ThenByDescending(x => x.Count)
                .ThenBy(x => x.Type)
                .ToListAsync(cancellationToken);
        }
    }
}
