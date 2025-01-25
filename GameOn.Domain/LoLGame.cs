// <copyright file="LoLGame.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// LoLGame class.
    /// </summary>
    public class LoLGame
    {
        /// <summary>
        /// Gets or sets Match ID.
        /// </summary>
        public long? GameId { get; set; }

        /// <summary>
        /// Gets or sets Match ID.
        /// </summary>
        public string MatchId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets End of Game Result.
        /// </summary>
        public string? EndOfGameResult { get; set; }

        /// <summary>
        /// Gets or sets game version.
        /// </summary>
        public string? GameVersion { get; set; }

        /// <summary>
        /// Gets or sets Retrieved on Date.
        /// </summary>
        public DateTime RetrievedOn { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets Game start DateTime.
        /// </summary>
        public DateTime GameStart { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets Game end DateTime.
        /// </summary>
        public DateTime GameEnd { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets Winning team ID.
        /// </summary>
        public int? WinningTeamId { get; set; }

        /// <summary>
        /// Gets or sets Queue Type.
        /// </summary>
        public string QueueType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets League of Legends Game Participants.
        /// </summary>
        public virtual List<LoLGameParticipant> LeagueOfLegendsGameParticipants { get; set; } = null!;
    }
}