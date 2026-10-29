// <copyright file="UpdateConnectedPlayerCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Players.Commands.UpdateConnectedPlayer
{
    using GameOn.Application.Valorant.Players.Commands.UpdateValorantPlayer;
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
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateConnectedPlayerCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="accountService">Riot Games Account Service, injected.</param>
        /// <param name="summonerService">League of Legends Summoner Service, injected.</param>
        /// <param name="mediator">MediatR sender, injected.</param>
        public UpdateConnectedPlayerCommandHandler(IApplicationDbContext context, IAccountService accountService, ISummonerService summonerService, ISender mediator)
        {
            this.context = context;
            this.accountService = accountService;
            this.summonerService = summonerService;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<Player> Handle(UpdateConnectedPlayerCommand request, CancellationToken cancellationToken)
        {
            var playerInDb = await this.context.Players.FirstOrDefaultAsync(x => x.KeycloakId == request.Player.KeycloakId, cancellationToken);

            if (playerInDb == null)
            {
                throw new NotImplementedException();
            }

            playerInDb.FullName = request.Player.FullName;
            playerInDb.Nickname = request.Player.Nickname;

            var syncValorant = false;

            if (request.Player.RiotGamesNickname is not null && request.Player.RiotGamesTagLine is not null)
            {
                // First, checking if player isn't already present in DB
                var playersWithRiotCombo = await this.context.Players.FirstOrDefaultAsync(x => x.RiotGamesNickname == request.Player.RiotGamesNickname && x.RiotGamesTagLine == request.Player.RiotGamesTagLine, cancellationToken);

                if (playersWithRiotCombo is not null && playersWithRiotCombo.Id != playerInDb.Id)
                {
                    return playerInDb;
                }
                else
                {
                    // Calling Riot Games API to get PUUID
                    var puuidFromRiot = await this.accountService.GetAccountPuuid(request.Player.RiotGamesTagLine, request.Player.RiotGamesNickname, cancellationToken);

                    if (puuidFromRiot is not null)
                    {
                        // A different Riot account (a rename keeps the PUUID) means the stored Valorant PUUID
                        // belongs to the previous one: dropped so the Valorant refresh resolves the new Riot ID
                        // instead of refreshing the old account and writing its name back over this one.
                        if (playerInDb.RiotGamesPUUID != puuidFromRiot.Puuid)
                        {
                            playerInDb.ValorantPuuid = null;
                        }

                        // Only when the Valorant account still has to be found: a plain profile edit must not
                        // spend the shared HenrikDev quota, the background job keeps known accounts fresh.
                        syncValorant = playerInDb.ValorantPuuid is null;

                        playerInDb.RiotGamesNickname = puuidFromRiot.GameName;
                        playerInDb.RiotGamesTagLine = puuidFromRiot.TagLine;
                        playerInDb.RiotGamesPUUID = puuidFromRiot.Puuid;

                        // Now, getting its league summoners ID
                        var summonerIdFromRiot = await this.summonerService.GetSummonerByPuuid(puuidFromRiot.Puuid, cancellationToken);

                        if (summonerIdFromRiot is not null)
                        {
                            playerInDb.LolSummonerLevel = summonerIdFromRiot.SummonerLevel;
                            playerInDb.LolIconId = summonerIdFromRiot.ProfileIconId;
                            playerInDb.LolRefreshedOn = DateTime.UtcNow;
                        }
                    }
                }
            }

            this.context.Players.Update(playerInDb);
            await this.context.SaveChangesAsync(cancellationToken);

            // Without this the Valorant account is never found: the background refresh only covers crew
            // members, and a newly registered account starts outside the crew. Same tracked entity, so the
            // returned player carries the Valorant fields. No Valorant account (HenrikDev 404) is a no-op.
            if (syncValorant)
            {
                await this.mediator.Send(new UpdateValorantPlayerCommand { Player = playerInDb }, cancellationToken);
            }

            return playerInDb;
        }
    }
}
