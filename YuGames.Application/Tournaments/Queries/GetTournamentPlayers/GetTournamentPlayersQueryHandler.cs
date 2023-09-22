// <copyright file="GetTournamentPlayersQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.Tournaments.Queries.GetTournamentPlayers
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using YuGames.Application.Common.Interfaces;
    using YuGames.Common.DTOs;

    /// <summary>
    /// GetTournamentPlayersQueryHandler class.
    /// </summary>
    public class GetTournamentPlayersQueryHandler : IRequestHandler<GetTournamentPlayersQuery, List<TournamentPlayerDto>>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTournamentPlayersQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetTournamentPlayersQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<List<TournamentPlayerDto>> Handle(GetTournamentPlayersQuery request, CancellationToken cancellationToken)
        {
            return await this.context.TournamentPlayers
                .Include(x => x.FifaTeam)
                .Include(x => x.Player)
                .Where(x => x.TournamentId == request.TournamentId)
                .Select(x => new TournamentPlayerDto
                            {
                                JoinedAt = x.JoinedAt,
                                Player = x.Player,
                                Team = x.FifaTeam,
                            })
                .ToListAsync(cancellationToken);
        }
    }
}
