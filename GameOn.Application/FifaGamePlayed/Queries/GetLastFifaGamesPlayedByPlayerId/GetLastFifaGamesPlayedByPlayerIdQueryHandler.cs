// <copyright file="GetLastFifaGamesPlayedByPlayerIdQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FifaGamePlayed.Queries.GetLastFifaGamesPlayedByPlayerId
{
    using System.Collections.Generic;
    using GameOn.Common.Interfaces;
    using GameOn.Application.FifaGamePlayed.Commands.ConvertFifaGamePlayedToDto;
    using GameOn.Common.DTOs;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetLastFifaGamesPlayedByPlayerIdQueryHandler class.
    /// </summary>
    public class GetLastFifaGamesPlayedByPlayerIdQueryHandler : IRequestHandler<GetLastFifaGamesPlayedByPlayerIdQuery, List<FifaGamePlayedDto>>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLastFifaGamesPlayedByPlayerIdQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public GetLastFifaGamesPlayedByPlayerIdQueryHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<List<FifaGamePlayedDto>> Handle(GetLastFifaGamesPlayedByPlayerIdQuery request, CancellationToken cancellationToken)
        {
            var games = await this.context.FifaGamesPlayed
                .Include(x => x.Season)
                .Include(x => x.CreatedBy)
                .Include(x => x.TeamPlayers)
                .Include(x => x.Platform)
                .Include(x => x.Highlights)
                .Where(x => x.TeamPlayers.FirstOrDefault(
                    x => x.PlayerId == request.PlayerId) != null
                            && x.IsPlayed == true)
                .OrderByDescending(x => x.PlayedOn)
                .Take(request.Limit)
                .ToListAsync(cancellationToken);

            var dtos = new List<FifaGamePlayedDto>();

            foreach (var game in games)
            {
                dtos.Add(await this.mediator.Send(new ConvertFifaGamePlayedToDtoCommand { FifaGamePlayed = game }));
            }

            return dtos;
        }
    }
}
