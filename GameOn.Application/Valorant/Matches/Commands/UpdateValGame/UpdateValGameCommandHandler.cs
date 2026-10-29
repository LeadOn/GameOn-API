// <copyright file="UpdateValGameCommandHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Matches.Commands.UpdateValGame
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Application.Valorant.Matches.Commands.UpsertValorantMatches;
    using GameOn.Common.Interfaces;
    using GameOn.Domain;
    using GameOn.External.HenrikDev.Interfaces;
    using GameOn.External.HenrikDev.Models.DTOs;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// <see cref="UpdateValGameCommand"/> handler.
    /// </summary>
    public class UpdateValGameCommandHandler : IRequestHandler<UpdateValGameCommand, ValGame?>
    {
        private readonly IApplicationDbContext context;
        private readonly IValorantMatchService valorantMatchService;
        private readonly ISender mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateValGameCommandHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="valorantMatchService">Valorant Match Service, injected.</param>
        /// <param name="mediator">MediatR sender, injected.</param>
        public UpdateValGameCommandHandler(
            IApplicationDbContext context,
            IValorantMatchService valorantMatchService,
            ISender mediator)
        {
            this.context = context;
            this.valorantMatchService = valorantMatchService;
            this.mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<ValGame?> Handle(UpdateValGameCommand request, CancellationToken cancellationToken)
        {
            var existingRegion = await this.context.ValorantGames
                .Where(x => x.MatchId == request.MatchId)
                .Select(x => x.Region)
                .FirstOrDefaultAsync(cancellationToken);

            var region = !string.IsNullOrWhiteSpace(request.Region)
                ? request.Region
                : !string.IsNullOrWhiteSpace(existingRegion)
                    ? existingRegion
                    : "eu";

            var matchFromApi = await this.valorantMatchService.GetMatchById(region, request.MatchId, cancellationToken);

            if (matchFromApi is null)
            {
                return null;
            }

            var persisted = await this.mediator.Send(
                new UpsertValorantMatchesCommand
                {
                    Matches = new List<ValorantMatchDto> { matchFromApi },
                    OverwriteExisting = true,
                },
                cancellationToken);

            return persisted.FirstOrDefault();
        }
    }
}
