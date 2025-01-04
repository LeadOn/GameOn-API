// <copyright file="GetAllHighlightsQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Highlights.Queries.GetAllHighlights
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetAllHighlightsQuery class.
    /// </summary>
    public class GetAllHighlightsQuery : IRequest<IEnumerable<Highlight>>
    {
    }
}
