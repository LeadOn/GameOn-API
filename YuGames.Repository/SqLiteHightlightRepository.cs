// <copyright file="SqLiteHightlightRepository.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Repository
{
    using Microsoft.EntityFrameworkCore;
    using YuGames.Entities;
    using YuGames.EntitiesContext;
    using YuGames.Repository.Contracts;

    /// <summary>
    /// Hightlight repository SQLite implementation.
    /// </summary>
    public class SqLiteHightlightRepository : IHighlightRepository
    {
        private YuGamesContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqLiteHightlightRepository"/> class.
        /// </summary>
        /// <param name="context">Database context, injected.</param>
        public SqLiteHightlightRepository(YuGamesContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<int> Count()
        {
            return await this.context.Highlights.CountAsync();
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAllFifaGame(int fifaGameId)
        {
            var highlights = await this.context.Highlights.Where(x => x.FifaGameId == fifaGameId).ToListAsync();
            this.context.RemoveRange(highlights);
            await this.context.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc />
        public async Task<List<Highlight>> GetAll()
        {
            return await this.context.Highlights.ToListAsync();
        }
    }
}
