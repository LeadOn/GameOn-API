// <copyright file="CreateTournamentCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Tournaments.Commands.CreateTournament
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// CreateTournamentCommand class.
    /// </summary>
    public class CreateTournamentCommand : IRequest<Tournament>
    {
        /// <summary>
        /// Gets or sets Tournament.
        /// </summary>
        public Tournament Tournament { get; set; } = new Tournament();
    }
}
