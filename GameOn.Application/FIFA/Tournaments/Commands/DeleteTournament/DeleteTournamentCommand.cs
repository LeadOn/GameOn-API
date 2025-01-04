// <copyright file="DeleteTournamentCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.Tournaments.Commands.DeleteTournament
{
    using MediatR;

    /// <summary>
    /// DeleteTournamentCommand class.
    /// </summary>
    public class DeleteTournamentCommand : IRequest<bool>
    {
        /// <summary>
        /// Gets or sets Tournament ID.
        /// </summary>
        public int TournamentId { get; set; }
    }
}
