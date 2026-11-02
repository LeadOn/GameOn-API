// <copyright file="GetValorantBackgroundQueueStatusQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Background.Queries.GetValorantBackgroundQueueStatus
{
    using GameOn.Common.DTOs.Valorant;
    using MediatR;

    /// <summary>
    /// Gets the state of the Valorant background queue.
    /// </summary>
    public class GetValorantBackgroundQueueStatusQuery : IRequest<ValBackgroundQueueStatusDto>
    {
    }
}
