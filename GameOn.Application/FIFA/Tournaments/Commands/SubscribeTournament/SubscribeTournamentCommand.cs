﻿// <copyright file="SubscribeTournamentCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.Tournaments.Commands.SubscribeTournament
{
    using GameOn.Common.DTOs;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// SubscribeTournamentCommand class.
    /// </summary>
    public class SubscribeTournamentCommand : IRequest<TournamentPlayer>
    {
        /// <summary>
        /// Gets or sets Tournament ID.
        /// </summary>
        public int TournamentId { get; set; }

        /// <summary>
        /// Gets or sets ConnectedPlayer.
        /// </summary>
        public ConnectedPlayerDto Player { get; set; } = new ConnectedPlayerDto();

        /// <summary>
        /// Gets or sets Fifa Team ID.
        /// </summary>
        public int FifaTeamId { get; set; }
    }
}
