// <copyright file="GetPlayerByKeycloakIdQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Players.Queries.GetPlayerByKeycloakId
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using YuGames.Application.Common.Interfaces;
    using YuGames.Domain;

    /// <summary>
    /// GetPlayerByKeycloakIdQueryHandler class.
    /// </summary>
    public class GetPlayerByKeycloakIdQueryHandler : IRequestHandler<GetPlayerByKeycloakIdQuery, Player?>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPlayerByKeycloakIdQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetPlayerByKeycloakIdQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<Player?> Handle(GetPlayerByKeycloakIdQuery request, CancellationToken cancellationToken)
        {
            return await this.context.Players.FirstOrDefaultAsync(x => x.KeycloakId == request.KeycloakId, cancellationToken);
        }
    }
}
