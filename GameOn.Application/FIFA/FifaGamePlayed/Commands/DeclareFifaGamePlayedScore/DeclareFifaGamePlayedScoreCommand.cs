// <copyright file="DeclareFifaGamePlayedScoreCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.FifaGamePlayed.Commands.DeclareFifaGamePlayedScore
{
    using GameOn.Common.DTOs;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// DeclareFifaGamePlayedScoreCommand class.
    /// </summary>
    public class DeclareFifaGamePlayedScoreCommand : IRequest<FifaGamePlayed?>
    {
        /// <summary>
        /// Gets or sets Declare Score DTO.
        /// </summary>
        public DeclareScoreDto ScoreDto { get; set; } = new DeclareScoreDto();
    }
}
