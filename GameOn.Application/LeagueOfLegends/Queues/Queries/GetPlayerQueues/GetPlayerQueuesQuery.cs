// <copyright file="GetPlayerQueuesQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Queues.Queries.GetPlayerQueues
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetPlayerQueuesQuery class.
    /// </summary>
    public class GetPlayerQueuesQuery : IRequest<IEnumerable<LoLQueue>>
    {
        /// <summary>
        /// Gets or sets player Id.
        /// </summary>
        public int PlayerId { get; set; }
    }
}
