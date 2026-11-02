// <copyright file="GetValorantPremierTeamQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Premier.Queries.GetValorantPremierTeam
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// <see cref="GetValorantPremierTeamQuery"/> handler.
    /// </summary>
    public class GetValorantPremierTeamQueryHandler : IRequestHandler<GetValorantPremierTeamQuery, ValorantPremierTeam?>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetValorantPremierTeamQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetValorantPremierTeamQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<ValorantPremierTeam?> Handle(GetValorantPremierTeamQuery request, CancellationToken cancellationToken)
        {
            return await this.context.ValorantPremierTeams
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.TeamId, cancellationToken);
        }
    }
}
