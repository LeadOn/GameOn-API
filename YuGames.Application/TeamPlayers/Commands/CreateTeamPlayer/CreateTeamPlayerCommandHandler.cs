// <copyright file="CreateTeamPlayerCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.TeamPlayers.Commands.CreateTeamPlayer
{
    using MediatR;
    using YuGames.Application.Common.Interfaces;
    using YuGames.Domain;

    /// <summary>
    /// SearchTeamPlayerQueryHandler class.
    /// </summary>
    public class CreateTeamPlayerCommandHandler : IRequestHandler<CreateTeamPlayerCommand, FifaTeamPlayer>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTeamPlayerCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public CreateTeamPlayerCommandHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<FifaTeamPlayer> Handle(CreateTeamPlayerCommand request, CancellationToken cancellationToken)
        {
            this.context.FifaTeamPlayers.Add(request.TeamPlayer);
            await this.context.SaveChangesAsync(cancellationToken);
            return request.TeamPlayer;
        }
    }
}
