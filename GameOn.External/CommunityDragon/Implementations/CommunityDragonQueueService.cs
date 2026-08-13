// <copyright file="CommunityDragonQueueService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.CommunityDragon.Implementations
{
    using GameOn.External.Common;
    using GameOn.External.CommunityDragon.Interfaces;
    using GameOn.External.CommunityDragon.Models.DTOs;

    /// <summary>
    /// CommunityDragonQueueService class.
    /// </summary>
    public class CommunityDragonQueueService : HttpServiceBase, ICommunityDragonQueueService
    {
        private readonly HttpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommunityDragonQueueService"/> class.
        /// </summary>
        /// <param name="client"><see cref="HttpClient"/>.</param>
        public CommunityDragonQueueService(HttpClient client)
        {
            this.client = client;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CommunityDragonQueueDto>> GetQueues(CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/fr_fr/v1/queues.json");
#pragma warning disable CS8603 // Existence possible d'un retour de référence null.
            return await RunRequest<IEnumerable<CommunityDragonQueueDto>>(this.client, request, cancellationToken);
#pragma warning restore CS8603 // Existence possible d'un retour de référence null
        }
    }
}
