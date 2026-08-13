// <copyright file="ICommunityDragonQueueService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.CommunityDragon.Interfaces
{
    using GameOn.External.CommunityDragon.Models.DTOs;

    /// <summary>
    /// ICommunityDragonQueueService interface.
    /// </summary>
    public interface ICommunityDragonQueueService
    {
        /// <summary>
        /// Gets the list of League of Legends queue types known by Community Dragon.
        /// </summary>
        /// <param name="cancellationToken">Token to stop all async execution.</param>
        /// <returns>List of <see cref="CommunityDragonQueueDto"/>.</returns>
        Task<IEnumerable<CommunityDragonQueueDto>> GetQueues(CancellationToken cancellationToken);
    }
}
