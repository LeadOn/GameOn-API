// <copyright file="UpdatePlayerSummonerAdminCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Commands.UpdatePlayerSummonerAdmin
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// UpdatePlayerSummonerAdmin class.
    /// </summary>
    public class UpdatePlayerSummonerAdminCommand : IRequest<Player>
    {
        /// <summary>
        /// Gets or sets Player.
        /// </summary>
        public Player Player { get; set; } = new Player();
    }
}
