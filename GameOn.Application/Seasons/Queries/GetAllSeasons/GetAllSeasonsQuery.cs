// <copyright file="GetAllSeasonsQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Seasons.Queries.GetAllSeasons
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetAllSeasonsQuery class.
    /// </summary>
    public class GetAllSeasonsQuery : IRequest<IEnumerable<Season>>
    {
    }
}
