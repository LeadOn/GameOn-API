// <copyright file="SeasonBusiness.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Business
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using YuGames.Business.Contracts;
    using YuGames.Entities;
    using YuGames.Repository.Contracts;

    /// <summary>
    /// Season business.
    /// </summary>
    public class SeasonBusiness : ISeasonBusiness
    {
        private ISeasonRepository seasonRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonBusiness" /> class.
        /// </summary>
        /// <param name="seasonRepo">Season repository, injected.</param>
        public SeasonBusiness(ISeasonRepository seasonRepo)
        {
            this.seasonRepo = seasonRepo;
        }

        /// <inheritdoc/>
        public async Task<List<Season>> GetAll()
        {
            return await this.seasonRepo.GetAll();
        }

        /// <inheritdoc />
        public async Task<Season?> GetCurrent()
        {
            return await this.seasonRepo.GetCurrent();
        }
    }
}