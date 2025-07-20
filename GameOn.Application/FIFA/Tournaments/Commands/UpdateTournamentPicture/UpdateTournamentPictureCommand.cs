// <copyright file="UpdateTournamentPictureCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.Tournaments.Commands.UpdateTournamentPicture
{
    using GameOn.Common.DTOs;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// UpdateTournamentPictureCommand class.
    /// </summary>
    public class UpdateTournamentPictureCommand : IRequest<bool>
    {
        /// <summary>
        /// Gets or sets Tournament ID.
        /// </summary>
        public int TournamentId { get; set; }

        /// <summary>
        /// Gets or sets File.
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public IFormFile File { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    }
}
