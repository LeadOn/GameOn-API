// <copyright file="ValGameTeam.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Per-team summary for a Valorant match.
    /// </summary>
    public class ValGameTeam
    {
        /// <summary>
        /// Gets or sets the primary key ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the parent match UUID.
        /// </summary>
        public string MatchId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the team identifier ("Red", "Blue", or player PUUID in Deathmatch).
        /// </summary>
        public string TeamId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the team won the match.
        /// </summary>
        public bool Won { get; set; }

        /// <summary>
        /// Gets or sets the number of rounds won by the team.
        /// </summary>
        public int RoundsWon { get; set; }

        /// <summary>
        /// Gets or sets the number of rounds lost by the team.
        /// </summary>
        public int RoundsLost { get; set; }

        /// <summary>
        /// Gets or sets the team placement (1 for the winner).
        /// </summary>
        public int? Placement { get; set; }

        /// <summary>
        /// Gets or sets the PUUID of the team MVP elected by Riot.
        /// </summary>
        public string? RiotMvpPuuid { get; set; }

        /// <summary>
        /// Gets or sets the Premier roster UUID, on Premier matches only.
        /// </summary>
        public string? PremierRosterId { get; set; }

        /// <summary>
        /// Gets or sets the Premier team name, on Premier matches only.
        /// </summary>
        public string? PremierTeamName { get; set; }

        /// <summary>
        /// Gets or sets the Premier team tag, on Premier matches only.
        /// </summary>
        public string? PremierTeamTag { get; set; }

        /// <summary>
        /// Gets or sets the parent match.
        /// </summary>
        [JsonIgnore]
        public virtual ValGame Game { get; set; } = null!;
    }
}
