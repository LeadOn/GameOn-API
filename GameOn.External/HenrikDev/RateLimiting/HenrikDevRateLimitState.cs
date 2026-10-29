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
