// <copyright file="SqLiteFifaTeamRepository.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.Repository
{
    using System;
    using System.Linq.Expressions;
    using Microsoft.EntityFrameworkCore;
    using YuFoot.Entities;
    using YuFoot.EntitiesContext;
    using YuFoot.Repository.Contracts;

    /// <summary>
    /// Fifa Team repository SQLite implementation.
    /// </summary>
    public class SqLiteFifaTeamRepository : IFifaTeamRepository
    {
        private YuFootContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqLiteFifaTeamRepository"/> class.
        /// </summary>
        /// <param name="context">Database context, injected.</param>
        public SqLiteFifaTeamRepository(YuFootContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<FifaTeam>> GetAll()
        {
            return await this.context.FifaTeams.ToListAsync();
        }
    }
}
