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
        public ??? Team1 {
            get;
            set;
        }
        public ??? Team2 {
            get;
            set;
        }
        
        
        
        /// <summary>
        /// Gets or sets Game Details.
        /// </summary>
        public GamePlayed GameDetails { get; set; } = new GamePlayed();

        /// <summary>
        /// Gets or sets Game players.
        /// </summary>
        public List<TeamPlayer> Players { get; set; } = new List<TeamPlayer>();
    }
}