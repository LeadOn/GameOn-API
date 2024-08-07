// <copyright file="GoToPhase1Command.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Tournaments.Commands.GoToPhase1
{
    using MediatR;

    /// <summary>
    /// GoToPhase1Command class.
    /// </summary>
    public class GoToPhase1Command : IRequest<bool>
    {
        /// <summary>
        /// Gets or sets Tournament ID.
        /// </summary>
        public int TournamentId { get; set; }
    }
}
