// <copyright file="GetConnectedPlayerQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Players.Queries.GetConnectedPlayer
{
    using GameOn.Common.DTOs;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// GetConnectedPlayerQuery class.
    /// </summary>
    public class GetConnectedPlayerQuery : IRequest<Player?>
    {
        /// <summary>
        /// Gets or sets Connected Player DTO.
        /// </summary>
        public ConnectedPlayerDto ConnectedPlayer { get; set; } = new ConnectedPlayerDto();
    }
}
