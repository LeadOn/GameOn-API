// <copyright file="VoteSoccerFiveCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.SoccerFives.Commands.VoteSoccerFive
{
    using GameOn.Common.DTOs;
    using MediatR;

    /// <summary>
    /// VoteSoccerFiveCommand class.
    /// </summary>
    public class VoteSoccerFiveCommand : IRequest<bool>
    {
        /// <summary>
        /// Gets or sets VoteSoccerFiveDto.
        /// </summary>
        public VoteSoccerFiveDto Vote { get; set; } = new VoteSoccerFiveDto();
    }
}
