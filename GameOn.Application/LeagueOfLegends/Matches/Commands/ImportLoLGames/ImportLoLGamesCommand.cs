// <copyright file="ImportLoLGamesCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Matches.Commands.ImportLoLGames
{
    using MediatR;

    /// <summary>
    /// ImportLoLGamesCommand class.
    /// </summary>
    public class ImportLoLGamesCommand : IRequest<bool>
    {
        /// <summary>
        /// Gets or sets list of matches IDs.
        /// </summary>
        public List<string> MatchIDs { get; set; } = new List<string>();
    }
}
