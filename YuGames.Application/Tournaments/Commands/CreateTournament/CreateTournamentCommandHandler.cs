// <copyright file="CreateTournamentCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Tournaments.Commands.CreateTournament
{
    using MediatR;
    using YuGames.Application.Common.Interfaces;
    using YuGames.Domain;

    /// <summary>
    /// CreateTournamentCommandHandler class.
    /// </summary>
    public class CreateTournamentCommandHandler : IRequestHandler<CreateTournamentCommand, Tournament>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTournamentCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public CreateTournamentCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<Tournament> Handle(CreateTournamentCommand request, CancellationToken cancellationToken)
        {
            this.context.Tournaments.Add(request.Tournament);
            await this.context.SaveChangesAsync(cancellationToken);
            return request.Tournament;
        }
    }
}
