// <copyright file="UpdateValorantPlayerCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Players.Commands.UpdateValorantPlayer
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Common.DTOs;
    using GameOn.Common.Interfaces;
    using GameOn.External.HenrikDev.Interfaces;
    using GameOn.External.HenrikDev.Models.DTOs;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// UpdateValorantPlayerCommandHandler class.
    /// </summary>
    public class UpdateValorantPlayerCommandHandler : IRequestHandler<UpdateValorantPlayerCommand, PlayerDto>
    {
        private readonly IApplicationDbContext context;
        private readonly IValorantAccountService valorantAccountService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateValorantPlayerCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="valorantAccountService">Valorant Account Service, injected.</param>
        public UpdateValorantPlayerCommandHandler(
            IApplicationDbContext context,
            IValorantAccountService valorantAccountService)
        {
            this.context = context;
            this.valorantAccountService = valorantAccountService;
        }

        /// <inheritdoc />
        public async Task<PlayerDto> Handle(UpdateValorantPlayerCommand request, CancellationToken cancellationToken)
        {
            // Resolved by ID whenever the caller has one, and only otherwise by Keycloak ID: smurf
            // accounts have no Keycloak identity, and a null-matching lookup would happily refresh some
            // unrelated player that also has none.
            var playerInDb = request.Player.Id != 0
                ? await this.context.Players.FirstOrDefaultAsync(x => x.Id == request.Player.Id, cancellationToken)
                : await this.context.Players.FirstOrDefaultAsync(x => x.KeycloakId != null && x.KeycloakId == request.Player.KeycloakId, cancellationToken);

            if (playerInDb == null)
            {
                throw new NotImplementedException();
            }

            ValorantAccountDto? valorantAccount = null;

            if (!string.IsNullOrWhiteSpace(playerInDb.ValorantPuuid))
            {
                valorantAccount = await this.valorantAccountService.GetAccountByPuuid(playerInDb.ValorantPuuid, cancellationToken);
            }
            else if (!string.IsNullOrWhiteSpace(playerInDb.RiotGamesNickname) && !string.IsNullOrWhiteSpace(playerInDb.RiotGamesTagLine))
            {
                valorantAccount = await this.valorantAccountService.GetAccountByRiotId(playerInDb.RiotGamesNickname, playerInDb.RiotGamesTagLine, cancellationToken);
            }

            if (valorantAccount is not null)
            {
                playerInDb.ValorantPuuid = valorantAccount.Puuid;
                playerInDb.ValorantAccountLevel = valorantAccount.AccountLevel;
                playerInDb.ValorantPlayerCardId = valorantAccount.Card;
                playerInDb.ValorantPlayerTitleId = valorantAccount.Title;
                playerInDb.ValorantRegion = valorantAccount.Region;
                playerInDb.ValorantRefreshedOn = DateTime.UtcNow;

                if (!string.IsNullOrWhiteSpace(valorantAccount.Name) && !string.IsNullOrWhiteSpace(valorantAccount.Tag))
                {
                    playerInDb.RiotGamesNickname = valorantAccount.Name;
                    playerInDb.RiotGamesTagLine = valorantAccount.Tag;
                }
            }

            this.context.Players.Update(playerInDb);
            await this.context.SaveChangesAsync(cancellationToken);

            return new PlayerDto(playerInDb);
        }
    }
}