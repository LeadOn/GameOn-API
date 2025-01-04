// <copyright file="GetCurrentSeasonQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.Seasons.Queries.GetCurrentSeason
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetCurrentSeasonQuery class.
    /// </summary>
    public class GetCurrentSeasonQuery : IRequest<Season?>
    {
    }
}
