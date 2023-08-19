// <copyright file="SqLiteFifaTeamRepository.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Repository
{
    using Microsoft.EntityFrameworkCore;
    using YuGames.Entities;
    using YuGames.EntitiesContext;
    using YuGames.Repository.Contracts;

    /// <summary>
    /// Fifa Team repository SQLite implementation.
    /// </summary>
    public class SqLiteFifaTeamRepository : IFifaTeamRepository
    {
        private YuGamesContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqLiteFifaTeamRepository"/> class.
        /// </summary>
        /// <param name="context">Database context, injected.</param>
        public SqLiteFifaTeamRepository(YuGamesContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<FifaTeam>> GetAll()
        {
            return await this.context.FifaTeams.OrderBy(x => x.Name).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<FifaTeam?> GetById(int id)
        {
            return await this.context.FifaTeams.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
