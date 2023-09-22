// <copyright file="UpdateTournamentCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Tournaments.Commands.UpdateTournament
{
    using MediatR;
    using YuGames.Common.DTOs;
    using YuGames.Domain;

    /// <summary>
    /// UpdateTournamentCommand class.
    /// </summary>
    public class UpdateTournamentCommand : IRequest<Tournament>
    {
        /// <summary>
        /// Gets or sets Tournament ID.
        /// </summary>
        public int TournamentId { get; set; }

        /// <summary>
        /// Gets or sets Tournament Dto.
        /// </summary>
        public TournamentDto TournamentDto { get; set; } = new TournamentDto();
    }
}
