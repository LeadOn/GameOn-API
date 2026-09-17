// <copyright file="LoLCoachQueue.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Coach.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Common.DTOs.LeagueOfLegends;
    using GameOn.Common.Interfaces;

    /// <summary>
    /// In-memory implementation of <see cref="ILoLCoachQueue"/>. Registered as a singleton: the line is the
    /// state, so there can only be one of it.
    /// </summary>
    /// <remarks>
    /// A <see cref="LinkedList{T}"/> guarded by a lock rather than a channel, because the line has to answer
    /// two questions a channel cannot: where a given ticket stands, and how to put a refused one back at the
    /// head instead of the tail. The waiting player is the reason both exist.
    /// </remarks>
    public class LoLCoachQueue : ILoLCoachQueue
    {
        /// <summary>
        /// What a generation is assumed to cost before any has been measured. Fifty seconds is what production
        /// actually showed on 2026-09-17; the long-standing "about fifteen seconds" was never true of this
        /// model and every estimate built on it was wrong by a factor of three.
        /// </summary>
        private const int SeedDurationSeconds = 50;

        /// <summary>
        /// How many recent generations the estimate averages over. Short enough to follow the provider when it
        /// slows down, long enough that a single outlier does not throw the whole line's estimate.
        /// </summary>
        private const int DurationSampleSize = 10;

        private readonly LinkedList<LoLCoachQueueTicket> waiting = new LinkedList<LoLCoachQueueTicket>();
        private readonly Queue<double> recentDurations = new Queue<double>();
        private readonly SemaphoreSlim signal = new SemaphoreSlim(0);
        private readonly object gate = new object();

        private LoLCoachQueueTicket? inProgress;
        private DateTime inProgressStartedOn;

        /// <inheritdoc/>
        public LoLCoachQueueStatusDto Enqueue(string matchId, int playerId, bool forceRegenerate)
        {
            lock (this.gate)
            {
                // Clicking twice must not buy two slots: the second press simply reports where the first one
                // already stands, which is also what the front does when a player refreshes the page.
                var existing = this.Find(matchId, playerId);

                if (existing is not null)
                {
                    return this.BuildStatus(existing);
                }

                var ticket = new LoLCoachQueueTicket
                {
                    MatchId = matchId,
                    PlayerId = playerId,
                    ForceRegenerate = forceRegenerate,
                    EnqueuedOn = DateTime.UtcNow,
                };

                this.waiting.AddLast(ticket);
                this.signal.Release();

                return this.BuildStatus(ticket);
            }
        }

        /// <inheritdoc/>
        public LoLCoachQueueStatusDto? GetStatus(string matchId, int playerId)
        {
            lock (this.gate)
            {
                var ticket = this.Find(matchId, playerId);

                return ticket is null ? null : this.BuildStatus(ticket);
            }
        }

        /// <inheritdoc/>
        public async Task<LoLCoachQueueTicket> DequeueAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                await this.signal.WaitAsync(cancellationToken);

                lock (this.gate)
                {
                    var next = this.waiting.First;

                    if (next is null)
                    {
                        // A spare signal with nothing behind it: wait again rather than trust the count.
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
        public void Complete(LoLCoachQueueTicket ticket, TimeSpan elapsed)
        {
            lock (this.gate)
            {
                if (ReferenceEquals(this.inProgress, ticket))
                {
                    this.inProgress = null;
                }

                if (elapsed <= TimeSpan.Zero)
                {
                    return;
                }

                this.recentDurations.Enqueue(elapsed.TotalSeconds);

                while (this.recentDurations.Count > DurationSampleSize)
                {
                    this.recentDurations.Dequeue();
                }
            }
        }

        /// <inheritdoc/>
        public void Retry(LoLCoachQueueTicket ticket)
        {
            lock (this.gate)
            {
                ticket.Attempts++;

                if (ReferenceEquals(this.inProgress, ticket))
                {
                    this.inProgress = null;
                }

                this.waiting.AddFirst(ticket);
                this.signal.Release();
            }
        }

        /// <summary>
        /// Tells whether a ticket is the one being asked about.
        /// </summary>
        /// <param name="ticket">Ticket to test.</param>
        /// <param name="matchId">Match to look for.</param>
        /// <param name="playerId">GameOn! player to look for.</param>
        /// <returns>True when it is the same analysis.</returns>
        private static bool Matches(LoLCoachQueueTicket ticket, string matchId, int playerId)
        {
            return ticket.PlayerId == playerId && string.Equals(ticket.MatchId, matchId, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Finds a ticket anywhere in the line. Must be called while holding the lock.
        /// </summary>
        /// <param name="matchId">Match to look for.</param>
        /// <param name="playerId">GameOn! player to look for.</param>
        /// <returns>The ticket, or null.</returns>
        private LoLCoachQueueTicket? Find(string matchId, int playerId)
        {
            if (this.inProgress is not null && Matches(this.inProgress, matchId, playerId))
            {
                return this.inProgress;
            }

            return this.waiting.FirstOrDefault(x => Matches(x, matchId, playerId));
        }

        /// <summary>
        /// Works out where a ticket stands and when it should be readable. Must be called while holding the
        /// lock.
        /// </summary>
        /// <param name="ticket">Ticket to describe.</param>
        /// <returns>Its status.</returns>
        private LoLCoachQueueStatusDto BuildStatus(LoLCoachQueueTicket ticket)
        {
            var average = this.recentDurations.Count == 0 ? SeedDurationSeconds : this.recentDurations.Average();
            var hasCurrent = this.inProgress is not null;

            // What is left of the generation already under way, never less than zero: one that overruns the
            // average would otherwise hand everybody behind it a wait shorter than reality.
            var currentRemaining = hasCurrent
                ? Math.Max(0, average - (DateTime.UtcNow - this.inProgressStartedOn).TotalSeconds)
                : 0;

            int position;
            double estimate;

            if (ReferenceEquals(this.inProgress, ticket))
            {
                position = 1;
                estimate = currentRemaining;
            }
            else
            {
                var ahead = 0;

                foreach (var queued in this.waiting)
                {
                    if (ReferenceEquals(queued, ticket))
                    {
                        break;
                    }

                    ahead++;
                }

                position = ahead + (hasCurrent ? 2 : 1);

                // Everything that has to finish first, plus this analysis's own generation - the player is
                // waiting to read their report, not to reach the front of the line.
                estimate = currentRemaining + (ahead * average) + average;
            }

            return new LoLCoachQueueStatusDto
            {
                MatchId = ticket.MatchId,
                PlayerId = ticket.PlayerId,
                Position = position,
                QueueLength = this.waiting.Count + (hasCurrent ? 1 : 0),
                EstimatedWaitSeconds = (int)Math.Ceiling(estimate),
                EnqueuedOn = ticket.EnqueuedOn,
            };
        }
    }
}
