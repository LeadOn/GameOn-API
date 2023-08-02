// <copyright file="SqLiteTeamPlayerRepository.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.Repository
{
    using Microsoft.EntityFrameworkCore;
    using YuFoot.Entities;
    using YuFoot.Repository.Contracts;

    /// <summary>
    /// Team Player repository SQLite implementation.
    /// </summary>
    public class SqLiteTeamPlayerRepository : ITeamPlayerRepository
    {
        private YuFootContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqLiteTeamPlayerRepository"/> class.
        /// </summary>
        /// <param name="context">Database context, injected.</param>
        public SqLiteTeamPlayerRepository(YuFootContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<TeamPlayer>> GetTeamPlayerByPlayerId(int playerId)
        {
            return await this.context.TeamPlayers.Where(x => x.PlayerId == playerId).ToListAsync();
        }

        public async Task<IEnumerable<TeamPlayer>> GetTeamPlayersByGameId(int gamePlayedId)
        {
            return await this.context.TeamPlayers.Where(x => x.GamePlayedId == gamePlayedId).ToListAsync();
        }
    }
}
