// <copyright file="GetGlobalStatsQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Stats.Queries.GetGlobalStats
{
    using MediatR;
    using YuGames.Common.DTOs;

    /// <summary>
    /// GetGlobalStatsQuery class.
    /// </summary>
    public class GetGlobalStatsQuery : IRequest<GlobalStatsDto>
    {
    }
}
