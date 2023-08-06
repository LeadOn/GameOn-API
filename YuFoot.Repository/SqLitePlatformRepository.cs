// <copyright file="SqLitePlatformRepository.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using YuFoot.Entities;
    using YuFoot.Repository.Contracts;

    /// <summary>
    /// Platform repository SQLite implementation.
    /// </summary>
    public class SqLitePlatformRepository : IPlatformRepository
    {
        private YuFootContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqLitePlatformRepository"/> class.
        /// </summary>
        /// <param name="context">Database context, injected.</param>
        public SqLitePlatformRepository(YuFootContext context)
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
    }
}
