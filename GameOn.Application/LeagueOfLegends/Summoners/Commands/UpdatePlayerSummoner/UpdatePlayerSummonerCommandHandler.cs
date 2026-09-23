// <copyright file="UpdatePlayerSummonerCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Commands.UpdatePlayerSummoner
{
    using GameOn.Application.LeagueOfLegends.Matches.Commands.ImportLoLGames;
    using GameOn.Application.LeagueOfLegends.Matches.Commands.RecomputeLoLGameRankChanges;
    using GameOn.Common.Interfaces;
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
        private readonly IMatchService matchService;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdatePlayerSummonerCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="summonerService">League of Legends Summoner Service, injected.</param>
        /// <param name="leagueService">League of Legends League Service, injected.</param>
        /// <param name="matchService">Match Service, injected.</param>
        /// <param name="mediator">Mediator interface, injected.</param>
        public UpdatePlayerSummonerCommandHandler(
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
        public async Task<Player> Handle(UpdatePlayerSummonerCommand request, CancellationToken cancellationToken)
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

            // Getting its league summoners ID
#pragma warning disable CS8604 // Possible null reference argument.
            var summonerIdFromRiot = await this.summonerService.GetSummonerByPuuid(playerInDb.RiotGamesPUUID, cancellationToken);
#pragma warning restore CS8604 // Possible null reference argument.

            if (summonerIdFromRiot is not null)
            {
                playerInDb.LolSummonerLevel = summonerIdFromRiot.SummonerLevel;
                playerInDb.LolIconId = summonerIdFromRiot.ProfileIconId;
                playerInDb.LolRefreshedOn = DateTime.UtcNow;

                // Updating player Rank
                var playerRank = await this.leagueService.GetLeagueEntries(playerInDb.RiotGamesPUUID, cancellationToken);

                if (playerRank is not null)
                {
                    foreach (var entry in playerRank)
                    {
                        var lastEntry = await this.context.LeagueOfLegendsRankHistory
                            .Where(x => x.PlayerId == playerInDb.Id && x.QueueType == entry.QueueType)
                            .OrderByDescending(x => x.CreatedOn)
                            .FirstOrDefaultAsync(cancellationToken);

                        // The win/loss counters are compared too, not just the rank: a game that leaves the
                        // LP where it was (a loss at 0 LP under demotion protection) must still get its own
                        // snapshot, or it merges with the next game and neither can have its LP attributed
                        // (see LoLGameRankChangeCalculator).
                        if (lastEntry is not null
                            && lastEntry.Tier == entry.Tier
                            && lastEntry.Rank == entry.Rank
                            && lastEntry.LeaguePoints == entry.LeaguePoints
                            && lastEntry.Wins == entry.Wins
                            && lastEntry.Losses == entry.Losses)
                        {
                            continue;
                        }

                        var playRank = new LeagueOfLegendsRankHistory
                        {
                            CreatedOn = DateTime.UtcNow,
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

                // Now that we have rank history, let's get thoses last games
                var gamesFromRiot = await this.matchService.GetLastGamesPlayed(playerInDb.RiotGamesPUUID, cancellationToken);

                if (gamesFromRiot is not null && gamesFromRiot.Count() > 0)
                {
                    // For each games found, importing them
                    await this.mediator.Send(new ImportLoLGamesCommand { MatchIDs = gamesFromRiot.ToList(), Player = playerInDb });
                }
            }

            // After the import, so that the games it just brought in are taken into account.
            await this.SyncRiotIdFromLatestGame(playerInDb, cancellationToken);

            this.context.Players.Update(playerInDb);
            await this.context.SaveChangesAsync(cancellationToken);

            // Run after both the snapshot and the games are saved, since either can be the missing half of
            // a game's LP: the snapshot of a game already imported through a duo partner's refresh, or a
            // game match-v5 only serves a refresh after league-v4 moved. A week back covers any such lag.
            await this.mediator.Send(
                new RecomputeLoLGameRankChangesCommand { PlayerId = playerInDb.Id, Since = DateTime.UtcNow.AddDays(-7) },
                cancellationToken);

            return playerInDb;
        }

        /// <summary>
        /// Brings the player's Riot ID in line with the one its most recent game was played under.
        /// </summary>
        /// <remarks>
        /// A Riot ID can be renamed at will while the PUUID stays the same, so nothing else in a refresh
        /// ever notices a rename, and summoner-v4 no longer returns any name. Every imported game, however,
        /// records the Riot ID each participant played under, which makes the latest one the freshest name
        /// we hold without an extra account-v1 call. A rename therefore only shows up once a game has been
        /// played under the new name.
        /// </remarks>
        /// <param name="playerInDb">Player being refreshed, updated in place.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns><see cref="Task"/>.</returns>
        private async Task SyncRiotIdFromLatestGame(Player playerInDb, CancellationToken cancellationToken)
        {
            // Matched on the PUUID too, not just the player ID: a player re-linked to another Riot account
            // keeps the games of the previous one, whose name must not come back.
            var participations = this.context.LeagueOfLegendsGameParticipants
                .Where(x => x.PlayerId == playerInDb.Id && x.Puuid == playerInDb.RiotGamesPUUID);

            var latestRiotId = await participations
                .Where(x => x.RiotIdGameName != string.Empty && x.RiotIdTagLine != string.Empty)
                .OrderByDescending(x => x.Game.GameStart)
                .Select(x => new { x.RiotIdGameName, x.RiotIdTagLine })
                .FirstOrDefaultAsync(cancellationToken);

            if (latestRiotId is null
                || (latestRiotId.RiotIdGameName == playerInDb.RiotGamesNickname && latestRiotId.RiotIdTagLine == playerInDb.RiotGamesTagLine))
            {
                return;
            }

            // Only a Riot ID the account has already been seen under in some game is known to be stale. One
            // that no game shows was set by hand (profile update, admin route, smurf link — the last two
            // refresh right after writing it) for a rename not played under yet: it is newer than anything
            // the games know, and overwriting it would undo that request. Under SQL Server's default
            // case-insensitive collation, the lookup also lets a mere casing difference be corrected.
            var isKnownStale = string.IsNullOrEmpty(playerInDb.RiotGamesNickname)
                || string.IsNullOrEmpty(playerInDb.RiotGamesTagLine)
                || await participations.AnyAsync(
                    x => x.RiotIdGameName == playerInDb.RiotGamesNickname && x.RiotIdTagLine == playerInDb.RiotGamesTagLine,
                    cancellationToken);

            if (isKnownStale)
            {
                playerInDb.RiotGamesNickname = latestRiotId.RiotIdGameName;
                playerInDb.RiotGamesTagLine = latestRiotId.RiotIdTagLine;
            }
        }
    }
}
