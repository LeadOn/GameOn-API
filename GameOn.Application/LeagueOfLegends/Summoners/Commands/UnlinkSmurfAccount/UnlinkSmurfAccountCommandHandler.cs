// <copyright file="UnlinkSmurfAccountCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Summoners.Commands.UnlinkSmurfAccount
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// UnlinkSmurfAccountCommandHandler class.
    /// </summary>
    public class UnlinkSmurfAccountCommandHandler : IRequestHandler<UnlinkSmurfAccountCommand, Player?>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnlinkSmurfAccountCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public UnlinkSmurfAccountCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<Player?> Handle(UnlinkSmurfAccountCommand request, CancellationToken cancellationToken)
        {
            var smurfAccount = await this.context.Players.FirstOrDefaultAsync(x => x.Id == request.SmurfPlayerId && x.PrimaryPlayerId != null, cancellationToken);

            if (smurfAccount is null)
            {
                return null;
            }

            smurfAccount.PrimaryPlayerId = null;

            this.context.Players.Update(smurfAccount);
            await this.context.SaveChangesAsync(cancellationToken);

            return smurfAccount;
        }
    }
}
