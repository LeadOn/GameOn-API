// <copyright file="PlayerDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs
{
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Common.DTOs.Valorant;
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
            this.PrimaryPlayerId = player.PrimaryPlayerId;
            this.InCrew = player.InCrew;
            this.ValorantPuuid = player.ValorantPuuid;
            this.ValorantAccountLevel = player.ValorantAccountLevel;
            this.ValorantPlayerCardId = player.ValorantPlayerCardId;
            this.ValorantPlayerTitleId = player.ValorantPlayerTitleId;
            this.ValorantRegion = player.ValorantRegion;
            this.ValorantRefreshedOn = player.ValorantRefreshedOn;
            this.ValorantPeakTierId = player.ValorantPeakTierId;
            this.ValorantPeakTierName = player.ValorantPeakTierName;
            this.ValorantPeakRr = player.ValorantPeakRr;
            this.ValorantPeakSeasonShort = player.ValorantPeakSeasonShort;

            if (player.ValorantPeakTierId is >= 3 and <= 27)
            {
                this.ValorantPeakRankIconUrl = $"https://media.valorant-api.com/competitivetiers/03621f52-342b-cf4e-4f86-9350a49c6d04/{player.ValorantPeakTierId}/largeicon.png";
            }

            if (!string.IsNullOrWhiteSpace(player.ValorantPlayerCardId) && player.ValorantPlayerCardId != Guid.Empty.ToString())
            {
                this.ValorantPlayerCardSmallUrl = $"https://media.valorant-api.com/playercards/{player.ValorantPlayerCardId}/smallart.png";
                this.ValorantPlayerCardWideUrl = $"https://media.valorant-api.com/playercards/{player.ValorantPlayerCardId}/wideart.png";
                this.ValorantPlayerCardLargeUrl = $"https://media.valorant-api.com/playercards/{player.ValorantPlayerCardId}/largeart.png";
            }
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
        /// Gets or sets the ID of the player this account is a smurf of, or null when it is a primary
        /// account. Set on the entries of <see cref="SmurfAccounts"/>, null on the member holding them.
        /// </summary>
        public int? PrimaryPlayerId { get; set; }

        /// <summary>
        /// Gets or sets Lol Icon ID.
        /// </summary>
        public int? LolIconId { get; set; }

        /// <summary>
        /// Gets or sets player's LOL refreshed on datetime.
        /// </summary>
        public DateTime? LolRefreshedOn { get; set; }

        /// <summary>
        /// Gets or sets player's Valorant PUUID.
        /// </summary>
        public string? ValorantPuuid { get; set; }

        /// <summary>
        /// Gets or sets player's Valorant account level.
        /// </summary>
        public int? ValorantAccountLevel { get; set; }

        /// <summary>
        /// Gets or sets player's equipped Valorant player card UUID.
        /// </summary>
        public string? ValorantPlayerCardId { get; set; }

        /// <summary>
        /// Gets or sets the square thumbnail URL of the player's equipped Valorant player card
        /// (<c>smallart.png</c> on <c>media.valorant-api.com</c>), or null when no card is set.
        /// </summary>
        public string? ValorantPlayerCardSmallUrl { get; set; }

        /// <summary>
        /// Gets or sets the horizontal banner URL of the player's equipped Valorant player card
        /// (<c>wideart.png</c> on <c>media.valorant-api.com</c>), or null when no card is set.
        /// </summary>
        public string? ValorantPlayerCardWideUrl { get; set; }

        /// <summary>
        /// Gets or sets the full-height vertical banner URL of the player's equipped Valorant player card
        /// (<c>largeart.png</c> on <c>media.valorant-api.com</c>), or null when no card is set.
        /// </summary>
        public string? ValorantPlayerCardLargeUrl { get; set; }

        /// <summary>
        /// Gets or sets player's equipped Valorant player title UUID.
        /// </summary>
        public string? ValorantPlayerTitleId { get; set; }

        /// <summary>
        /// Gets or sets player's Valorant region affinity (e.g. eu, na, ap, kr, latam, br).
        /// </summary>
        public string? ValorantRegion { get; set; }

        /// <summary>
        /// Gets or sets player's Valorant profile refreshed on datetime.
        /// </summary>
        public DateTime? ValorantRefreshedOn { get; set; }

        /// <summary>
        /// Gets or sets player's all-time peak Valorant competitive tier ID.
        /// </summary>
        public int? ValorantPeakTierId { get; set; }

        /// <summary>
        /// Gets or sets player's all-time peak Valorant competitive tier name.
        /// </summary>
        public string? ValorantPeakTierName { get; set; }

        /// <summary>
        /// Gets or sets player's Ranked Rating (RR) at their all-time peak Valorant tier.
        /// </summary>
        public int? ValorantPeakRr { get; set; }

        /// <summary>
        /// Gets or sets the Valorant Act/Season short code (e.g. e9a3) in which the peak rank was achieved.
        /// </summary>
        public string? ValorantPeakSeasonShort { get; set; }

        /// <summary>
        /// Gets or sets the large rank icon URL for the player's all-time peak Valorant tier on
        /// <c>media.valorant-api.com</c>, or null when no peak rank is set.
        /// </summary>
        public string? ValorantPeakRankIconUrl { get; set; }

        /// <summary>
        /// Gets or sets the player's most recent Valorant rank snapshot.
        /// </summary>
        public ValorantRankHistory? ValorantCurrentRank { get; set; }

        /// <summary>
        /// Gets or sets the player's Valorant Elo/RR change over the trailing 7 days (current Elo minus
        /// the Elo as of 7 days ago). Null when there isn't a rank snapshot both now and at least 7 days ago.
        /// </summary>
        public int? RrChange7DaysValorant { get; set; }

        /// <summary>
        /// Gets or sets the player's most recent ranked Valorant game results, newest to oldest (true = win).
        /// </summary>
        public List<bool> RecentFormValorant { get; set; } = new List<bool>();

        /// <summary>
        /// Gets or sets the Valorant agent this account played the most over the last rolling month.
        /// </summary>
        public string? MainAgentName { get; set; }

        /// <summary>
        /// Gets or sets the square icon URL on <c>media.valorant-api.com</c> for <see cref="MainAgentName"/>.
        /// </summary>
        public string? MainAgentIconUrl { get; set; }

        /// <summary>
        /// Gets or sets the account's Valorant performance recap over the requested filters (period, queues,
        /// agent). Null when the account has no counted game. Covers this account alone: a smurf's games are
        /// never counted towards its owner. Only served by <c>GET val/player/{id}</c>; null on every other route.
        /// </summary>
        public ValorantPlayerPerformanceStatsDto? ValorantPerformanceStats { get; set; }

        /// <summary>
        /// Gets or sets player's creation date.
        /// </summary>
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets whether a player is archived or not.
        /// </summary>
        public bool Archived { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether this account belongs to the crew. Accounts outside the
        /// crew are refreshed on demand only and are left out of the lists and the stats.
        /// </summary>
        public bool InCrew { get; set; } = false;

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
        /// Gets or sets the champion this account played the most over the last rolling month, every queue
        /// combined, as Riot's internal name (ex: "MonkeyKing" for Wukong). Same computation as the first
        /// entry of <see cref="LoLSummonerPerformanceStatsDto.ChampionStats"/> on <c>GET lol/summoner/{id}</c>
        /// with <c>period=Month</c>: same games and same exclusions (remakes, bot and tutorial queues), ties
        /// broken by win rate then champion name. Null when the account has no game over the month. Only
        /// served by <c>GET lol/summoner</c>; null on every other route.
        /// </summary>
        public string? MainChampionName { get; set; }

        /// <summary>
        /// Gets or sets the player's performance recap over the requested time window (see
        /// <see cref="LoLStatsPeriod"/>), all queues combined. Null when the player has no game in the
        /// period. Covers this account alone: a smurf's games are never counted towards its owner.
        /// </summary>
        public LoLSummonerPerformanceStatsDto? PerformanceStats { get; set; }

        /// <summary>
        /// Gets or sets the other Riot accounts this player owns, each with its own rank. Informational
        /// only: accounts stay fully independent everywhere else — their games, records and stats are
        /// never merged into this player's. Only served by <c>GET lol/summoner/{id}</c>, and only
        /// identity, level, icon and rank fields are filled in. Empty for a player with no smurf, and
        /// for a smurf account itself.
        /// </summary>
        public List<PlayerDto> SmurfAccounts { get; set; } = new List<PlayerDto>();
    }
}
