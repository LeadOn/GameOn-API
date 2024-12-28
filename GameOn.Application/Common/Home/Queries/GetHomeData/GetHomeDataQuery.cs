// <copyright file="GetHomeDataQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Home.Queries.GetHomeData
{
    using GameOn.Common.DTOs.Common;
    using MediatR;

    /// <summary>
    /// GetHomeDataQuery class.
    /// </summary>
    public class GetHomeDataQuery : IRequest<HomeDataDto>
    {
    }
}
