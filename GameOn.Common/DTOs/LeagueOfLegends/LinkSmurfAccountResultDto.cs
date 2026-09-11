// <copyright file="LinkSmurfAccountResultDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    using GameOn.Domain;

    /// <summary>
    /// Result of a smurf account link attempt.
    /// </summary>
    public class LinkSmurfAccountResultDto
    {
        /// <summary>
        /// Gets or sets the outcome of the link attempt.
        /// </summary>
        public LinkSmurfAccountStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the linked smurf account, when the link succeeded.
        /// </summary>
        public Player? SmurfAccount { get; set; }

        /// <summary>
        /// Gets or sets the number of past game participations re-attached to the smurf account by the
        /// link. Games imported before the link were stored with no player (the PUUID was unknown at
        /// import time), so they are backfilled rather than left out of the player's history.
        /// </summary>
        public int BackfilledParticipations { get; set; }
    }
}
