﻿// <copyright file="UpdatePlayerSummonerCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Commands.UpdatePlayerSummoner
{
    using GameOn.Application.Common.Interfaces;
    using GameOn.Domain;
    using GameOn.External.RiotGames.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// UpdatePlayerSummonerCommandHandler class.
    /// </summary>
    public class UpdatePlayerSummonerCommandHandler : IRequestHandler<UpdatePlayerSummonerCommand, Player>
    {
        private readonly IApplicationDbContext context;
        private readonly ISummonerService summonerService;
        private readonly ILeagueService leagueService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdatePlayerSummonerCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="summonerService">League of Legends Summoner Service, injected.</param>
        /// <param name="leagueService">League of Legends League Service, injected.</param>
        public UpdatePlayerSummonerCommandHandler(IApplicationDbContext context, ISummonerService summonerService, ILeagueService leagueService)
        {
            this.context = context;
            this.summonerService = summonerService;
            this.leagueService = leagueService;
        }

        /// <inheritdoc />
        public async Task<Player> Handle(UpdatePlayerSummonerCommand request, CancellationToken cancellationToken)
        {
            var playerInDb = await this.context.Players.FirstOrDefaultAsync(x => x.KeycloakId == request.Player.KeycloakId);

            if (playerInDb == null)
            {
                throw new NotImplementedException();
            }

            // Getting its league summoners ID
#pragma warning disable CS8604 // Existence possible d'un argument de référence null.
            var summonerIdFromRiot = await this.summonerService.GetSummonerByPuuid(playerInDb.RiotGamesPUUID);
#pragma warning restore CS8604 // Existence possible d'un argument de référence null.

            if (summonerIdFromRiot is not null)
            {
                playerInDb.LolSummonerId = summonerIdFromRiot.SummonerId;
                playerInDb.LolSummonerLevel = summonerIdFromRiot.SummonerLevel;
                playerInDb.LolRefreshedOn = DateTime.Now;

                // Updating player Rank
                var playerRank = await this.leagueService.GetLeagueEntries(summonerIdFromRiot.SummonerId);

                if (playerRank is not null)
                {
                    foreach (var entry in playerRank)
                    {
                        var playRank = new LeagueOfLegendsRankHistory
                        {
                            CreatedOn = DateTime.Now,
                            FreshBlood = entry.FreshBlood,
                            HotStreak = entry.HotStreak,
                            Inactive = entry.Inactive,
                            LeaguePoints = entry.LeaguePoints,
                            Losses = entry.Losses,
                            QueueType = entry.QueueType,
                            PlayerId = playerInDb.Id,
                            Rank = entry.Rank,
                            Tier = entry.Tier,
                            Veteran = entry.Veteran,
                            Wins = entry.Wins,
                        };

                        this.context.LeagueOfLegendsRankHistory.Add(playRank);
                    }
                }
            }

            this.context.Players.Update(playerInDb);
            await this.context.SaveChangesAsync(cancellationToken);
            return playerInDb;
        }
    }
}
