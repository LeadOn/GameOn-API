// <copyright file="GetLoLGlobalStatsQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Stats.Queries.GetLoLGlobalStats
{
    using GameOn.Common.DTOs.LeagueOfLegends;
    using MediatR;

    /// <summary>
    /// GetLoLGlobalStatsQuery class.
    /// </summary>
    public class GetLoLGlobalStatsQuery : IRequest<LoLGlobalStatsDto>
    {
    }
}
