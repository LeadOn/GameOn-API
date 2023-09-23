// <copyright file="GetCurrentSeasonQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Seasons.Queries.GetCurrentSeason
{
    using MediatR;
    using YuGames.Domain;

    /// <summary>
    /// GetCurrentSeasonQuery class.
    /// </summary>
    public class GetCurrentSeasonQuery : IRequest<Season?>
    {
    }
}
