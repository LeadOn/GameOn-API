// <copyright file="GetFifaGamePlayedByIdQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.FifaGamePlayed.Queries.GetFifaGamePlayedById
{
    using GameOn.Application.FifaGamePlayed.Commands.ConvertFifaGamePlayedToDto;
    using GameOn.Common.DTOs;
    using GameOn.Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetFifaGamePlayedByIdQueryHandler class.
    /// </summary>
    public class GetFifaGamePlayedByIdQueryHandler : IRequestHandler<GetFifaGamePlayedByIdQuery, FifaGamePlayedDto?>
    {
        private readonly IApplicationDbContext context;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFifaGamePlayedByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="mediator">Mediator, injected.</param>
        public GetFifaGamePlayedByIdQueryHandler(IApplicationDbContext context, ISender mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<FifaGamePlayedDto?> Handle(GetFifaGamePlayedByIdQuery request, CancellationToken cancellationToken)
        {
            var gameInDb = await this.context.FifaGamesPlayed
                .Include(x => x.Season)
                .Include(x => x.Highlights)
                .Include(x => x.Platform)
                .Include(x => x.CreatedBy)
                .FirstOrDefaultAsync(x => x.Id == request.FifaGamePlayedId, cancellationToken);

            if (gameInDb is null)
            {
                return null;
            }

            return await this.mediator.Send(new ConvertFifaGamePlayedToDtoCommand { FifaGamePlayed = gameInDb });
        }
    }
}
