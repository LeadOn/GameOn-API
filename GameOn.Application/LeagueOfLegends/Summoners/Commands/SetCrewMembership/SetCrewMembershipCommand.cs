// <copyright file="SetCrewMembershipCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Commands.SetCrewMembership
{
    using GameOn.Common.DTOs.LeagueOfLegends;
    using MediatR;

    /// <summary>
    /// SetCrewMembershipCommand class. Puts an account in or out of the crew, which decides whether it
    /// is refreshed automatically and whether its games and ranks count towards the crew's numbers.
    /// </summary>
    public class SetCrewMembershipCommand : IRequest<SetCrewMembershipResultDto?>
    {
        /// <summary>
        /// Gets or sets the ID of the account to move in or out of the crew.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the account belongs to the crew.
        /// </summary>
        public bool InCrew { get; set; }
    }
}
