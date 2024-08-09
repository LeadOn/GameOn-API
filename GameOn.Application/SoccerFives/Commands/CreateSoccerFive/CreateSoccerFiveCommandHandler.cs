// <copyright file="CreateSoccerFiveCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.SoccerFives.Commands.CreateSoccerFive
{
    using GameOn.Application.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;

    /// <summary>
    /// CreateTournamentCommandHandler class.
    /// </summary>
    public class CreateSoccerFiveCommandHandler : IRequestHandler<CreateSoccerFiveCommand, SoccerFive>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSoccerFiveCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public CreateSoccerFiveCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<SoccerFive> Handle(CreateSoccerFiveCommand request, CancellationToken cancellationToken)
        {
            this.context.SoccerFives.Add(request.SoccerFive);
            await this.context.SaveChangesAsync(cancellationToken);
            return request.SoccerFive;
        }
    }
}
