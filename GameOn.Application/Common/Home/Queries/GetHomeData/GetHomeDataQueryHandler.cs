// <copyright file="GetHomeDataQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Home.Queries.GetHomeData
{
    using GameOn.Application.FIFA.Tournaments.Queries.GetFeaturedTournaments;
    using GameOn.Application.Seasons.Queries.GetCurrentSeason;
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
            var homeData = new HomeDataDto();

            var latestChangelog = await this.context.Changelogs.OrderByDescending(e => e.PublicationDate).FirstOrDefaultAsync();
            if (latestChangelog is not null)
            {
                homeData.LatestChangelog = latestChangelog;
            }

            var currentSeason = await this.mediator.Send(new GetCurrentSeasonQuery());
            if (currentSeason is not null)
            {
                homeData.CurrentSeason = currentSeason;
            }

            var featuredTournaments = await this.mediator.Send(new GetFeaturedTournamentsQuery());
            if (featuredTournaments is not null)
            {
                homeData.FeaturedTournaments = featuredTournaments.ToList();
            }

            return homeData;
        }
    }
}
