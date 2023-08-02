// <copyright file="TeamDto.cs" company="LeadOn's Corp'">
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
        public int Id { get; set; }
        public string Code { get; set; }
        public int Score { get; set; }
        public List<Player> Players { get; set; }
    }
}