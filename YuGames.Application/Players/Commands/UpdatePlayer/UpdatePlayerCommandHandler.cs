// <copyright file="UpdatePlayerCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Players.Commands.UpdatePlayer
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using YuGames.Application.Common.Interfaces;
    using YuGames.Domain;

    /// <summary>
    /// UpdatePlayerCommandHandler class.
    /// </summary>
    public class UpdatePlayerCommandHandler : IRequestHandler<UpdatePlayerCommand, Player>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdatePlayerCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public UpdatePlayerCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<Player> Handle(UpdatePlayerCommand request, CancellationToken cancellationToken)
        {
            var playerInDb = await this.context.Players.FirstOrDefaultAsync(x => x.Id == request.Player.Id);

            if (playerInDb == null)
            {
                throw new NotImplementedException();
            }

            playerInDb.FullName = request.Player.FullName;
            playerInDb.Nickname = request.Player.Nickname;
            playerInDb.ProfilePictureUrl = request.Player.ProfilePictureUrl;
            playerInDb.KeycloakId = request.Player.KeycloakId;

            this.context.Players.Update(playerInDb);
            await this.context.SaveChangesAsync(cancellationToken);
            return playerInDb;
        }
    }
}
