// <copyright file="GetLoLLiveGamesQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Live.Queries.GetLoLLiveGames
{
    using GameOn.Common.DTOs.LeagueOfLegends;
    using MediatR;

    /// <summary>
    /// GetLoLLiveGamesQuery class. The tracked accounts currently in a game.
    /// </summary>
    public class GetLoLLiveGamesQuery : IRequest<List<LoLLiveGameDto>>
    {
        /// <summary>
        /// Gets or sets a value indicating whether smurf accounts are looked up too. Defaults to true, like
        /// <c>GET lol/Home</c>: a game played on a smurf is being played all the same.
        /// </summary>
        public bool IncludeSmurfs { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether accounts outside the crew are looked up too. Defaults to
        /// false, like <c>GET lol/Home</c>. Each such account costs one more Riot call per refresh, only paid
        /// by the callers who ask for them.
        /// </summary>
        public bool IncludeOutOfCrew { get; set; }
    }
}
