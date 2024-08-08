// <copyright file="GetAllPlayersQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Players.Queries.GetAllPlayers
{
    using GameOn.Application.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetAllPlayersQueryHandler class.
    /// </summary>
    public class GetAllPlayersQueryHandler : IRequestHandler<GetAllPlayersQuery, IEnumerable<Player>>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllPlayersQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetAllPlayersQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Player>> Handle(GetAllPlayersQuery request, CancellationToken cancellationToken)
        {
            return await this.context.Players.Where(x => x.Archived == request.Archived).ToListAsync(cancellationToken);
        }
    }
}
