// <copyright file="UpdateSoccerFiveCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.SoccerFives.Commands.UpdateSoccerFive
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// UpdateSoccerFiveCommand class.
    /// </summary>
    public class UpdateSoccerFiveCommand : IRequest<SoccerFive>
    {
        /// <summary>
        /// Gets or sets five ID.
        /// </summary>
        public int SoccerFiveId { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets planned on date.
        /// </summary>
        public DateTime? PlannedOn { get; set; }

        /// <summary>
        /// Gets or sets state.
        /// </summary>
        public int? State { get; set; }
    }
}
