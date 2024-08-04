// <copyright file="UpdateConnectedPlayerCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Players.Commands.UpdateConnectedPlayer
{
    using GameOn.Application.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// UpdateConnectedPlayerCommandHandler class.
    /// </summary>
    public class UpdateConnectedPlayerCommandHandler : IRequestHandler<UpdateConnectedPlayerCommand, Player>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateConnectedPlayerCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public UpdateConnectedPlayerCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<Player> Handle(UpdateConnectedPlayerCommand request, CancellationToken cancellationToken)
        {
            var playerInDb = await this.context.Players.FirstOrDefaultAsync(x => x.KeycloakId == request.Player.KeycloakId);

            if (playerInDb == null)
            {
                throw new NotImplementedException();
            }

            playerInDb.FullName = request.Player.FullName;
            playerInDb.Nickname = request.Player.Nickname;
            playerInDb.ProfilePictureUrl = request.Player.ProfilePictureUrl;

            this.context.Players.Update(playerInDb);
            await this.context.SaveChangesAsync(cancellationToken);
            return playerInDb;
        }
    }
}
