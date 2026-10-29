// <copyright file="Player.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System.Text.Json.Serialization;

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
        /// Gets or sets player's Valorant PUUID (36-char Riot account UUID used by Valorant endpoints,
        /// distinct from the API-key-encrypted <see cref="RiotGamesPUUID"/>).
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
        /// Gets or sets the ID of the account this player is a smurf of, or null when this player is
        /// a primary account (a real person). Smurf rows hold their own Riot account, rank history and
        /// game participations; reads roll them up to the primary account they point at.
        /// </summary>
        public int? PrimaryPlayerId { get; set; }

        /// <summary>
        /// Gets or sets player's creation date.
        /// </summary>
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets whether a player is archived or not.
        /// </summary>
        public bool Archived { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether this account belongs to the crew. Crew accounts are
        /// refreshed automatically and are the only ones counted by the lists and the stats; the others
        /// are still refreshed on demand, they just stop weighing on numbers meant to describe the crew.
        /// Defaults to false so a brand new account never lands in the stats before someone says it should.
        /// </summary>
        public bool InCrew { get; set; } = false;

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
        /// Gets or sets League of Legends Rank History.
        /// </summary>
        [JsonIgnore]
        public virtual List<LeagueOfLegendsRankHistory> LeagueOfLegendsRankHistory { get; set; } = null!;

        /// <summary>
        /// Gets or sets League of Legends Game Participants.
        /// </summary>
        [JsonIgnore]
        public virtual List<LoLGameParticipant> LeagueOfLegendsGameParticipants { get; set; } = null!;

        /// <summary>
        /// Gets or sets Valorant Rank History.
        /// </summary>
        [JsonIgnore]
        public virtual List<ValorantRankHistory> ValorantRankHistory { get; set; } = null!;

        /// <summary>
        /// Gets or sets Valorant Game Participants.
        /// </summary>
        [JsonIgnore]
        public virtual List<ValGameParticipant> ValorantGameParticipants { get; set; } = null!;

        /// <summary>
        /// Gets or sets the primary account this player is a smurf of.
        /// </summary>
        [JsonIgnore]
        public virtual Player? PrimaryPlayer { get; set; }

        /// <summary>
        /// Gets or sets the smurf accounts attached to this player. Never more than one level deep:
        /// a smurf cannot itself hold smurfs.
        /// </summary>
        [JsonIgnore]
        public virtual List<Player> SmurfAccounts { get; set; } = null!;
    }
}