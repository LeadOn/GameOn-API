// <copyright file="UpdatePlayerSummonerAdminCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Commands.UpdatePlayerSummoner
{
    using GameOn.Application.LeagueOfLegends.Matches.Commands.ImportLoLGames;
    using GameOn.Application.LeagueOfLegends.Summoners.Commands.UpdatePlayerSummonerAdmin;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using GameOn.External.RiotGames.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// UpdatePlayerSummonerAdminCommandHandler class.
    /// </summary>
    public class UpdatePlayerSummonerAdminCommandHandler : IRequestHandler<UpdatePlayerSummonerAdminCommand, Player>
    {
        private readonly IApplicationDbContext context;
        private readonly ISummonerService summonerService;
        private readonly IAccountService accountService;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdatePlayerSummonerAdminCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="summonerService">League of Legends Summoner Service, injected.</param>
        /// <param name="accountService">League of Legends Account Service, injected.</param>
        /// <param name="mediator">Mediator interface, injected.</param>
        public UpdatePlayerSummonerAdminCommandHandler(
            IApplicationDbContext context,
            ISummonerService summonerService,
            IAccountService accountService,
            ISender mediator)
        {
            this.context = context;
            this.summonerService = summonerService;
            this.accountService = accountService;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<Player> Handle(UpdatePlayerSummonerAdminCommand request, CancellationToken cancellationToken)
        {
            var playerInDb = await this.context.Players.FirstOrDefaultAsync(x => x.KeycloakId == request.Player.KeycloakId);

            if (playerInDb == null)
            {
                throw new NotImplementedException();
            }

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

                        // // Now, getting its league summoners ID
                        // var summonerIdFromRiot = await this.summonerService.GetSummonerByPuuid(puuidFromRiot.Puuid, cancellationToken);

                        // if (summonerIdFromRiot is not null)
                        // {
                        //     playerInDb.LolSummonerId = summonerIdFromRiot.SummonerId;
                        // }
                    }
                }
            }

            await this.mediator.Send(new UpdatePlayerSummonerCommand { Player = playerInDb }, cancellationToken);

            return playerInDb;
        }
    }
}
