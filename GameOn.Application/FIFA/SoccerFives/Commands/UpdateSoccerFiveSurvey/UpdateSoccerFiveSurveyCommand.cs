// <copyright file="UpdateSoccerFiveSurveyCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.SoccerFives.Commands.UpdateSoccerFiveSurvey
{
    using GameOn.Common.DTOs;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// UpdateSoccerFiveSurveyCommand class.
    /// </summary>
    public class UpdateSoccerFiveSurveyCommand : IRequest<SoccerFiveDto>
    {
        /// <summary>
        /// Gets or sets current player ID.
        /// </summary>
        public int CurrentPlayerId { get; set; }

        /// <summary>
        /// Gets or sets Survey.
        /// </summary>
        public UpdateSoccerFiveSurvey Survey { get; set; } = new UpdateSoccerFiveSurvey();
    }
}
