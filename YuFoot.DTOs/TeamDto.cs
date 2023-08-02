﻿// <copyright file="TeamDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>
namespace YuFoot.DTOs
{
    using YuFoot.Entities;

    /// <summary>
    /// TeamDto class.
    /// </summary>
    public class TeamDto
    {
        /// <summary>
        /// Gets or sets ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets team code.
        /// </summary>
        public string Code { get; set; } = "Unknown";

        /// <summary>
        /// Gets or sets Score.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Gets or sets players.
        /// </summary>
        public List<Player> Players { get; set; } = new List<Player>();
    }
}