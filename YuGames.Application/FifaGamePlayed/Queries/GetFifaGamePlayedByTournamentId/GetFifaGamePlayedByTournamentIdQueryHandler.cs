// <copyright file="GetFifaGamePlayedByTournamentIdQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.FifaGamePlayed.Queries.GetFifaGamePlayedByTournamentId
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using YuGames.Application.Common.Interfaces;
    using YuGames.Domain;

    /// <summary>
    /// GetFifaGamePlayedByTournamentIdQueryHandler class.
    /// </summary>
    public class GetFifaGamePlayedByTournamentIdQueryHandler : IRequestHandler<GetFifaGamePlayedByTournamentIdQuery, IEnumerable<FifaGamePlayed>>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFifaGamePlayedByTournamentIdQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetFifaGamePlayedByTournamentIdQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<FifaGamePlayed>> Handle(GetFifaGamePlayedByTournamentIdQuery request, CancellationToken cancellationToken)
        {
            return await this.context.Tournaments.ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
