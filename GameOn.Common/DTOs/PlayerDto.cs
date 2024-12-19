// <copyright file="PlayerDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs
{
    using System.Text.Json.Serialization;
    using GameOn.Domain;

    /// <summary>
    /// PlayerDto class.
    /// </summary>
    public class PlayerDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerDto"/> class.
        /// </summary>
        public PlayerDto()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerDto"/> class.
        /// </summary>
        /// <param name="player"><see cref="Player" />.</param>
        public PlayerDto(Player player)
        {
            this.Id = player.Id;
            this.KeycloakId = player.KeycloakId;
            this.FullName = player.FullName;
            this.Nickname = player.Nickname;
            this.ProfilePictureUrl = player.ProfilePictureUrl;
            this.RiotGamesNickname = player.RiotGamesNickname;
            this.RiotGamesPUUID = player.RiotGamesPUUID;
            this.RiotGamesTagLine = player.RiotGamesTagLine;
            this.LolSummonerId = player.LolSummonerId;
            this.LolSummonerLevel = player.LolSummonerLevel;
            this.LolRefreshedOn = player.LolRefreshedOn;
            this.CreatedOn = player.CreatedOn;
            this.Archived = player.Archived;
        }

        /// <summary>
        /// Gets or sets player's ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets player's Keycloak ID.
        /// </summary>
        public string? KeycloakId { get; set; }

        /// <summary>
        /// Gets or sets player's full name.
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Gets or sets player's nickname.
        /// </summary>
        public string Nickname { get; set; } = "J0hnD03";

        /// <summary>
        /// Gets or sets player's profile picture URL.
        /// </summary>
        public string? ProfilePictureUrl { get; set; }

        /// <summary>
        /// Gets or sets player's Riot Games nickname.
        /// </summary>
        public string? RiotGamesNickname { get; set; }

        /// <summary>
        /// Gets or sets player's Riot Games tag line.
        /// </summary>
        public string? RiotGamesTagLine { get; set; }

        /// <summary>
        /// Gets or sets player's Riot Games PUUID.
        /// </summary>
        public string? RiotGamesPUUID { get; set; }

        /// <summary>
        /// Gets or sets player's LOL Summoner ID.
        /// </summary>
        public string? LolSummonerId { get; set; }

        /// <summary>
        /// Gets or sets player's LOL Summoner's level.
        /// </summary>
        public long? LolSummonerLevel { get; set; }

        /// <summary>
        /// Gets or sets player's LOL refreshed on datetime.
        /// </summary>
        public DateTime? LolRefreshedOn { get; set; }

        /// <summary>
        /// Gets or sets player's creation date.
        /// </summary>
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets whether a player is archived or not.
        /// </summary>
        public bool Archived { get; set; } = false;

        /// <summary>
        /// Gets or sets Tournaments won.
        /// </summary>
        public virtual List<Tournament> TournamentsWon { get; set; } = null!;

        /// <summary>
        /// Gets or sets current player's League of Legends solo queue rank.
        /// </summary>
        public LeagueOfLegendsRankHistory? LeagueOfLegendsSoloRank { get; set; } = null!;

        /// <summary>
        /// Gets or sets current player's League of Legends Flex rank.
        /// </summary>
        public LeagueOfLegendsRankHistory? LeagueOfLegendsFlexRank { get; set; } = null!;
    }
}
