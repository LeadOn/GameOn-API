// <copyright file="FifaTeamBusiness.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Business
{
    using YuGames.Business.Contracts;
    using YuGames.Entities;
    using YuGames.Repository.Contracts;

    /// <summary>
    /// FifaTeam business.
    /// </summary>
    public class FifaTeamBusiness : IFifaTeamBusiness
    {
        private IFifaTeamRepository fifaTeamRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="FifaTeamBusiness" /> class.
        /// </summary>
        /// <param name="fifaTeamRepo">FifaTeam repository, injected.</param>
        public FifaTeamBusiness(IFifaTeamRepository fifaTeamRepo)
        {
            this.fifaTeamRepo = fifaTeamRepo;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<FifaTeam>> GetAll()
        {
            return await this.fifaTeamRepo.GetAll();
        }
    }
}