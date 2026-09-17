// <copyright file="ILoLCoachQueue.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.Interfaces
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.Common.DTOs.LeagueOfLegends;

    /// <summary>
    /// The line of coach analyses waiting to be written.
    /// </summary>
    /// <remarks>
    /// Exists because a generation takes about fifty seconds against a provider that allows five calls a
    /// minute: holding the caller's connection open only ever worked for one player at a time, and a second
    /// simultaneous click could do nothing but fail. Nothing is ever queued on its own - a ticket exists
    /// because somebody pressed the button.
    /// </remarks>
    public interface ILoLCoachQueue
    {
        /// <summary>
        /// Puts an analysis in the line, or returns where it already stands if it is in there.
        /// </summary>
        /// <param name="matchId">Match to analyse.</param>
        /// <param name="playerId">GameOn! player to analyse.</param>
        /// <param name="forceRegenerate">Whether a stored report should be overwritten.</param>
        /// <returns>Where the analysis stands.</returns>
        LoLCoachQueueStatusDto Enqueue(string matchId, int playerId, bool forceRegenerate);

        /// <summary>
        /// Tells where an analysis stands, without putting anything in the line.
        /// </summary>
        /// <param name="matchId">Match to look for.</param>
        /// <param name="playerId">GameOn! player to look for.</param>
        /// <returns>Where the analysis stands, or null when it is not queued.</returns>
        LoLCoachQueueStatusDto? GetStatus(string matchId, int playerId);

        /// <summary>
        /// Waits for the next analysis to write. Reserved for the single consumer.
        /// </summary>
        /// <param name="cancellationToken">Token to stop waiting.</param>
        /// <returns>The next ticket.</returns>
        Task<LoLCoachQueueTicket> DequeueAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Drops a ticket out of the line and records how long it took, which is what feeds the estimates
        /// handed to everyone still waiting.
        /// </summary>
        /// <param name="ticket">Ticket that is done with, successfully or not.</param>
        /// <param name="elapsed">How long it held the consumer.</param>
        void Complete(LoLCoachQueueTicket ticket, TimeSpan elapsed);

        /// <summary>
        /// Puts a refused ticket back at the head of the line, keeping the place it had earned.
        /// </summary>
        /// <param name="ticket">Ticket the provider turned away.</param>
        void Retry(LoLCoachQueueTicket ticket);
    }
}
