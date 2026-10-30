// <copyright file="GetAllValorantPlayersQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Players.Queries.GetAllValorantPlayers
{
    using System.Collections.Generic;
    using GameOn.Common.DTOs;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetAllValorantPlayersQuery class.
    /// </summary>
    public class GetAllValorantPlayersQuery : IRequest<IEnumerable<PlayerDto>>
    {
        /// <summary>
        /// Gets or sets a value indicating whether archived players are returned instead of active ones.
        /// </summary>
        public bool Archived { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether smurf accounts are listed alongside primary accounts.
        /// </summary>
        public bool IncludeSmurfs { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether accounts outside the crew (<see cref="Player.InCrew"/>)
        /// are listed too.
        /// </summary>
        public bool IncludeOutOfCrew { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether accounts with a Riot ID but no Valorant data are listed
        /// too. Defaults to true. When false, an account needs a Valorant PUUID and at least one rank
        /// snapshot or stored game: a League of Legends-only player is left out.
        /// </summary>
        public bool IncludeWithoutValorant { get; set; } = true;
    }
}