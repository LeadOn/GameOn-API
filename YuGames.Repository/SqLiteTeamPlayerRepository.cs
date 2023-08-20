// <copyright file="SqLiteTeamPlayerRepository.cs" company="LeadOn's Corp'">
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
    /// Team Player repository SQLite implementation.
    /// </summary>
    public class SqLiteTeamPlayerRepository : ITeamPlayerRepository
    {
        private YuGamesContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqLiteTeamPlayerRepository"/> class.
        /// </summary>
        /// <param name="context">Database context, injected.</param>
        public SqLiteTeamPlayerRepository(YuGamesContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<FifaTeamPlayer> CreateTeamPlayer(FifaTeamPlayer fifaTeamPlayer)
        {
            this.context.FifaTeamPlayers.Add(fifaTeamPlayer);
            await this.context.SaveChangesAsync();
            return fifaTeamPlayer;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAllFifaGame(int fifaGameId)
        {
            var teamPlayers = await this.context.FifaTeamPlayers.Where(x => x.FifaGameId == fifaGameId).ToListAsync();
            this.context.FifaTeamPlayers.RemoveRange(teamPlayers);
            await this.context.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<FifaTeamPlayer>> GetTeamPlayerByPlayerId(int playerId)
        {
            return await this.context.FifaTeamPlayers.Where(x => x.PlayerId == playerId).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<FifaTeamPlayer>> GetTeamPlayersByGameId(int gamePlayedId)
        {
            return await this.context.FifaTeamPlayers.Where(x => x.FifaGameId == gamePlayedId).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<FifaTeamPlayer>> Search(Expression<Func<FifaTeamPlayer, bool>> query, int limit)
        {
            return await this.context.FifaTeamPlayers.Where(query).Take(limit).ToListAsync();
        }
    }
}
