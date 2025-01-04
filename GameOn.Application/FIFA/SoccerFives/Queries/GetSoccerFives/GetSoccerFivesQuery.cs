// <copyright file="GetSoccerFivesQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.SoccerFives.Queries.GetSoccerFives
{
    using GameOn.Common.DTOs;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetSoccerFivesQuery class.
    /// </summary>
    public class GetSoccerFivesQuery : IRequest<List<SoccerFive>>
    {
    }
}
