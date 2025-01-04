// <copyright file="GetPlayerByKeycloakIdQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Players.Queries.GetPlayerByKeycloakId
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

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
