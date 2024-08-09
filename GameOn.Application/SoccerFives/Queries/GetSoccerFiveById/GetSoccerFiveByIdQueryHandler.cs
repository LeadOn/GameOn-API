// <copyright file="GetSoccerFiveByIdQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.TournamentPlayers.Queries.GetSoccerFiveById
{
    using GameOn.Application.Common.Interfaces;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetSoccerFiveByIdQueryHandler class.
    /// </summary>
    public class GetSoccerFiveByIdQueryHandler : IRequestHandler<GetSoccerFiveByIdQuery, SoccerFive?>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSoccerFiveByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetSoccerFiveByIdQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<SoccerFive?> Handle(GetSoccerFiveByIdQuery request, CancellationToken cancellationToken)
        {
            return await this.context.SoccerFives.Include(x => x.CreatedBy).Include(x => x.VotesChoices).ThenInclude(x => x.Answers).ThenInclude(x => x.Player).FirstOrDefaultAsync(x => x.Id == request.SoccerFiveId, cancellationToken);
        }
    }
}
