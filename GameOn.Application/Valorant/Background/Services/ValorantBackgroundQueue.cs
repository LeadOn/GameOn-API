// <copyright file="ValorantBackgroundQueue.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Background.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Common.DTOs.Valorant;
    using GameOn.Common.Interfaces;

    /// <summary>
    /// In-memory implementation of <see cref="IValorantBackgroundQueue"/>. Registered as a singleton: the
    /// line is the state. Lost on redeploy, which only means a backfill has to be asked for again: every task
    /// is idempotent and skips what the database already holds.
    /// </summary>
    public class ValorantBackgroundQueue : IValorantBackgroundQueue
    {
        private const int KeptResults = 20;

        private readonly LinkedList<ValBackgroundTaskDto> waiting = new LinkedList<ValBackgroundTaskDto>();
        private readonly LinkedList<ValBackgroundTaskResultDto> results = new LinkedList<ValBackgroundTaskResultDto>();
        private readonly SemaphoreSlim signal = new SemaphoreSlim(0);
        private readonly object gate = new object();

        private ValBackgroundTaskDto? inProgress;
        private DateTime inProgressStartedOn;

        /// <inheritdoc/>
        public bool Enqueue(ValBackgroundTaskDto task)
        {
            lock (this.gate)
            {
                if (this.waiting.Any(x => x.Kind == task.Kind && x.PlayerId == task.PlayerId && x.Since == task.Since))
                {
                    return false;
                }

                this.waiting.AddLast(task);
            }

            this.signal.Release();
            return true;
        }

        /// <inheritdoc/>
        public async Task<ValBackgroundTaskDto> DequeueAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                await this.signal.WaitAsync(cancellationToken);

                lock (this.gate)
                {
                    var next = this.waiting.First;

                    if (next is null)
                    {
                        continue;
                    }

                    this.waiting.RemoveFirst();
                    this.inProgress = next.Value;
                    this.inProgressStartedOn = DateTime.UtcNow;
                    return next.Value;
                }
            }
        }

        /// <inheritdoc/>
        public void Complete(string summary, bool failed)
        {
            lock (this.gate)
            {
                if (this.inProgress is null)
                {
                    return;
                }

                this.results.AddFirst(new ValBackgroundTaskResultDto
                {
                    Task = this.inProgress,
                    StartedOn = this.inProgressStartedOn,
                    FinishedOn = DateTime.UtcNow,
                    Failed = failed,
                    Summary = summary,
                });

                while (this.results.Count > KeptResults)
                {
                    this.results.RemoveLast();
                }

                this.inProgress = null;
            }
        }

        /// <inheritdoc/>
        public ValBackgroundQueueStatusDto GetStatus()
        {
            lock (this.gate)
            {
                return new ValBackgroundQueueStatusDto
                {
                    InProgress = this.inProgress,
                    InProgressStartedOn = this.inProgress is null ? null : this.inProgressStartedOn,
                    Pending = this.waiting.ToList(),
                    RecentResults = this.results.ToList(),
                };
            }
        }
    }
}
