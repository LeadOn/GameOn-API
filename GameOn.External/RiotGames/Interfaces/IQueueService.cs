// <copyright file="IQueueService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Interfaces
{
    using GameOn.External.RiotGames.Models.DTOs;

    /// <summary>
    /// IQueueService interface.
    /// </summary>
    public interface IQueueService
    {
        /// <summary>
        /// Gets the list of League of Legends queue types.
        /// </summary>
        /// <param name="cancellationToken">Token to stop all async execution.</param>
        /// <returns>List of <see cref="QueueDto"/>.</returns>
        Task<IEnumerable<QueueDto>> GetQueues(CancellationToken cancellationToken);
    }
}
