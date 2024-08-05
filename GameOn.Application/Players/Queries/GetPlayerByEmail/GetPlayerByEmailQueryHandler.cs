// <copyright file="GetPlayerByEmailQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Players.Queries.GetPlayerByEmail
{
    using GameOn.Application.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetPlayerByEmailQueryHandler class.
    /// </summary>
    public class GetPlayerByEmailQueryHandler : IRequestHandler<GetPlayerByEmailQuery, Player?>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPlayerByEmailQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetPlayerByEmailQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<Player?> Handle(GetPlayerByEmailQuery request, CancellationToken cancellationToken)
        {
            return await this.context.Players.FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);
        }
    }
}
