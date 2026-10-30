// <copyright file="LinkSmurfAccountCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Commands.LinkSmurfAccount
{
    using GameOn.Application.Common.Players;
    using GameOn.Application.LeagueOfLegends.Summoners.Commands.UpdatePlayerSummoner;
    using GameOn.Common.DTOs.Common;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using GameOn.External.RiotGames.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// LinkSmurfAccountCommandHandler class.
    /// </summary>
    public class LinkSmurfAccountCommandHandler : IRequestHandler<LinkSmurfAccountCommand, LinkSmurfAccountResultDto>
    {
        private readonly IApplicationDbContext context;
        private readonly IAccountService accountService;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkSmurfAccountCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="accountService">League of Legends Account Service, injected.</param>
        /// <param name="mediator">Mediator interface, injected.</param>
        public LinkSmurfAccountCommandHandler(IApplicationDbContext context, IAccountService accountService, ISender mediator)
        {
            this.context = context;
            this.accountService = accountService;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<LinkSmurfAccountResultDto> Handle(LinkSmurfAccountCommand request, CancellationToken cancellationToken)
        {
            var primaryPlayer = await this.context.Players.FirstOrDefaultAsync(x => x.Id == request.PrimaryPlayerId, cancellationToken);

            if (primaryPlayer is null)
            {
                return new LinkSmurfAccountResultDto { Status = LinkSmurfAccountStatus.PrimaryNotFound };
            }

            if (!SmurfAccountRules.CanHoldSmurfs(primaryPlayer))
            {
                return new LinkSmurfAccountResultDto { Status = LinkSmurfAccountStatus.PrimaryIsSmurf };
            }

            var accountFromRiot = await this.accountService.GetAccountPuuid(request.RiotGamesTagLine, request.RiotGamesNickname, cancellationToken);

            if (accountFromRiot is null || string.IsNullOrEmpty(accountFromRiot.Puuid))
            {
                return new LinkSmurfAccountResultDto { Status = LinkSmurfAccountStatus.RiotAccountNotFound };
            }

            var existingAccount = await this.context.Players.FirstOrDefaultAsync(x => x.RiotGamesPUUID == accountFromRiot.Puuid, cancellationToken);
            Player smurfAccount;

            if (existingAccount is null)
            {
                // Unknown Riot account: it gets its own player row, holding the Riot identity, the rank
                // history and the participations of that account. KeycloakId stays null, a smurf is not
                // a login.
                smurfAccount = new Player
                {
                    Nickname = accountFromRiot.GameName ?? request.RiotGamesNickname,
                    RiotGamesNickname = accountFromRiot.GameName,
                    RiotGamesTagLine = accountFromRiot.TagLine,
                    RiotGamesPUUID = accountFromRiot.Puuid,
                    PrimaryPlayerId = primaryPlayer.Id,
                    CreatedOn = DateTime.UtcNow,
                };

                this.context.Players.Add(smurfAccount);
            }
            else
            {
                if (!await SmurfAccountRules.IsAvailableAsSmurf(this.context, existingAccount, primaryPlayer, cancellationToken))
                {
                    return new LinkSmurfAccountResultDto { Status = LinkSmurfAccountStatus.AccountNotAvailable };
                }

                existingAccount.PrimaryPlayerId = primaryPlayer.Id;
                this.context.Players.Update(existingAccount);
                smurfAccount = existingAccount;
            }

            await this.context.SaveChangesAsync(cancellationToken);

            // Games imported before the link stored this PUUID with no player attached (see
            // UpdateLoLGameCommandHandler, which resolves participants against known PUUIDs at import
            // time). Without this backfill the account's whole past history stays invisible.
            var backfilled = await this.context.LeagueOfLegendsGameParticipants
                .Where(x => x.Puuid == accountFromRiot.Puuid && x.PlayerId == null)
                .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.PlayerId, (int?)smurfAccount.Id), cancellationToken);

            // Pulls the account's summoner level, icon, current rank and recent games from Riot.
            await this.mediator.Send(new UpdatePlayerSummonerCommand { Player = smurfAccount }, cancellationToken);

            return new LinkSmurfAccountResultDto
            {
                Status = LinkSmurfAccountStatus.Linked,
                SmurfAccount = smurfAccount,
                BackfilledParticipations = backfilled,
            };
        }
    }
}
