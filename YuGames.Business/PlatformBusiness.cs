// <copyright file="PlatformBusiness.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Business
{
    using YuGames.Business.Contracts;
    using YuGames.Entities;
    using YuGames.Repository.Contracts;

    /// <summary>
    /// Platform business.
    /// </summary>
    public class PlatformBusiness : IPlatformBusiness
    {
        private IPlatformRepository platformRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformBusiness" /> class.
        /// </summary>
        /// <param name="platformRepo">Platform repository, injected.</param>
        public PlatformBusiness(IPlatformRepository platformRepo)
        {
            this.platformRepo = platformRepo;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Platform>> GetAll()
        {
            return await this.platformRepo.GetAll();
        }

        /// <inheritdoc />
        public async Task<Platform?> Create(Platform platform)
        {
            return await this.platformRepo.Create(platform);
        }
    }
}