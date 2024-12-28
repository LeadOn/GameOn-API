// <copyright file="GetSoccerFiveByIdQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.TournamentPlayers.Queries.GetSoccerFiveById
{
    using GameOn.Common.DTOs;
    using GameOn.Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// GetSoccerFiveByIdQueryHandler class.
    /// </summary>
    public class GetSoccerFiveByIdQueryHandler : IRequestHandler<GetSoccerFiveByIdQuery, SoccerFiveDto?>
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
        public async Task<SoccerFiveDto?> Handle(GetSoccerFiveByIdQuery request, CancellationToken cancellationToken)
        {
#pragma warning disable CS8620 // Impossible d'utiliser l'argument pour le paramètre, car il existe des différences dans l'acceptation des valeurs null par les types référence.
            var five = await this.context.SoccerFives.Include(x => x.CreatedBy).Include(x => x.VotesChoices).ThenInclude(x => x.Answers).ThenInclude(x => x.Player).FirstOrDefaultAsync(x => x.Id == request.SoccerFiveId, cancellationToken);
#pragma warning restore CS8620 // Impossible d'utiliser l'argument pour le paramètre, car il existe des différences dans l'acceptation des valeurs null par les types référence.

            if (five == null)
            {
                return null;
            }

            return new SoccerFiveDto(five);
        }
    }
}
