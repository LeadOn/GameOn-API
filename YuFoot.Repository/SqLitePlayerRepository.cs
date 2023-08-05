// <copyright file="SqLitePlayerRepository.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.Repository
{
    using Microsoft.EntityFrameworkCore;
    using YuFoot.DTOs;
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
        public async Task<Player> GetPlayerByKeycloakId(ConnectedPlayerDto player)
        {
            var userInDb = await this.context.Players.FirstOrDefaultAsync(x => x.KeycloakId == player.KeycloakId);

            if (userInDb is not null)
            {
                return userInDb;
            }
            else
            {
                var fullName = player.Email;

                if (player.FirstName is not null)
                {
                    fullName = player.FirstName;
                    if (player.LastName is not null)
                    {
                        fullName += " " + player.LastName;
                    }
                }

                var nickname = fullName;

                if (player.PreferredUsername is not null)
                {
                    nickname = player.PreferredUsername;
                }

                // Creating user
                var user = new Player
                {
                    KeycloakId = player.KeycloakId,
                    CreatedOn = DateTime.UtcNow,
                    FullName = fullName,
                    Nickname = nickname,
                };

                this.context.Players.Add(user);
                await this.context.SaveChangesAsync();
                return user;
            }
        }

        /// <inheritdoc />
        public async Task<Player> UpdateUser(string keycloakId, string fullName, string nickname, string profilePictureUrl)
        {
            var userInDb = await this.context.Players.FirstOrDefaultAsync(x => x.KeycloakId == keycloakId);

            if (userInDb is not null)
            {
                userInDb.FullName = fullName;
                userInDb.Nickname = nickname;
                userInDb.ProfilePictureUrl = profilePictureUrl;
                await this.context.SaveChangesAsync();
                return userInDb;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
