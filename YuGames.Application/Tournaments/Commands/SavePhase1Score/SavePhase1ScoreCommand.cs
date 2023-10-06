// <copyright file="SavePhase1ScoreCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Tournaments.Commands.SavePhase1Score
{
    using MediatR;

    /// <summary>
    /// SavePhase1ScoreCommand class.
    /// </summary>
    public class SavePhase1ScoreCommand : IRequest<bool>
    {
        /// <summary>
        /// Gets or sets Tournament ID.
        /// </summary>
        public int TournamentId { get; set; }
    }
}
