// <copyright file="ImportCustomLoLGameCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Matches.Commands.ImportCustomLoLGame
{
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Common.DTOs.LeagueOfLegends.LeagueClient;
    using MediatR;

    /// <summary>
    /// ImportCustomLoLGameCommand class. Imports a custom game from a payload captured on the League
    /// client, the only source that has the data: custom games are absent from match-v5's match list
    /// and come back as an empty stub (or a 404) when asked for by ID.
    /// </summary>
    public class ImportCustomLoLGameCommand : IRequest<ImportCustomLoLGameResultDto>
    {
        /// <summary>
        /// Gets or sets the game, as served by <c>/lol-match-history/v1/games/{gameId}</c>.
        /// </summary>
        public LcuGameDto? Game { get; set; }

        /// <summary>
        /// Gets or sets the game's timeline, as served by <c>/lol-match-history/v1/game-timelines/{gameId}</c>.
        /// Optional: without it the game is imported with its end of game stats but no gold, level or
        /// kill history.
        /// </summary>
        public LcuTimelineDto? Timeline { get; set; }
    }
}
