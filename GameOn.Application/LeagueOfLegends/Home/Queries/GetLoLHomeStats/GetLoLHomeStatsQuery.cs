// <copyright file="GetLoLHomeStatsQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Home.Queries.GetLoLHomeStats
{
    using GameOn.Common.DTOs.LeagueOfLegends;
    using MediatR;

    /// <summary>
    /// GetLoLHomeStatsQuery class. Aggregated recap powering the League of Legends v2 home page.
    /// </summary>
    public class GetLoLHomeStatsQuery : IRequest<LoLHomeStatsDto>
    {
    }
}
