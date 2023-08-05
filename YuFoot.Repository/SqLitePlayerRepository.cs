// <copyright file="SqLitePlayerRepository.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.Repository
{
    using Microsoft.EntityFrameworkCore;
    using YuFoot.Entities;
    using YuFoot.Repository.Contracts;

    /// <summary>
    /// Player repository SQLite implementation.
    /// </summary>
    public class SqLitePlayerRepository : IPlayerRepository
    {
        private YuFootContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqLitePlayerRepository"/> class.
        /// </summary>
        /// <param name="context">Database context, injected.</param>
        public SqLitePlayerRepository(YuFootContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<Player?> GetPlayerById(int id) =>
            await this.context.Players.FirstOrDefaultAsync(x => x.Id == id);

        /// <inheritdoc />
        public async Task<IEnumerable<Player>> GetAll() => await this.context.Players.ToListAsync();

        /// <inheritdoc />
        public async Task<Player> GetPlayerByKeycloakId(string keycloakId, string email)
        {
            var userInDb = await this.context.Players.FirstOrDefaultAsync(x => x.KeycloakId == keycloakId);

            if (userInDb is not null)
            {
                return userInDb;
            }
            else
            {
                // Creating user
                var user = new Player
                {
                    KeycloakId = keycloakId,
                    CreatedOn = DateTime.UtcNow,
                    FullName = email,
                    Nickname = email,
                };
                this.context.Players.Add(user);
                await this.context.SaveChangesAsync();
                return user;
            }
        }
    }
}
