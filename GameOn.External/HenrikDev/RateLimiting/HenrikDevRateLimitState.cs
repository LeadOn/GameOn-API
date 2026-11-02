// <copyright file="HenrikDevRateLimitState.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.HenrikDev.RateLimiting
{
    using System;

    /// <summary>
    /// Process-wide view of the HenrikDev quota. Registered as a singleton: the bucket belongs to the API key,
    /// not to a request, so the background refresh and a player's manual refresh have to see the same one.
    /// </summary>
    public class HenrikDevRateLimitState
    {
        private readonly object sync = new ();

        private DateTime blockedUntil = DateTime.MinValue;

        private int? remaining;

        private DateTime windowResetsAt = DateTime.MinValue;

        /// <summary>
        /// Gets how long a caller has to wait before its request may leave, <see cref="TimeSpan.Zero"/> when
        /// the bucket still has room.
        /// </summary>
        /// <returns>Remaining wait.</returns>
        public TimeSpan GetWait()
        {
            lock (this.sync)
            {
                var wait = this.blockedUntil - DateTime.UtcNow;
                return wait > TimeSpan.Zero ? wait : TimeSpan.Zero;
            }
        }

        /// <summary>
        /// Gets how long a low-priority caller (backfill, daily extras) has to wait: on top of the hard block, it
        /// also waits for the next window whenever the bucket is down to its last <paramref name="reserve"/>
        /// requests, which stay for the regular refresh and the players' clicks.
        /// </summary>
        /// <param name="reserve">Requests of the window a low-priority caller must leave untouched.</param>
        /// <returns>Remaining wait.</returns>
        public TimeSpan GetLowPriorityWait(int reserve)
        {
            lock (this.sync)
            {
                var now = DateTime.UtcNow;
                var wait = this.blockedUntil - now;

                if (this.remaining is { } left && left <= reserve && this.windowResetsAt > now)
                {
                    var untilReset = this.windowResetsAt - now;
                    wait = untilReset > wait ? untilReset : wait;
                }

                return wait > TimeSpan.Zero ? wait : TimeSpan.Zero;
            }
        }

        /// <summary>
        /// Records the bucket state read on the last response.
        /// </summary>
        /// <param name="remainingRequests">Requests left in the window, null when the header was absent.</param>
        /// <param name="reset">Time until the window resets.</param>
        public void Report(int? remainingRequests, TimeSpan reset)
        {
            lock (this.sync)
            {
                this.remaining = remainingRequests;
                this.windowResetsAt = DateTime.UtcNow + reset;
            }
        }

        /// <summary>
        /// Holds every request back until the given delay has elapsed. Never shortens a block already in place.
        /// </summary>
        /// <param name="delay">How long the bucket stays empty.</param>
        public void BlockFor(TimeSpan delay)
        {
            lock (this.sync)
            {
                var until = DateTime.UtcNow + delay;
                if (until > this.blockedUntil)
                {
                    this.blockedUntil = until;
                }
            }
        }
    }
}
