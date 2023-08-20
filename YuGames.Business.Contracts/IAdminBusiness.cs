// <copyright file="IAdminBusiness.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.Business.Contracts
{
    using YuGames.DTOs;

    /// <summary>
    /// Admin business interface.
    /// </summary>
    public interface IAdminBusiness
    {
        /// <summary>
        /// Get dashboard stats.
        /// </summary>
        /// <param name="keycloakId">Keycloak ID of connected user.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<AdminDashboardDto?> GetDashboardStats(string keycloakId);
    }
}