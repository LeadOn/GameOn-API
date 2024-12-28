// <copyright file="GetGameByIdQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Matches.Queries.GetGameById
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetGameByIdQueryHandler class.
    /// </summary>
    public class GetGameByIdQueryHandler : IRequestHandler<GetGameByIdQuery, LoLGame?>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetGameByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetGameByIdQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<LoLGame?> Handle(GetGameByIdQuery request, CancellationToken cancellationToken)
        {
            return await this.context.LeagueOfLegendsGames.FirstOrDefaultAsync(x => x.GameId == request.GameId);
        }
    }
}
