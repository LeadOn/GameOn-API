// <copyright file="SearchFifaGamesPlayedQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FifaGamePlayed.Queries.SearchFifaGamesPlayed
{
    using System;
    using System.Collections.Generic;
    using GameOn.Application.Common.Interfaces;
    using GameOn.Application.FifaGamePlayed.Commands.ConvertFifaGamePlayedToDto;
    using GameOn.Application.Platforms.Queries.GetPlatformById;
    using GameOn.Common.DTOs;
    using GameOn.Domain;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// SearchFifaGamesPlayedQueryHandler class.
    /// </summary>
    public class SearchFifaGamesPlayedQueryHandler : IRequestHandler<SearchFifaGamesPlayedQuery, List<FifaGamePlayedDto>>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchFifaGamesPlayedQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public SearchFifaGamesPlayedQueryHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<List<FifaGamePlayedDto>> Handle(SearchFifaGamesPlayedQuery request, CancellationToken cancellationToken)
        {
            request.Limit ??= 10;

            if (request.Limit is not null && request.Limit > 50)
            {
                request.Limit = 50;
            }

            if (request.PlatformId is not null)
            {
                // checking if platform exists in database
                var platformInDb = await this.mediator.Send(new GetPlatformByIdQuery { PlatformId = (int)request.PlatformId });

                if (platformInDb is null)
                {
                    request.PlatformId = null;
                }
            }

            if (request.StartDate is null)
            {
                request.StartDate = DateTime.UtcNow.AddYears(-5);
            }

            if (request.EndDate is null)
            {
                request.EndDate = DateTime.UtcNow.AddDays(1);
            }

            IEnumerable<FifaGamePlayed> gamesInDb = new List<FifaGamePlayed>();

            if (request.PlatformId is null && request.Limit is not null)
            {
                gamesInDb = await this.context.FifaGamesPlayed
                    .Include(x => x.Season)
                    .Include(x => x.Highlights)
                    .Include(x => x.Platform)
                    .Include(x => x.CreatedBy)
                    .Where(x => x.PlayedOn >= request.StartDate && x.PlayedOn <= request.EndDate)
                    .OrderByDescending(x => x.PlayedOn)
                    .Take((int)request.Limit)
                    .ToListAsync(cancellationToken);
            }
            else if (request.PlatformId is not null && request.Limit is not null)
            {
                gamesInDb = await this.context.FifaGamesPlayed
                    .Include(x => x.Season)
                    .Include(x => x.Highlights)
                    .Include(x => x.Platform)
                    .Include(x => x.CreatedBy)
                    .Where(x => x.PlayedOn >= request.StartDate && x.PlayedOn <= request.EndDate && x.PlatformId == (int)request.PlatformId)
                    .OrderByDescending(x => x.PlayedOn)
                    .Take((int)request.Limit)
                    .ToListAsync(cancellationToken);
            }

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
