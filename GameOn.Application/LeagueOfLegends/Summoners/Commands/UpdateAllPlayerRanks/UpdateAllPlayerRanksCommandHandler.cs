// <copyright file="UpdateAllPlayerRanksCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Commands.UpdateAllPlayerRanks
{
    using GameOn.Application.LeagueOfLegends.Matches.Commands.ImportLoLGames;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using GameOn.External.RiotGames.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// UpdateAllPlayerRanksCommandHandler class.
    /// </summary>
    public class UpdateAllPlayerRanksCommandHandler : IRequestHandler<UpdateAllPlayerRanksCommand>
    {
        private readonly IApplicationDbContext context;
        private readonly ISummonerService summonerService;
        private readonly ILeagueService leagueService;
        private readonly IMatchService matchService;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateAllPlayerRanksCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="summonerService">League of Legends Summoner Service, injected.</param>
        /// <param name="leagueService">League of Legends League Service, injected.</param>
        /// <param name="matchService">Match Service, injected.</param>
        /// <param name="mediator">Mediator interface, injected.</param>
        public UpdateAllPlayerRanksCommandHandler(
            IApplicationDbContext context,
            ISummonerService summonerService,
            ILeagueService leagueService,
            IMatchService matchService,
            ISender mediator)
        {
            this.context = context;
            this.summonerService = summonerService;
            this.leagueService = leagueService;
            this.matchService = matchService;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task Handle(UpdateAllPlayerRanksCommand request, CancellationToken cancellationToken)
        {
            foreach (var playerInDb in await this.context.Players.Where(x => x.RiotGamesPUUID != null && x.RiotGamesPUUID != string.Empty).ToListAsync(cancellationToken))
            {
                // Getting its league summoners ID
    #pragma warning disable CS8604 // Existence possible d'un argument de référence null.
                var summonerIdFromRiot = await this.summonerService.GetSummonerByPuuid(playerInDb.RiotGamesPUUID, cancellationToken);
    #pragma warning restore CS8604 // Existence possible d'un argument de référence null.

                if (summonerIdFromRiot is not null)
                {
                    playerInDb.LolSummonerLevel = summonerIdFromRiot.SummonerLevel;
                    playerInDb.LolIconId = summonerIdFromRiot.ProfileIconId;
                    playerInDb.LolRefreshedOn = DateTime.Now;

                    // Updating player Rank
                    var playerRank = await this.leagueService.GetLeagueEntries(playerInDb.RiotGamesPUUID, cancellationToken);

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
            }
        }
    }
}
