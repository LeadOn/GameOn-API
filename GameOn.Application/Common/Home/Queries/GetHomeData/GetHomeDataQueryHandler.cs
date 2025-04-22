// <copyright file="GetHomeDataQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

using GameOn.Application.Common.Changelogs.Queries.GetLatestChangelog;

namespace GameOn.Application.Common.Home.Queries.GetHomeData
{
    using GameOn.Application.FIFA.Seasons.Queries.GetCurrentSeason;
    using GameOn.Application.FIFA.Tournaments.Queries.GetFeaturedTournaments;
    using GameOn.Common.DTOs.Common;
    using GameOn.Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetHomeDataQueryHandler class.
    /// </summary>
    public class GetHomeDataQueryHandler : IRequestHandler<GetHomeDataQuery, HomeDataDto>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetHomeDataQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public GetHomeDataQueryHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<HomeDataDto> Handle(GetHomeDataQuery request, CancellationToken cancellationToken)
        {
            var homeData = new HomeDataDto
            {
                LatestChangelog = await this.mediator.Send(new GetLatestChangelogQuery(), cancellationToken),
            };

            var currentSeason = await this.mediator.Send(new GetCurrentSeasonQuery(), cancellationToken);
            if (currentSeason is not null)
            {
                homeData.CurrentSeason = currentSeason;
            }

            var featuredTournaments = await this.mediator.Send(new GetFeaturedTournamentsQuery(), cancellationToken);
            homeData.FeaturedTournaments = featuredTournaments.ToList();

            return homeData;
        }
    }
}
