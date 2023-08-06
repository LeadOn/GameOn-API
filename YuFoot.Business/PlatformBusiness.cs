// <copyright file="PlatformBusiness.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.Business
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using YuFoot.Business.Contracts;
    using YuFoot.Entities;
    using YuFoot.Repository.Contracts;

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
    }
}