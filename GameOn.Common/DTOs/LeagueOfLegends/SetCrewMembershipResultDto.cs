// <copyright file="SetCrewMembershipResultDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    using GameOn.Domain;

    /// <summary>
    /// Result of a crew membership change.
    /// </summary>
    public class SetCrewMembershipResultDto
    {
        /// <summary>
        /// Gets or sets the account whose membership changed.
        /// </summary>
        public Player Account { get; set; } = null!;

        /// <summary>
        /// Gets or sets the number of smurf accounts dragged along by the change. Only ever non-zero
        /// when a primary account leaves the crew: its smurfs leave with it, since they are the same
        /// person and would otherwise keep feeding the crew's stats on their own. Bringing the account
        /// back into the crew does not bring them back -- an admin may have excluded a given smurf on
        /// purpose, and resurrecting it silently would undo that.
        /// </summary>
        public int AffectedSmurfAccounts { get; set; }
    }
}
