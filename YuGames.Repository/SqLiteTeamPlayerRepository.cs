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
        public async Task<TeamPlayer> CreateTeamPlayer(TeamPlayer teamPlayer)
        {
            this.context.TeamPlayers.Add(teamPlayer);
            await this.context.SaveChangesAsync();
            return teamPlayer;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TeamPlayer>> GetTeamPlayerByPlayerId(int playerId)
        {
            return await this.context.TeamPlayers.Where(x => x.PlayerId == playerId).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TeamPlayer>> GetTeamPlayersByGameId(int gamePlayedId)
        {
            return await this.context.TeamPlayers.Where(x => x.GamePlayedId == gamePlayedId).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TeamPlayer>> Search(Expression<Func<TeamPlayer, bool>> query, int limit)
        {
            return await this.context.TeamPlayers.Where(query).Take(limit).ToListAsync();
        }
    }
}
