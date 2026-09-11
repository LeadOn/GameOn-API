// <copyright file="LinkSmurfAccountCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Commands.LinkSmurfAccount
{
    using GameOn.Common.DTOs.LeagueOfLegends;
    using MediatR;

    /// <summary>
    /// LinkSmurfAccountCommand class. Attaches a Riot account to an existing player as one of their
    /// smurfs, creating the account row when it isn't known yet.
    /// </summary>
    public class LinkSmurfAccountCommand : IRequest<LinkSmurfAccountResultDto>
    {
        /// <summary>
        /// Gets or sets the ID of the player the smurf account belongs to.
        /// </summary>
        public int PrimaryPlayerId { get; set; }

        /// <summary>
        /// Gets or sets the Riot Games nickname (game name) of the smurf account.
        /// </summary>
        public string RiotGamesNickname { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Riot Games tag line of the smurf account (example: EUW).
        /// </summary>
        public string RiotGamesTagLine { get; set; } = string.Empty;
    }
}
