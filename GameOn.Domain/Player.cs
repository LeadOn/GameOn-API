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
        /// Gets or sets the remaining Valorant rank protection shields.
        /// </summary>
        public int? ValorantRankProtectionShields { get; set; }

        /// <summary>
        /// Gets or sets the Valorant rank protection status (e.g. Empty, Full).
        /// </summary>
        public string? ValorantRankProtectionStatus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the player currently sits on a protected Valorant tier floor.
        /// </summary>
        public bool? ValorantIsAtRankProtectedTier { get; set; }

        /// <summary>
        /// Gets or sets the Valorant placement games still needed to get a rank.
        /// </summary>
        public int? ValorantGamesNeededForRating { get; set; }

        /// <summary>
        /// Gets or sets the Valorant ranked games still needed to appear on the leaderboard.
        /// </summary>
        public int? ValorantGamesNeededForLeaderboard { get; set; }

        /// <summary>
        /// Gets or sets the player's current Valorant leaderboard rank, null when not on it.
        /// </summary>
        public int? ValorantLeaderboardRank { get; set; }

        /// <summary>
        /// Gets or sets the UTC date the Valorant leaderboard rank was last updated.
        /// </summary>
        public DateTime? ValorantLeaderboardUpdatedOn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the player hides their Valorant act rank badge.
        /// </summary>
        public bool? ValorantIsActRankBadgeHidden { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the player is anonymized on the Valorant leaderboard.
        /// </summary>
        public bool? ValorantIsLeaderboardAnonymized { get; set; }

        /// <summary>
        /// Gets or sets the ranking schema the Valorant peak was reached under (base or ascendant).
        /// </summary>
        public string? ValorantPeakRankingSchema { get; set; }

        /// <summary>
        /// Gets or sets the Valorant lifetime prestige as raw JSON (e.g. <c>{"DIAMOND": {"count": 6}}</c>).
        /// </summary>
        public string? ValorantLifetimePrestigeJson { get; set; }

        /// <summary>
        /// Gets or sets the ID of the Valorant Premier team the player belongs to, null when none.
        /// </summary>
        public string? ValorantPremierTeamId { get; set; }

        /// <summary>
        /// Gets or sets the player's role in their Valorant Premier team (e.g. OWNER, MEMBER).
        /// </summary>
        public string? ValorantPremierRole { get; set; }

        /// <summary>
        /// Gets or sets the UTC date the player joined their Valorant Premier team.
        /// </summary>
        public DateTime? ValorantPremierJoinedOn { get; set; }

        /// <summary>
        /// Gets or sets the UTC date the Valorant accolades, agent mastery and Premier team were last refreshed.
        /// </summary>
        public DateTime? ValorantExtrasRefreshedOn { get; set; }

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
        /// Gets or sets the Valorant end-of-act results.
        /// </summary>
        [JsonIgnore]
        public virtual List<ValorantSeasonResult> ValorantSeasonResults { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Valorant agent mastery levels.
        /// </summary>
        [JsonIgnore]
        public virtual List<ValorantAgentMastery> ValorantAgentMasteries { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Valorant accolade summaries (all-time and per act).
        /// </summary>
        [JsonIgnore]
        public virtual List<ValorantAccoladeSummary> ValorantAccoladeSummaries { get; set; } = null!;

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