// <copyright file="UpdateLoLGameCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Matches.Commands.UpdateLoLGame
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// UpdateLoLGameCommand class.
    /// </summary>
    public class UpdateLoLGameCommand : IRequest<LoLGame>
    {
        /// <summary>
        /// Gets or sets Match ID.
        /// </summary>
        public string MatchId { get; set; } = string.Empty;
    }
}
