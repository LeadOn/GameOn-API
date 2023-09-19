// <copyright file="GetCurrentSeasonQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Seasons.Queries.GetCurrentSeason
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using YuGames.Application.Common.Interfaces;
    using YuGames.Domain;

    /// <summary>
    /// GetCurrentSeasonQueryHandler class.
    /// </summary>
    public class GetCurrentSeasonQueryHandler : IRequestHandler<GetCurrentSeasonQuery, Season?>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCurrentSeasonQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetCurrentSeasonQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<Season?> Handle(GetCurrentSeasonQuery request, CancellationToken cancellationToken)
        {
            return await this.context.Seasons.FirstOrDefaultAsync(x => x.Id == int.Parse(Environment.GetEnvironmentVariable("CURRENT_SEASON") ?? "1"));
        }
    }
}
