// <copyright file="LinkValorantSmurfAccountCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Players.Commands.LinkValorantSmurfAccount
{
    using GameOn.Application.Common.Players;
    using GameOn.Application.Valorant.Players.Commands.UpdateValorantPlayer;
    using GameOn.Common.DTOs.Common;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using GameOn.External.HenrikDev.Interfaces;
    using GameOn.External.RiotGames.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// LinkValorantSmurfAccountCommandHandler class.
    /// </summary>
    public class LinkValorantSmurfAccountCommandHandler : IRequestHandler<LinkValorantSmurfAccountCommand, LinkSmurfAccountResultDto>
    {
        private readonly IApplicationDbContext context;
        private readonly IValorantAccountService valorantAccountService;
        private readonly IAccountService accountService;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkValorantSmurfAccountCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="valorantAccountService">HenrikDev Valorant Account Service, injected.</param>
        /// <param name="accountService">Riot Games Account Service, injected.</param>
        /// <param name="mediator">Mediator interface, injected.</param>
        public LinkValorantSmurfAccountCommandHandler(IApplicationDbContext context, IValorantAccountService valorantAccountService, IAccountService accountService, ISender mediator)
        {
            this.context = context;
            this.valorantAccountService = valorantAccountService;
            this.accountService = accountService;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<LinkSmurfAccountResultDto> Handle(LinkValorantSmurfAccountCommand request, CancellationToken cancellationToken)
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

            var valorantAccount = await this.valorantAccountService.GetAccountByRiotId(request.RiotGamesNickname, request.RiotGamesTagLine, cancellationToken);

            if (valorantAccount is null || string.IsNullOrEmpty(valorantAccount.Puuid))
            {
                return new LinkSmurfAccountResultDto { Status = LinkSmurfAccountStatus.RiotAccountNotFound };
            }

            var gameName = string.IsNullOrWhiteSpace(valorantAccount.Name) ? request.RiotGamesNickname : valorantAccount.Name;
            var tagLine = string.IsNullOrWhiteSpace(valorantAccount.Tag) ? request.RiotGamesTagLine : valorantAccount.Tag;

            // The HenrikDev PUUID is the raw Valorant one, the stored RiotGamesPUUID is the one Riot encrypts
            // for our API key: the two never compare equal, so the account is looked up on both. The Riot
            // one also fills a new row, which then works for League of Legends as well.
            var riotAccount = await this.accountService.GetAccountPuuid(tagLine, gameName, cancellationToken);
            var riotPuuid = riotAccount?.Puuid;

            var existingAccount = await this.context.Players.FirstOrDefaultAsync(x => x.ValorantPuuid == valorantAccount.Puuid, cancellationToken);

            if (existingAccount is null && !string.IsNullOrEmpty(riotPuuid))
            {
                existingAccount = await this.context.Players.FirstOrDefaultAsync(x => x.RiotGamesPUUID == riotPuuid, cancellationToken);
            }

            // Last resort on the Riot ID itself: a PUUID stored under another API key never matches, and
            // creating a second row for the same Riot ID would make it "taken" for everyone afterwards.
            existingAccount ??= await this.context.Players.FirstOrDefaultAsync(x => x.RiotGamesNickname == gameName && x.RiotGamesTagLine == tagLine, cancellationToken);

            Player smurfAccount;

            if (existingAccount is null)
            {
                // Same as the League of Legends link: KeycloakId stays null (a smurf is not a login), and
                // InCrew keeps its false default, an admin decides whether the account counts.
                smurfAccount = new Player
                {
                    Nickname = gameName,
                    RiotGamesNickname = gameName,
                    RiotGamesTagLine = tagLine,
                    RiotGamesPUUID = riotPuuid,
                    ValorantPuuid = valorantAccount.Puuid,
                    ValorantRegion = valorantAccount.Region,
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
                existingAccount.ValorantPuuid = valorantAccount.Puuid;
                this.context.Players.Update(existingAccount);
                smurfAccount = existingAccount;
            }

            await this.context.SaveChangesAsync(cancellationToken);

            // Games imported before the link stored this PUUID with no player attached (participants are
            // resolved against known PUUIDs at import time): without this the account's past history
            // stays invisible.
            var backfilled = await this.context.ValorantGameParticipants
                .Where(x => x.Puuid == valorantAccount.Puuid && x.PlayerId == null)
                .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.PlayerId, (int?)smurfAccount.Id), cancellationToken);

            // Pulls the account's card, level, rank history and recent games from HenrikDev.
            await this.mediator.Send(new UpdateValorantPlayerCommand { Player = smurfAccount }, cancellationToken);

            return new LinkSmurfAccountResultDto
            {
                Status = LinkSmurfAccountStatus.Linked,
                SmurfAccount = smurfAccount,
                BackfilledParticipations = backfilled,
            };
        }
    }
}
