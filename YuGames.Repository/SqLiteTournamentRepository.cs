// <copyright file="SqLiteTournamentRepository.cs" company="LeadOn's Corp'">
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
    /// Tournament repository SQLite implementation.
    /// </summary>
    public class SqLiteTournamentRepository : ITournamentRepository
    {
        private YuGamesContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqLiteTournamentRepository"/> class.
        /// </summary>
        /// <param name="context">Database context, injected.</param>
        public SqLiteTournamentRepository(YuGamesContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<Tournament> Create(Tournament tournament)
        {
            this.context.Tournaments.Add(tournament);
            await this.context.SaveChangesAsync();
            return tournament;
        }

        /// <inheritdoc />
        public async Task<List<Tournament>> GetAll()
        {
            return await this.context.Tournaments.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<int> Count()
        {
            return await this.context.Tournaments.CountAsync();
        }
    }
}
