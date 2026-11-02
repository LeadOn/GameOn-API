// <copyright file="ValPerformanceLine.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Matches.Services
{
    using System;
    using System.Linq.Expressions;
    using GameOn.Domain;

    /// <summary>
    /// One tracked participation, flattened to what the Valorant stats and awards read. Loaded through
    /// <see cref="Projection"/> so that every handler fetches the same columns the same way.
    /// </summary>
    public class ValPerformanceLine
    {
        /// <summary>
        /// Gets the projection from a participation to a line, translated to SQL by EF.
        /// Only valid on participations linked to a GameOn player (<c>PlayerId != null</c>).
        /// </summary>
        public static Expression<Func<ValGameParticipant, ValPerformanceLine>> Projection { get; } = x => new ValPerformanceLine
        {
            PlayerId = x.PlayerId!.Value,
            MatchId = x.MatchId,
            TeamId = x.TeamId,
            PartyId = x.PartyId,
            Win = x.Win,
            IsDraw = !x.Win && x.Game.WinningTeamId == null,
            GameStart = x.Game.GameStart,
            GameLengthMs = x.Game.GameLengthMs,
            MapId = x.Game.MapId,
            MapName = x.Game.MapName,
            AgentId = x.AgentId,
            AgentName = x.AgentName,
            Score = x.Score,
            Kills = x.Kills,
            Deaths = x.Deaths,
            Assists = x.Assists,
            Headshots = x.Headshots,
            Bodyshots = x.Bodyshots,
            Legshots = x.Legshots,
            DamageDealt = x.DamageDealt,
            HasStats = x.Stats != null,
            RoundsPlayed = x.Stats != null ? x.Stats.RoundsPlayed : 0,
            Acs = x.Stats != null ? x.Stats.Acs : 0,
            KastPercent = x.Stats != null ? x.Stats.KastPercent : 0,
            Rating = x.Stats != null ? x.Stats.Rating : 0,
            FirstKills = x.Stats != null ? x.Stats.FirstKills : 0,
            FirstDeaths = x.Stats != null ? x.Stats.FirstDeaths : 0,
            SpikePlants = x.Stats != null ? x.Stats.SpikePlants : 0,
            SpikeDefuses = x.Stats != null ? x.Stats.SpikeDefuses : 0,
            KnifeKills = x.Stats != null ? x.Stats.KnifeKills : 0,
            AttackRounds = x.Stats != null ? x.Stats.AttackRounds : null,
            AttackRoundsWon = x.Stats != null ? x.Stats.AttackRoundsWon : null,
            AttackKills = x.Stats != null ? x.Stats.AttackKills : null,
            AttackDeaths = x.Stats != null ? x.Stats.AttackDeaths : null,
            AttackScore = x.Stats != null ? x.Stats.AttackScore : null,
            DefenseRounds = x.Stats != null ? x.Stats.DefenseRounds : null,
            DefenseRoundsWon = x.Stats != null ? x.Stats.DefenseRoundsWon : null,
            DefenseKills = x.Stats != null ? x.Stats.DefenseKills : null,
            DefenseDeaths = x.Stats != null ? x.Stats.DefenseDeaths : null,
            DefenseScore = x.Stats != null ? x.Stats.DefenseScore : null,
            PistolRounds = x.Stats != null ? x.Stats.PistolRounds : null,
            PistolRoundsWon = x.Stats != null ? x.Stats.PistolRoundsWon : null,
            EcoRounds = x.Stats != null ? x.Stats.EcoRounds : null,
            EcoRoundsWon = x.Stats != null ? x.Stats.EcoRoundsWon : null,
            ForceBuyRounds = x.Stats != null ? x.Stats.ForceBuyRounds : null,
            ForceBuyRoundsWon = x.Stats != null ? x.Stats.ForceBuyRoundsWon : null,
            FullBuyRounds = x.Stats != null ? x.Stats.FullBuyRounds : null,
            FullBuyRoundsWon = x.Stats != null ? x.Stats.FullBuyRoundsWon : null,
            TradeKills = x.Stats != null ? x.Stats.TradeKills : 0,
            TradedDeaths = x.Stats != null ? x.Stats.TradedDeaths : 0,
            ClutchAttempts = x.Stats != null ? x.Stats.ClutchAttempts : 0,
            Clutch1v1Won = x.Stats != null ? x.Stats.Clutch1v1Won : 0,
            Clutch1v2Won = x.Stats != null ? x.Stats.Clutch1v2Won : 0,
            Clutch1v3Won = x.Stats != null ? x.Stats.Clutch1v3Won : 0,
            Clutch1v4Won = x.Stats != null ? x.Stats.Clutch1v4Won : 0,
            Clutch1v5Won = x.Stats != null ? x.Stats.Clutch1v5Won : 0,
            LongestKillDistanceMeters = x.Stats != null ? x.Stats.LongestKillDistanceMeters : null,
            RiotPerformanceScore = x.Performance != null ? x.Performance.Score : null,
            Puuid = x.Puuid,
        };

        /// <summary>Gets or sets the GameOn player ID.</summary>
        public int PlayerId { get; set; }

        /// <summary>Gets or sets the match UUID.</summary>
        public string MatchId { get; set; } = string.Empty;

        /// <summary>Gets or sets the team ("Red" or "Blue").</summary>
        public string? TeamId { get; set; }

        /// <summary>Gets or sets the party UUID, shared by premade teammates.</summary>
        public string? PartyId { get; set; }

        /// <summary>Gets or sets a value indicating whether the participant won.</summary>
        public bool Win { get; set; }

        /// <summary>Gets or sets a value indicating whether the game ended without a winner.</summary>
        public bool IsDraw { get; set; }

        /// <summary>Gets or sets the game start, in UTC.</summary>
        public DateTime GameStart { get; set; }

        /// <summary>Gets or sets the game duration, in milliseconds.</summary>
        public long GameLengthMs { get; set; }

        /// <summary>Gets or sets the map UUID.</summary>
        public string? MapId { get; set; }

        /// <summary>Gets or sets the map display name.</summary>
        public string? MapName { get; set; }

        /// <summary>Gets or sets the agent UUID.</summary>
        public string? AgentId { get; set; }

        /// <summary>Gets or sets the agent display name.</summary>
        public string? AgentName { get; set; }

        /// <summary>Gets or sets the combat score of the whole game.</summary>
        public int Score { get; set; }

        /// <summary>Gets or sets the kills.</summary>
        public int Kills { get; set; }

        /// <summary>Gets or sets the deaths.</summary>
        public int Deaths { get; set; }

        /// <summary>Gets or sets the assists.</summary>
        public int Assists { get; set; }

        /// <summary>Gets or sets the shots that hit the head.</summary>
        public int Headshots { get; set; }

        /// <summary>Gets or sets the shots that hit the body.</summary>
        public int Bodyshots { get; set; }

        /// <summary>Gets or sets the shots that hit the legs.</summary>
        public int Legshots { get; set; }

        /// <summary>Gets or sets the damage dealt over the game.</summary>
        public int DamageDealt { get; set; }

        /// <summary>Gets or sets a value indicating whether derived stats exist for this participation.</summary>
        public bool HasStats { get; set; }

        /// <summary>Gets or sets the rounds played. 0 without derived stats.</summary>
        public int RoundsPlayed { get; set; }

        /// <summary>Gets or sets the ACS of this game. 0 without derived stats.</summary>
        public double Acs { get; set; }

        /// <summary>Gets or sets the KAST percentage of this game. 0 without derived stats.</summary>
        public double KastPercent { get; set; }

        /// <summary>Gets or sets the GameOn rating of this game. 0 without derived stats.</summary>
        public double Rating { get; set; }

        /// <summary>Gets or sets the first kills. 0 without derived stats.</summary>
        public int FirstKills { get; set; }

        /// <summary>Gets or sets the first deaths. 0 without derived stats.</summary>
        public int FirstDeaths { get; set; }

        /// <summary>Gets or sets the spike plants. 0 without derived stats.</summary>
        public int SpikePlants { get; set; }

        /// <summary>Gets or sets the spike defuses. 0 without derived stats.</summary>
        public int SpikeDefuses { get; set; }

        /// <summary>Gets or sets the melee kills. 0 without derived stats.</summary>
        public int KnifeKills { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ValGameParticipantStat.AttackRounds"/>.
        /// </summary>
        public int? AttackRounds { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ValGameParticipantStat.AttackRoundsWon"/>.
        /// </summary>
        public int? AttackRoundsWon { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ValGameParticipantStat.AttackKills"/>.
        /// </summary>
        public int? AttackKills { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ValGameParticipantStat.AttackDeaths"/>.
        /// </summary>
        public int? AttackDeaths { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ValGameParticipantStat.AttackScore"/>.
        /// </summary>
        public int? AttackScore { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ValGameParticipantStat.DefenseRounds"/>.
        /// </summary>
        public int? DefenseRounds { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ValGameParticipantStat.DefenseRoundsWon"/>.
        /// </summary>
        public int? DefenseRoundsWon { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ValGameParticipantStat.DefenseKills"/>.
        /// </summary>
        public int? DefenseKills { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ValGameParticipantStat.DefenseDeaths"/>.
        /// </summary>
        public int? DefenseDeaths { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ValGameParticipantStat.DefenseScore"/>.
        /// </summary>
        public int? DefenseScore { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ValGameParticipantStat.PistolRounds"/>.
        /// </summary>
        public int? PistolRounds { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ValGameParticipantStat.PistolRoundsWon"/>.
        /// </summary>
        public int? PistolRoundsWon { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ValGameParticipantStat.EcoRounds"/>.
        /// </summary>
        public int? EcoRounds { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ValGameParticipantStat.EcoRoundsWon"/>.
        /// </summary>
        public int? EcoRoundsWon { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ValGameParticipantStat.ForceBuyRounds"/>.
        /// </summary>
        public int? ForceBuyRounds { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ValGameParticipantStat.ForceBuyRoundsWon"/>.
        /// </summary>
        public int? ForceBuyRoundsWon { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ValGameParticipantStat.FullBuyRounds"/>.
        /// </summary>
        public int? FullBuyRounds { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ValGameParticipantStat.FullBuyRoundsWon"/>.
        /// </summary>
        public int? FullBuyRoundsWon { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ValGameParticipantStat.TradeKills"/>.
        /// </summary>
        public int TradeKills { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ValGameParticipantStat.TradedDeaths"/>.
        /// </summary>
        public int TradedDeaths { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ValGameParticipantStat.ClutchAttempts"/>.
        /// </summary>
        public int ClutchAttempts { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ValGameParticipantStat.Clutch1v1Won"/>.
        /// </summary>
        public int Clutch1v1Won { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ValGameParticipantStat.Clutch1v2Won"/>.
        /// </summary>
        public int Clutch1v2Won { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ValGameParticipantStat.Clutch1v3Won"/>.
        /// </summary>
        public int Clutch1v3Won { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ValGameParticipantStat.Clutch1v4Won"/>.
        /// </summary>
        public int Clutch1v4Won { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ValGameParticipantStat.Clutch1v5Won"/>.
        /// </summary>
        public int Clutch1v5Won { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ValGameParticipantStat.LongestKillDistanceMeters"/>.
        /// </summary>
        public double? LongestKillDistanceMeters { get; set; }

        /// <summary>
        /// Gets or sets Riot's own performance score, null when Riot did not rate the game.
        /// </summary>
        public double? RiotPerformanceScore { get; set; }

        /// <summary>
        /// Gets or sets the account's Valorant PUUID in the game.
        /// </summary>
        public string Puuid { get; set; } = string.Empty;

        /// <summary>Gets the shots that hit, head, body and legs combined.</summary>
        public int ShotsHit => this.Headshots + this.Bodyshots + this.Legshots;
    }
}
