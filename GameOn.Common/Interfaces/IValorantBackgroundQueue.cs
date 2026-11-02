// <copyright file="IValorantBackgroundQueue.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Common.DTOs.Valorant;

    /// <summary>
    /// The line of Valorant background tasks (backfill, reimport, daily extras), worked one at a time by a
    /// single consumer so that they never compete with each other for the HenrikDev quota.
    /// </summary>
    public interface IValorantBackgroundQueue
    {
        /// <summary>
        /// Queues a task, unless the same one (kind, player, bound) is already waiting.
        /// </summary>
        /// <param name="task">Task to queue.</param>
        /// <returns>True when queued, false when it was already waiting.</returns>
        bool Enqueue(ValBackgroundTaskDto task);

        /// <summary>
        /// Waits for the next task and marks it as running.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The next task.</returns>
        Task<ValBackgroundTaskDto> DequeueAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Records the outcome of the running task.
        /// </summary>
        /// <param name="summary">What it did, or the error that stopped it.</param>
        /// <param name="failed">Whether it stopped on an error.</param>
        void Complete(string summary, bool failed);

        /// <summary>
        /// Gets the state of the line.
        /// </summary>
        /// <returns>The running task, the waiting ones and the last results.</returns>
        ValBackgroundQueueStatusDto GetStatus();
    }
}
