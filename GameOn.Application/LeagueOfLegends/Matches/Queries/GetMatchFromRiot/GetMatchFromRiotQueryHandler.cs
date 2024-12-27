// <copyright file="GetMatchFromRiotQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Matches.Queries.GetMatchFromRiot
{
    using GameOn.Application.Common.Interfaces;
    using GameOn.External.RiotGames.Interfaces;
    using GameOn.External.RiotGames.Models.DTOs;
    using MediatR;

    /// <summary>
    /// GetMatchFromRiotQueryHandler class.
    /// </summary>
    public class GetMatchFromRiotQueryHandler : IRequestHandler<GetMatchFromRiotQuery, MatchDto>
    {
        private readonly IApplicationDbContext context;
        private readonly IMatchService matchService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetMatchFromRiotQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        /// <param name="matchService">IMatchService, injected.</param>
        public GetMatchFromRiotQueryHandler(IApplicationDbContext context, IMatchService matchService)
        {
            this.context = context;
            this.matchService = matchService;
        }

        /// <inheritdoc />
        public async Task<MatchDto> Handle(GetMatchFromRiotQuery request, CancellationToken cancellationToken)
        {
            return await this.matchService.GetGameById(request.MatchId, cancellationToken);
        }
    }
}
