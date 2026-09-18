// <copyright file="SetCrewMembershipCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Commands.SetCrewMembership
{
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// SetCrewMembershipCommandHandler class.
    /// </summary>
    public class SetCrewMembershipCommandHandler : IRequestHandler<SetCrewMembershipCommand, SetCrewMembershipResultDto?>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetCrewMembershipCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public SetCrewMembershipCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<SetCrewMembershipResultDto?> Handle(SetCrewMembershipCommand request, CancellationToken cancellationToken)
        {
            var playerInDb = await this.context.Players.FirstOrDefaultAsync(x => x.Id == request.PlayerId, cancellationToken);

            if (playerInDb is null)
            {
                return null;
            }

            playerInDb.InCrew = request.InCrew;
            this.context.Players.Update(playerInDb);

            // Saved before the cascade below, which writes straight to the database: if this save fails,
            // nothing should have demoted the smurfs already.
            await this.context.SaveChangesAsync(cancellationToken);

            var affectedSmurfAccounts = 0;

            // A smurf is the same person as the account it hangs off, so it cannot stay in the crew once
            // its owner has left: it would keep feeding the very stats the owner was pulled out of. The
            // opposite direction is deliberately not mirrored -- see SetCrewMembershipResultDto.
            if (!request.InCrew)
            {
                affectedSmurfAccounts = await this.context.Players
                    .Where(x => x.PrimaryPlayerId == playerInDb.Id && x.InCrew)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.InCrew, false), cancellationToken);
            }

            return new SetCrewMembershipResultDto
            {
                Account = playerInDb,
                AffectedSmurfAccounts = affectedSmurfAccounts,
            };
        }
    }
}
