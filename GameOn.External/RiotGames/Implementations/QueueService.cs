// <copyright file="QueueService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Implementations
{
    using GameOn.External.Common;
    using GameOn.External.RiotGames.Interfaces;
    using GameOn.External.RiotGames.Models.DTOs;

    /// <summary>
    /// QueueService class.
    /// </summary>
    public class QueueService : HttpServiceBase, IQueueService
    {
        private readonly HttpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueService"/> class.
        /// </summary>
        /// <param name="client"><see cref="HttpClient"/>.</param>
        public QueueService(HttpClient client)
        {
            this.client = client;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<QueueDto>> GetQueues(CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://static.developer.riotgames.com/docs/lol/queues.json");
#pragma warning disable CS8603 // Existence possible d'un retour de référence null.
            return await RunRequest<IEnumerable<QueueDto>>(this.client, request, cancellationToken);
#pragma warning restore CS8603 // Existence possible d'un retour de référence null
        }
    }
}
