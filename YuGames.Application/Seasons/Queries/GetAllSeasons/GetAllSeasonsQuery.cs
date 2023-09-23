﻿// <copyright file="GetAllSeasonsQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Seasons.Queries.GetAllSeasons
{
    using MediatR;
    using YuGames.Domain;

    /// <summary>
    /// GetAllSeasonsQuery class.
    /// </summary>
    public class GetAllSeasonsQuery : IRequest<IEnumerable<Season>>
    {
    }
}
