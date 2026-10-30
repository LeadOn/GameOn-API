// <copyright file="SetPlayerArchivedCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Players.Commands.SetPlayerArchived
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// SetPlayerArchivedCommandHandler class.
    /// </summary>
    public class SetPlayerArchivedCommandHandler : IRequestHandler<SetPlayerArchivedCommand, Player?>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetPlayerArchivedCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public SetPlayerArchivedCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<Player?> Handle(SetPlayerArchivedCommand request, CancellationToken cancellationToken)
        {
            var playerInDb = await this.context.Players.FirstOrDefaultAsync(x => x.Id == request.PlayerId, cancellationToken);

            if (playerInDb is null)
            {
                return null;
            }

            playerInDb.Archived = request.Archived;
            await this.context.SaveChangesAsync(cancellationToken);

            return playerInDb;
        }
    }
}
