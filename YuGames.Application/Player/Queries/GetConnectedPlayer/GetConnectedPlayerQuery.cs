// <copyright file="GetConnectedPlayerQuery.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Player.Queries.GetConnectedPlayer
{
    using MediatR;
    using YuGames.Common.DTOs;
    using YuGames.Domain;

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
