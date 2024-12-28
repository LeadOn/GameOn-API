// <copyright file="GetGlobalStatsQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Stats.Queries.GetGlobalStats
{
    using GameOn.Common.DTOs;
    using MediatR;

    /// <summary>
    /// GetGlobalStatsQuery class.
    /// </summary>
    public class GetGlobalStatsQuery : IRequest<GlobalStatsDto>
    {
    }
}
