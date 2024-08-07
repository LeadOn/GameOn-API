// <copyright file="GetFifaTeamByIdQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FifaTeams.Queries.GetFifaTeamById
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetFifaTeamByIdQuery class.
    /// </summary>
    public class GetFifaTeamByIdQuery : IRequest<FifaTeam?>
    {
        /// <summary>
        /// Gets or sets ID.
        /// </summary>
        public int Id { get; set; }
    }
}
