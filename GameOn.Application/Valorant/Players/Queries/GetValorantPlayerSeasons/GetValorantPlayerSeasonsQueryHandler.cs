// <copyright file="GetValorantPlayerSeasonsQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Players.Queries.GetValorantPlayerSeasons
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
    /// <see cref="GetValorantPlayerSeasonsQuery"/> handler.
    /// </summary>
    public class GetValorantPlayerSeasonsQueryHandler : IRequestHandler<GetValorantPlayerSeasonsQuery, List<ValorantSeasonResult>>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetValorantPlayerSeasonsQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetValorantPlayerSeasonsQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<List<ValorantSeasonResult>> Handle(GetValorantPlayerSeasonsQuery request, CancellationToken cancellationToken)
        {
            // The act UUID carries no order and the short code sorts badly past e9 (e10a1 < e9a1), so acts are
            // ordered on the episode and act numbers parsed from the short code.
            var results = await this.context.ValorantSeasonResults
                .AsNoTracking()
                .Where(x => x.PlayerId == request.PlayerId)
                .ToListAsync(cancellationToken);

            return results
                .OrderBy(x => ActOrder(x.SeasonShort))
                .ThenBy(x => x.SeasonShort)
                .ToList();
        }

        private static (int Episode, int Act) ActOrder(string? seasonShort)
        {
            // Format: e{episode}a{act}, e.g. e11a6.
            if (string.IsNullOrWhiteSpace(seasonShort))
            {
                return (int.MaxValue, int.MaxValue);
            }

            var parts = seasonShort.TrimStart('e', 'E').Split('a', 'A');
            return parts.Length == 2 && int.TryParse(parts[0], out var episode) && int.TryParse(parts[1], out var act)
                ? (episode, act)
                : (int.MaxValue, int.MaxValue);
        }
    }
}
