// <copyright file="GamePlayedDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>
namespace YuFoot.DTOs
{
    using YuFoot.Entities;

    /// <summary>
    /// GamePlayedDto class.
    /// </summary>
    public class GamePlayedDto
    {
        public int Id { get; set; }
        public DateTime PlayedOn { get; set; }
        public int PlatformId { get; set; }
        public TeamDto Team1 { get; set; }
        public TeamDto Team2 { get; set; }
    }
}