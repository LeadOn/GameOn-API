// <copyright file="DeleteSoccerFiveCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.SoccerFives.Commands.DeleteSoccerFive
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// DeleteSoccerFiveCommand class.
    /// </summary>
    public class DeleteSoccerFiveCommand : IRequest<bool>
    {
        /// <summary>
        /// Gets or sets five ID.
        /// </summary>
        public int SoccerFiveId { get; set; }

        /// <summary>
        /// Gets or sets current player ID.
        /// </summary>
        public int CurrentPlayerId { get; set; }
    }
}
