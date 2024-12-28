// <copyright file="UpdateConnectedPlayerCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Players.Commands.UpdateConnectedPlayer
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using GameOn.External.RiotGames.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// UpdateConnectedPlayerCommandHandler class.
    /// </summary>
    public class UpdateConnectedPlayerCommandHandler : IRequestHandler<UpdateConnectedPlayerCommand, Player>
    {
        private readonly IApplicationDbContext context;
        private readonly IAccountService accountService;
        private readonly ISummonerService summonerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateConnectedPlayerCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="accountService">Riot Games Account Service, injected.</param>
        /// <param name="summonerService">League of Legends Summoner Service, injected.</param>
        public UpdateConnectedPlayerCommandHandler(IApplicationDbContext context, IAccountService accountService, ISummonerService summonerService)
        {
            this.context = context;
            this.accountService = accountService;
            this.summonerService = summonerService;
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

            if (request.Player.RiotGamesNickname is not null && request.Player.RiotGamesTagLine is not null)
            {
                // First, checking if player isn't already present in DB
                var playersWithRiotCombo = await this.context.Players.FirstOrDefaultAsync(x => x.RiotGamesNickname == request.Player.RiotGamesNickname && x.RiotGamesTagLine == request.Player.RiotGamesTagLine, cancellationToken);

                if (playersWithRiotCombo is not null)
                {
                    return playerInDb;
                }
                else
                {
                    // Calling Riot Games API to get PUUID
                    var puuidFromRiot = await this.accountService.GetAccountPuuid(request.Player.RiotGamesTagLine, request.Player.RiotGamesNickname, cancellationToken);

                    if (puuidFromRiot is not null)
                    {
                        playerInDb.RiotGamesNickname = puuidFromRiot.GameName;
                        playerInDb.RiotGamesTagLine = puuidFromRiot.TagLine;
                        playerInDb.RiotGamesPUUID = puuidFromRiot.Puuid;

                        // Now, getting its league summoners ID
                        var summonerIdFromRiot = await this.summonerService.GetSummonerByPuuid(puuidFromRiot.Puuid, cancellationToken);

                        if (summonerIdFromRiot is not null)
                        {
                            playerInDb.LolSummonerId = summonerIdFromRiot.SummonerId;
                        }
                    }
                }
            }

            this.context.Players.Update(playerInDb);
            await this.context.SaveChangesAsync(cancellationToken);
            return playerInDb;
        }
    }
}
