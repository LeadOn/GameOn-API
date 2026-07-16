// <copyright file="GetAllQueuesQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Queues.Queries.GetAllQueues
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetAllQueuesQuery class.
    /// </summary>
    public class GetAllQueuesQuery : IRequest<IEnumerable<LoLQueue>>
    {
    }
}
