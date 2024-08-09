// <copyright file="CreateSoccerFiveCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.SoccerFives.Commands.CreateSoccerFive
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// CreateSoccerFiveCommand class.
    /// </summary>
    public class CreateSoccerFiveCommand : IRequest<SoccerFive>
    {
        /// <summary>
        /// Gets or sets Five.
        /// </summary>
        public SoccerFive SoccerFive { get; set; } = new SoccerFive();
    }
}
