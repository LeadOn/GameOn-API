// <copyright file="GetLoLLiveGamesQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Live.Queries.GetLoLLiveGames
{
    using GameOn.Application.Common.Players;
    using GameOn.Application.LeagueOfLegends.Live.Services;
    using GameOn.Common.DTOs;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Common.Interfaces;
    using GameOn.External.CommunityDragon.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetLoLLiveGamesQueryHandler class.
    /// </summary>
    public class GetLoLLiveGamesQueryHandler : IRequestHandler<GetLoLLiveGamesQuery, List<LoLLiveGameDto>>
    {
        private readonly IApplicationDbContext context;
        private readonly ILoLLiveGameCache liveGameCache;
        private readonly ICommunityDragonChampionService championService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLoLLiveGamesQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="liveGameCache">Spectator-v5 answers cache, injected.</param>
        /// <param name="championService">Champion referential, injected.</param>
        public GetLoLLiveGamesQueryHandler(IApplicationDbContext context, ILoLLiveGameCache liveGameCache, ICommunityDragonChampionService championService)
        {
            this.context = context;
            this.liveGameCache = liveGameCache;
            this.championService = championService;
        }

        /// <inheritdoc />
        public async Task<List<LoLLiveGameDto>> Handle(GetLoLLiveGamesQuery request, CancellationToken cancellationToken)
        {
            // The same roster as the home page and the player list: crew accounts unless asked otherwise,
            // smurfs included unless asked otherwise, archived accounts never (they're listed nowhere).
            var playersQuery = this.context.Players
                .Where(x => !x.Archived && x.RiotGamesPUUID != null && x.RiotGamesPUUID != string.Empty);

            if (!request.IncludeOutOfCrew)
            {
                playersQuery = playersQuery.InCrewOnly();
            }

            if (!request.IncludeSmurfs)
            {
                playersQuery = playersQuery.PrimariesOnly();
            }

            var players = await playersQuery.ToListAsync(cancellationToken);

            var snapshots = await this.liveGameCache.GetActiveGames(
                players.Select(x => x.RiotGamesPUUID!).ToList(),
                cancellationToken);

            // Spectator-v5 answers per account, and the answer lists all ten players: the account's own
            // champion and side are read off its own row in that list.
            var liveGames = players
                .Select(player => new { Player = player, Snapshot = snapshots.GetValueOrDefault(player.RiotGamesPUUID!) })
                .Where(x => x.Snapshot?.Game is not null)
                .Select(x => new
                {
                    x.Player,
                    Game = x.Snapshot!.Game!,
                    x.Snapshot.RetrievedOn,
                    Participant = x.Snapshot.Game!.Participants.FirstOrDefault(participant => participant.Puuid == x.Player.RiotGamesPUUID),
                })
                .Where(x => x.Participant is not null)
                .ToList();

            if (liveGames.Count == 0)
            {
                return new List<LoLLiveGameDto>();
            }

            // Spectator-v5 only gives the champion ID. Community Dragon's alias is the name match-v5 uses
            // (MonkeyKing), the one LoLGameParticipant.ChampionName holds, already cached for the LCU import.
            var championNames = await this.championService.GetChampionNamesById(cancellationToken);

            // Longest running game first, games still on the loading screen last; entries of the same game
            // stay next to each other, split by side, so the front can group them without re-sorting.
            return liveGames
                .Select(x => new LoLLiveGameDto
                {
                    Player = new PlayerDto(x.Player),
                    GameId = x.Game.GameId,
                    ChampionId = (int)x.Participant!.ChampionId,
                    ChampionName = championNames.GetValueOrDefault((int)x.Participant.ChampionId),
                    QueueId = x.Game.GameQueueConfigId is > 0 ? (int)x.Game.GameQueueConfigId.Value : null,
                    TeamId = (int)x.Participant.TeamId,
                    GameStart = x.Game.GameStartTime > 0 ? DateTime.UnixEpoch.AddMilliseconds(x.Game.GameStartTime) : null,
                    GameLengthSeconds = Math.Max(0, x.Game.GameLength),
                    RetrievedOn = x.RetrievedOn,
                })
                .OrderBy(x => x.GameStart ?? DateTime.MaxValue)
                .ThenBy(x => x.GameId)
                .ThenBy(x => x.TeamId)
                .ThenBy(x => x.Player.Id)
                .ToList();
        }
    }
}
