// <copyright file="UpdateValorantPlayerAdminCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Players.Commands.UpdateValorantPlayerAdmin
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Application.Valorant.Players.Commands.UpdateValorantPlayer;
    using GameOn.Common.DTOs;
    using GameOn.Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// UpdateValorantPlayerAdminCommandHandler class.
    /// </summary>
    public class UpdateValorantPlayerAdminCommandHandler : IRequestHandler<UpdateValorantPlayerAdminCommand, PlayerDto>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateValorantPlayerAdminCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">Mediator interface, injected.</param>
        public UpdateValorantPlayerAdminCommandHandler(
            IApplicationDbContext context,
            ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<PlayerDto> Handle(UpdateValorantPlayerAdminCommand request, CancellationToken cancellationToken)
        {
            var playerInDb = request.Player.Id != 0
                ? await this.context.Players.FirstOrDefaultAsync(x => x.Id == request.Player.Id, cancellationToken)
                : await this.context.Players.FirstOrDefaultAsync(x => x.KeycloakId != null && x.KeycloakId == request.Player.KeycloakId, cancellationToken);

            if (playerInDb == null)
            {
                throw new NotImplementedException();
            }

            if (!string.IsNullOrWhiteSpace(request.Player.RiotGamesNickname) && !string.IsNullOrWhiteSpace(request.Player.RiotGamesTagLine))
            {
                var existingPlayer = await this.context.Players.FirstOrDefaultAsync(
                    x => x.RiotGamesNickname == request.Player.RiotGamesNickname && x.RiotGamesTagLine == request.Player.RiotGamesTagLine,
                    cancellationToken);

                if (existingPlayer is not null && existingPlayer.Id != playerInDb.Id)
                {
                    return new PlayerDto(playerInDb);
                }

                playerInDb.RiotGamesNickname = request.Player.RiotGamesNickname;
                playerInDb.RiotGamesTagLine = request.Player.RiotGamesTagLine;
                playerInDb.ValorantPuuid = null;

                this.context.Players.Update(playerInDb);
                await this.context.SaveChangesAsync(cancellationToken);
            }

            return await this.mediator.Send(new UpdateValorantPlayerCommand { Player = playerInDb }, cancellationToken);
        }
    }
}