// <copyright file="LoLGlobalStatsDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    /// <summary>
    /// LoLGlobalStatsDto class. Global fun stats recap, all players included.
    /// </summary>
    public class LoLGlobalStatsDto
    {
        /// <summary>
        /// Gets or sets the total number of games analyzed.
        /// </summary>
        public int TotalGamesAnalyzed { get; set; }

        /// <summary>
        /// Gets or sets the total number of tracked players.
        /// </summary>
        public int TotalPlayersTracked { get; set; }

        /// <summary>
        /// Gets or sets the Ping Machine award: most pings sent in a single game (all-in, assist me and command pings).
        /// </summary>
        public LoLFunStatDto? PingMachine { get; set; }

        /// <summary>
        /// Gets or sets the Biggest Inter award: most deaths in a single game.
        /// </summary>
        public LoLFunStatDto? BiggestInter { get; set; }

        /// <summary>
        /// Gets or sets the Highest Bounty award: highest bounty level reached in a single game.
        /// </summary>
        public LoLFunStatDto? HighestBounty { get; set; }

        /// <summary>
        /// Gets or sets the Shopping Addict award: most consumables purchased in a single game.
        /// </summary>
        public LoLFunStatDto? ShoppingAddict { get; set; }

        /// <summary>
        /// Gets or sets the One-Trick Pony award: highest share of games played on a single champion.
        /// </summary>
        public LoLFunStatDto? OneTrickPony { get; set; }

        /// <summary>
        /// Gets or sets the Crowd Control Master award: most time enemies spent controlled by them in a single game.
        /// </summary>
        public LoLFunStatDto? CrowdControlMaster { get; set; }

        /// <summary>
        /// Gets or sets the Punching Ball award: most damage taken per minute in a single game.
        /// </summary>
        public LoLFunStatDto? PunchingBall { get; set; }

        /// <summary>
        /// Gets or sets the Pacifist award: least damage dealt to champions in a full game.
        /// </summary>
        public LoLFunStatDto? Pacifist { get; set; }

        /// <summary>
        /// Gets or sets the Squirrel award: most unspent gold held at the end of a game.
        /// </summary>
        public LoLFunStatDto? Squirrel { get; set; }

        /// <summary>
        /// Gets or sets the Jungle Thief award: most jungle monsters killed in a game without being the team's jungler.
        /// </summary>
        public LoLFunStatDto? JungleThief { get; set; }

        /// <summary>
        /// Gets or sets the Comeback King award: most games won while the team was behind in gold at 20 minutes.
        /// </summary>
        public LoLFunStatDto? ComebackKing { get; set; }

        /// <summary>
        /// Gets or sets the Night Owl award: most games played between midnight and 6 AM.
        /// </summary>
        public LoLFunStatDto? NightOwl { get; set; }

        /// <summary>
        /// Gets or sets the Longest Loss Streak award: most consecutive losses.
        /// </summary>
        public LoLFunStatDto? LongestLossStreak { get; set; }

        /// <summary>
        /// Gets or sets the Emotional Elevator award: biggest LP drop between two rank snapshots.
        /// </summary>
        public LoLFunStatDto? EmotionalElevator { get; set; }

        /// <summary>
        /// Gets or sets the Cursed Patch award: game version with the worst win rate across all tracked players.
        /// </summary>
        public LoLFunStatDto? CursedPatch { get; set; }
    }
}
