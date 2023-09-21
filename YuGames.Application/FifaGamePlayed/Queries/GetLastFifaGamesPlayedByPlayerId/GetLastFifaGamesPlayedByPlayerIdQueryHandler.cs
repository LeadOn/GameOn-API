﻿// <copyright file="GetLastFifaGamesPlayedByPlayerIdQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.FifaGamePlayed.Queries.GetLastFifaGamesPlayedByPlayerId
{
    using System.Collections.Generic;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using YuGames.Application.Common.Interfaces;
    using YuGames.Application.FifaGamePlayed.Commands.ConvertFifaGamePlayedToDto;
    using YuGames.Common.DTOs;

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
                .Take(request.Limit)
                .ToListAsync(cancellationToken);

            var dtos = new List<FifaGamePlayedDto>();

            foreach (var game in games)
            {
                dtos.Add(await this.mediator.Send(new ConvertFifaGamePlayedToDtoCommand { FifaGamePlayed = game }));
            }

            return dtos.OrderByDescending(x => x.PlayedOn).ToList();
        }
    }
}
