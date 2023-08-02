// <copyright file="SqLiteGamePlayedRepository.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.Repository
{
    using System.Linq.Expressions;
    using Microsoft.EntityFrameworkCore;
    using YuFoot.Entities;
    using YuFoot.Repository.Contracts;

    /// <summary>
    /// GamePlayed repository SQLite implementation.
    /// </summary>
    public class SqLiteGamePlayedRepository : IGamePlayedRepository
    {
        private YuFootContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqLiteGamePlayedRepository"/> class.
        /// </summary>
        /// <param name="context">Database context, injected.</param>
        public SqLiteGamePlayedRepository(YuFootContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<GamePlayed?> GetById(int id) =>
            await this.context.GamesPlayed.FirstOrDefaultAsync(x => x.Id == id);

        /// <inheritdoc />
        public async Task<IEnumerable<GamePlayed>> Search(Expression<Func<GamePlayed, bool>> query, int limit)
        {
            return await this.context.GamesPlayed.Include(x => x.TeamPlayers).Include(x => x.Platform).Include(x => x.Highlights).Where(query).OrderByDescending(x => x.PlayedOn).Take(limit).ToListAsync();
        }
    }
}
