// <copyright file="PlayerBusiness.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.Business
{
    using YuFoot.Business.Contracts;
    using YuFoot.Entities;
    using YuFoot.Repository.Contracts;

    /// <summary>
    /// Player business.
    /// </summary>
    public class PlayerBusiness : IPlayerBusiness
    {
        private IPlayerRepository playerRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerBusiness" /> class.
        /// </summary>
        /// <param name="playerRepo">Player repository, injected.</param>
        public PlayerBusiness(IPlayerRepository playerRepo)
        {
            this.playerRepo = playerRepo;
        }

        /// <inheritdoc />
        public async Task<Player?> GetPlayerById(int id)
        {
            return await this.playerRepo.GetPlayerById(id);
        }
    }
}