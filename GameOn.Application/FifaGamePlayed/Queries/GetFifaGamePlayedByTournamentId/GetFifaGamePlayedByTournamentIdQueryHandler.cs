// <copyright file="GetFifaGamePlayedByTournamentIdQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FifaGamePlayed.Queries.GetFifaGamePlayedByTournamentId
{
    using GameOn.Common.Interfaces;
    using GameOn.Common.DTOs;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetFifaGamePlayedByTournamentIdQueryHandler class.
    /// </summary>
    public class GetFifaGamePlayedByTournamentIdQueryHandler : IRequestHandler<GetFifaGamePlayedByTournamentIdQuery, IEnumerable<FifaGamePlayedDto>>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFifaGamePlayedByTournamentIdQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public GetFifaGamePlayedByTournamentIdQueryHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<FifaGamePlayedDto>> Handle(GetFifaGamePlayedByTournamentIdQuery request, CancellationToken cancellationToken)
        {
            var gamesPlayedDto = new List<FifaGamePlayedDto>();

            // First, getting last games (from GamePlayed table)
            var gamesPlayed = await this.context.FifaGamesPlayed
                .Include(x => x.Platform)
                .Include(x => x.Season)
                .Include(x => x.Highlights)
                .Include(x => x.Platform)
                .Where(x => x.TournamentId == request.TournamentId && x.IsPlayed == request.IsPlayed)
                .ToListAsync(cancellationToken);

            // For each game played, getting player information
            foreach (var game in gamesPlayed)
            {
                gamesPlayedDto.Add(await this.mediator.Send(new Commands.ConvertFifaGamePlayedToDto.ConvertFifaGamePlayedToDtoCommand { FifaGamePlayed = game }));
            }

            return gamesPlayedDto.OrderByDescending(x => x.PlayedOn).OrderByDescending(x => x.IsPlayed);
        }
    }
}
