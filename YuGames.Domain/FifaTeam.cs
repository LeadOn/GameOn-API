// <copyright file="FifaTeam.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Domain
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// FifaTeam class.
    /// </summary>
    public class FifaTeam
    {
        /// <summary>
        /// Gets or sets ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; } = "UNKNOWN NAME";

        /// <summary>
        /// Gets or sets Games played Team 1.
        /// </summary>
        [JsonIgnore]
        public virtual List<FifaGamePlayed> GamesPlayedTeam1 { get; set; } = null!;

        /// <summary>
        /// Gets or sets Games played Team 2.
        /// </summary>
        [JsonIgnore]
        public virtual List<FifaGamePlayed> GamesPlayedTeam2 { get; set; } = null!;

        /// <summary>
        /// Gets or sets TournamentPlayer.
        /// </summary>
        [JsonIgnore]
        public virtual List<TournamentPlayer> TournamentPlayers { get; set; } = null!;
    }
}
