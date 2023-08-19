// <copyright file="SqLitePlatformRepository.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Repository
{
    using Microsoft.EntityFrameworkCore;
    using YuGames.Entities;
    using YuGames.EntitiesContext;
    using YuGames.Repository.Contracts;

    /// <summary>
    /// Platform repository SQLite implementation.
    /// </summary>
    public class SqLitePlatformRepository : IPlatformRepository
    {
        private YuGamesContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqLitePlatformRepository"/> class.
        /// </summary>
        /// <param name="context">Database context, injected.</param>
        public SqLitePlatformRepository(YuGamesContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Platform>> GetAll()
        {
            return await this.context.Platforms.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Platform?> GetById(int id)
        {
            return await this.context.Platforms.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <inheritdoc />
        public async Task<Platform?> Create(Platform platform)
        {
            var platformToCreate = new Platform
            {
                Name = platform.Name,
            };

            await this.context.Platforms.AddAsync(platformToCreate);
            await this.context.SaveChangesAsync();

            return platformToCreate;
        }

        /// <inheritdoc />
        public async Task<Platform?> Update(Platform platform)
        {
            var platformInDb = await this.context.Platforms.FirstOrDefaultAsync(x => x.Id == platform.Id);

            if (platformInDb is null)
            {
                return null;
            }
            else
            {
                platformInDb.Name = platform.Name;

                this.context.Platforms.Update(platformInDb);
                await this.context.SaveChangesAsync();

                return platformInDb;
            }
        }
    }
}
