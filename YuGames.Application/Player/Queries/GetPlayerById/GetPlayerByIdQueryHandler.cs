// <copyright file="GetPlayerByIdQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Player.Queries.GetPlayerById
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using YuGames.Application.Common.Interfaces;
    using YuGames.Domain;

    /// <summary>
    /// GetPlayerByIdQueryHandler class.
    /// </summary>
    public class GetPlayerByIdQueryHandler : IRequestHandler<GetPlayerByIdQuery, Player?>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPlayerByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetPlayerByIdQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<Player?> Handle(GetPlayerByIdQuery request, CancellationToken cancellationToken)
        {
            return await this.context.Players.FirstOrDefaultAsync(x => x.Id == request.PlayerId);
        }
    }
}
