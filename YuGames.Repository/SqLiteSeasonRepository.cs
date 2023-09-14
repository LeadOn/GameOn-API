// <copyright file="SqLiteSeasonRepository.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using YuGames.Entities;
    using YuGames.EntitiesContext;
    using YuGames.Repository.Contracts;

    /// <summary>
    /// Season repository SQLite implementation.
    /// </summary>
    public class SqLiteSeasonRepository : ISeasonRepository
    {
        private YuGamesContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqLiteSeasonRepository"/> class.
        /// </summary>
        /// <param name="context">Database context, injected.</param>
        public SqLiteSeasonRepository(YuGamesContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<List<Season>> GetAll()
        {
            return await this.context.Seasons.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Season?> GetCurrent()
        {
            return await this.context.Seasons.FirstOrDefaultAsync(x => x.Id == int.Parse(Environment.GetEnvironmentVariable("CURRENT_SEASON") ?? "1"));
        }
        
        /// <inheritdoc />
        public async Task<int> Count()
        {
            return await this.context.Seasons.CountAsync();
        }
    }
}
