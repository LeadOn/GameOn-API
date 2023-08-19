// <copyright file="SqLiteGamePlayedRepository.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Repository
{
    using System.Linq.Expressions;
    using Microsoft.EntityFrameworkCore;
    using YuGames.Entities;
    using YuGames.EntitiesContext;
    using YuGames.Repository.Contracts;

    /// <summary>
    /// GamePlayed repository SQLite implementation.
    /// </summary>
    public class SqLiteGamePlayedRepository : IGamePlayedRepository
    {
        private YuGamesContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqLiteGamePlayedRepository"/> class.
        /// </summary>
        /// <param name="context">Database context, injected.</param>
        public SqLiteGamePlayedRepository(YuGamesContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<FifaGamePlayed> Create(FifaGamePlayed fifaGame)
        {
            this.context.Add(fifaGame);
            await this.context.SaveChangesAsync();
            return fifaGame;
        }

        /// <inheritdoc />
        public async Task<FifaGamePlayed?> GetById(int id) =>
            await this.context.GamesPlayed.Include(x => x.Highlights).Include(x => x.TeamPlayers).Include(x => x.Platform).Include(x => x.CreatedBy).FirstOrDefaultAsync(x => x.Id == id);

        /// <inheritdoc />
        public async Task<IEnumerable<FifaGamePlayed>> Search(Expression<Func<FifaGamePlayed, bool>> query, int limit)
        {
            return await this.context.GamesPlayed.Include(x => x.CreatedBy).Include(x => x.TeamPlayers).Include(x => x.Platform).Include(x => x.Highlights).Where(query).OrderByDescending(x => x.PlayedOn).Take(limit).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<FifaGamePlayed?> Update(FifaGamePlayed fifaGame)
        {
            this.context.Update(fifaGame);
            await this.context.SaveChangesAsync();
            return fifaGame;
        }
    }
}
