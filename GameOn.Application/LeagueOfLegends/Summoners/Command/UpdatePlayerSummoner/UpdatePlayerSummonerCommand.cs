// <copyright file="UpdatePlayerSummonerCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Commands.UpdatePlayerSummoner
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// UpdatePlayerSummonerCommand class.
    /// </summary>
    public class UpdatePlayerSummonerCommand : IRequest<Player>
    {
        /// <summary>
        /// Gets or sets Player.
        /// </summary>
        public Player Player { get; set; } = new Player();
    }
}
