// <copyright file="PlayerDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs
{
    using GameOn.Common.DTOs.LeagueOfLegends;
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
            this.LolSummonerLevel = player.LolSummonerLevel;
            this.LolRefreshedOn = player.LolRefreshedOn;
            this.CreatedOn = player.CreatedOn;
            this.Archived = player.Archived;
            this.LolIconId = player.LolIconId;
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
        /// Gets or sets player's LOL Summoner's level.
        /// </summary>
        public long? LolSummonerLevel { get; set; }

        /// <summary>
        /// Gets or sets Lol Icon ID.
        /// </summary>
        public int? LolIconId { get; set; }

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

        /// <summary>
        /// Gets or sets the player's most recent ranked Solo/Duo game results, newest to oldest (true = win).
        /// Fewer entries when the player hasn't played that many ranked Solo/Duo games yet, empty if none.
        /// How many games back this goes depends on the endpoint (5 on <c>GET lol/summoner</c>, the ladder's
        /// compact form; 8 on <c>GET lol/summoner/{id}</c>, the profile page's rank card).
        /// </summary>
        public List<bool> RecentFormSolo { get; set; } = new List<bool>();

        /// <summary>
        /// Gets or sets the player's most recent ranked Flex game results. See <see cref="RecentFormSolo"/>
        /// for ordering, emptiness, and the per-endpoint game count.
        /// </summary>
        public List<bool> RecentFormFlex { get; set; } = new List<bool>();

        /// <summary>
        /// Gets or sets the Solo/Duo LP change over the trailing 7 days (current LP minus the LP as of 7
        /// days ago, on a continuous cross-tier scale). Null when there isn't a rank snapshot both now and
        /// at least 7 days ago (e.g. a newly linked account, or a queue never played).
        /// </summary>
        public int? LpChange7DaysSolo { get; set; }

        /// <summary>
        /// Gets or sets the Flex LP change over the trailing 7 days. See <see cref="LpChange7DaysSolo"/>.
        /// </summary>
        public int? LpChange7DaysFlex { get; set; }

        /// <summary>
        /// Gets or sets the player's performance recap over the requested time window (see
        /// <see cref="LoLStatsPeriod"/>), all queues combined. Null when the player has no game in the period.
        /// </summary>
        public LoLSummonerPerformanceStatsDto? PerformanceStats { get; set; }
    }
}
