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

        /// <summary>Gets the shots that hit, head, body and legs combined.</summary>
        public int ShotsHit => this.Headshots + this.Bodyshots + this.Legshots;
    }
}
