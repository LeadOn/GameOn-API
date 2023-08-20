// <copyright file="AdminBusiness.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Business
{
    using System.Threading.Tasks;
    using YuGames.Business.Contracts;
    using YuGames.DTOs;
    using YuGames.Repository.Contracts;

    /// <summary>
    /// Admin business.
    /// </summary>
    public class AdminBusiness : IAdminBusiness
    {
        private IPlayerRepository playerRepo;
        private IPlatformRepository platformRepo;
        private IHighlightRepository highlightRepo;
        private IFifaGamePlayedRepository fifaGamePlayedRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminBusiness" /> class.
        /// </summary>
        /// <param name="playerRepo">Player repository, injected.</param>
        /// <param name="platformRepo">Platform repository, injected.</param>
        /// <param name="highlightRepo">Highlight repository, injected.</param>
        /// <param name="fifaGamePlayedRepo">FifaGamePlayed repository, injected.</param>
        public AdminBusiness(IPlayerRepository playerRepo, IPlatformRepository platformRepo, IHighlightRepository highlightRepo, IFifaGamePlayedRepository fifaGamePlayedRepo)
        {
            this.playerRepo = playerRepo;
            this.platformRepo = platformRepo;
            this.highlightRepo = highlightRepo;
            this.fifaGamePlayedRepo = fifaGamePlayedRepo;
        }

        /// <inheritdoc />
        public async Task<AdminDashboardDto?> GetDashboardStats(string keycloakId)
        {
            // First getting user
            var playerInDb = await this.playerRepo.GetPlayerByKeycloakId(keycloakId);

            if (playerInDb == null)
            {
                return null;
            }

            var adminDashboard = new AdminDashboardDto
            {
                CurrentUser = playerInDb,
            };

            // Getting counts
            adminDashboard.Platforms = await this.platformRepo.Count();
            adminDashboard.Players = await this.playerRepo.Count();
            adminDashboard.Highlights = await this.highlightRepo.Count();
            adminDashboard.FifaGames = await this.fifaGamePlayedRepo.Count();

            return adminDashboard;
        }
    }
}