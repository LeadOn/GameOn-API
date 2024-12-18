// <copyright file="Player.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System.Text.Json.Serialization;
    using GameOn.Domain;

    /// <summary>
    /// Player class.
    /// </summary>
    public class Player
    {
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
        /// Gets or sets TeamPlayers.
        /// </summary>
        [JsonIgnore]
        public virtual List<FifaTeamPlayer> FifaTeamPlayers { get; set; } = null!;

        /// <summary>
        /// Gets or sets Highlights.
        /// </summary>
        [JsonIgnore]
        public virtual List<Highlight> Highlights { get; set; } = null!;

        /// <summary>
        /// Gets or sets TournamentPlayer.
        /// </summary>
        [JsonIgnore]
        public virtual List<TournamentPlayer> TournamentPlayed { get; set; } = null!;

        /// <summary>
        /// Gets or sets games created.
        /// </summary>
        [JsonIgnore]
        public virtual List<FifaGamePlayed> FifaGameCreated { get; set; } = null!;

        /// <summary>
        /// Gets or sets five created.
        /// </summary>
        [JsonIgnore]
        public virtual List<SoccerFive> SoccerFivesCreated { get; set; } = null!;

        /// <summary>
        /// Gets or sets five created.
        /// </summary>
        [JsonIgnore]
        public virtual List<SoccerFiveVoteAnswer> SoccerFiveVoteAnswers { get; set; } = null!;
    }
}