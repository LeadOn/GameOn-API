// <copyright file="GetAllHighlightsQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Highlights.Queries.GetAllHighlights
{
    using MediatR;
    using YuGames.Domain;

    /// <summary>
    /// GetAllHighlightsQuery class.
    /// </summary>
    public class GetAllHighlightsQuery : IRequest<IEnumerable<Highlight>>
    {
    }
}
