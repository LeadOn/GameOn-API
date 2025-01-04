// <copyright file="CreateTournamentCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.Tournaments.Commands.CreateTournament
{
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;

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
            context.Tournaments.Add(request.Tournament);
            await context.SaveChangesAsync(cancellationToken);
            return request.Tournament;
        }
    }
}
