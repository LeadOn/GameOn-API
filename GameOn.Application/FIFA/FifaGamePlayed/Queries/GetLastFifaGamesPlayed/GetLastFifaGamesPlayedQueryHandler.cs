// <copyright file="GetLastFifaGamesPlayedQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FIFA.FifaGamePlayed.Queries.GetLastFifaGamesPlayed
{
    using System.Collections.Generic;
    using GameOn.Application.FIFA.FifaGamePlayed.Commands.ConvertFifaGamePlayedToDto;
    using GameOn.Common.DTOs;
    using GameOn.Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetLastFifaGamesPlayedQueryHandler class.
    /// </summary>
    public class GetLastFifaGamesPlayedQueryHandler : IRequestHandler<GetLastFifaGamesPlayedQuery, List<FifaGamePlayedDto>>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLastFifaGamesPlayedQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public GetLastFifaGamesPlayedQueryHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<List<FifaGamePlayedDto>> Handle(GetLastFifaGamesPlayedQuery request, CancellationToken cancellationToken)
        {
            var games = await this.context.FifaGamesPlayed
                .Include(x => x.Season)
                .Include(x => x.CreatedBy)
                .Include(x => x.TeamPlayers)
                .Include(x => x.Platform)
                .Include(x => x.Highlights)
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
