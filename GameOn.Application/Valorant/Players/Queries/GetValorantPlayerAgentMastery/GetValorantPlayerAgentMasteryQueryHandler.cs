// <copyright file="GetValorantPlayerAgentMasteryQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Players.Queries.GetValorantPlayerAgentMastery
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
    /// <see cref="GetValorantPlayerAgentMasteryQuery"/> handler.
    /// </summary>
    public class GetValorantPlayerAgentMasteryQueryHandler : IRequestHandler<GetValorantPlayerAgentMasteryQuery, List<ValorantAgentMastery>>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetValorantPlayerAgentMasteryQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetValorantPlayerAgentMasteryQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<List<ValorantAgentMastery>> Handle(GetValorantPlayerAgentMasteryQuery request, CancellationToken cancellationToken)
        {
            return await this.context.ValorantAgentMasteries
                .AsNoTracking()
                .Where(x => x.PlayerId == request.PlayerId)
                .OrderByDescending(x => x.Level ?? 0)
                .ThenByDescending(x => x.FlourishLongLevel ?? 0)
                .ThenBy(x => x.AgentName)
                .ToListAsync(cancellationToken);
        }
    }
}
