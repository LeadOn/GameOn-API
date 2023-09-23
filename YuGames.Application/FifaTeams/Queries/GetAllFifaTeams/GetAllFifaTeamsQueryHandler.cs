// <copyright file="GetAllFifaTeamsQueryHandler.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Application.FifaTeams.Queries.GetAllFifaTeams
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using YuGames.Application.Common.Interfaces;
    using YuGames.Domain;

    /// <summary>
    /// GetAllFifaTeamsQueryHandler class.
    /// </summary>
    public class GetAllFifaTeamsQueryHandler : IRequestHandler<GetAllFifaTeamsQuery, IEnumerable<FifaTeam>>
    {
        private readonly IApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllFifaTeamsQueryHandler"/> class.
        /// </summary>
        /// <param name="context">DbContext, injected.</param>
        public GetAllFifaTeamsQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<FifaTeam>> Handle(GetAllFifaTeamsQuery request, CancellationToken cancellationToken)
        {
            return await this.context.FifaTeams.OrderBy(x => x.Name).ToListAsync();
        }
    }
}
