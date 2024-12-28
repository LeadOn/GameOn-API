// <copyright file="GetUserNextPlannedMatchsQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FifaGamePlayed.Queries.GetFifaGamePlayedById
{
    using GameOn.Application.FifaGamePlayed.Commands.ConvertFifaGamePlayedToDto;
    using GameOn.Application.FifaGamePlayed.Queries.GetUserNextPlannedMatchs;
    using GameOn.Common.DTOs;
    using GameOn.Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetUserNextPlannedMatchsQueryHandler class.
    /// </summary>
    public class GetUserNextPlannedMatchsQueryHandler : IRequestHandler<GetUserNextPlannedMatchsQuery, IEnumerable<FifaGamePlayedDto>>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserNextPlannedMatchsQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public GetUserNextPlannedMatchsQueryHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<FifaGamePlayedDto>> Handle(GetUserNextPlannedMatchsQuery request, CancellationToken cancellationToken)
        {
            var gamesInDb = await this.context.FifaGamesPlayed
                .Include(x => x.Season)
                .Include(x => x.Platform)
                .Include(x => x.CreatedBy)
                .Where(x => x.IsPlayed == false && x.TeamPlayers.FirstOrDefault(x => x.PlayerId == request.PlayerId) != null)
                .Take(request.Limit)
                .ToListAsync(cancellationToken);

            var gamePlayedDtos = new List<FifaGamePlayedDto>();

            // For each game played, getting player information
            foreach (var game in gamesInDb)
            {
                gamePlayedDtos.Add(await this.mediator.Send(new ConvertFifaGamePlayedToDtoCommand { FifaGamePlayed = game }));
            }

            return gamePlayedDtos;
        }
    }
}
