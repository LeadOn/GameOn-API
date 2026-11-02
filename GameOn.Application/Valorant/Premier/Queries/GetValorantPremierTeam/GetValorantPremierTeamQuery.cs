// <copyright file="GetValorantPremierTeamQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Premier.Queries.GetValorantPremierTeam
{
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// Gets a Valorant Premier team by its roster ID.
    /// </summary>
    public class GetValorantPremierTeamQuery : IRequest<ValorantPremierTeam?>
    {
        /// <summary>
        /// Gets or sets the Premier roster ID.
        /// </summary>
        public string TeamId { get; set; } = string.Empty;
    }
}
