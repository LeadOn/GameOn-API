// <copyright file="IValorantMapService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.ValorantApi.Interfaces
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using GameOn.External.ValorantApi.Models.DTOs;

    /// <summary>
    /// Valorant map reference data (minimap transform, named zones) from <c>valorant-api.com</c>.
    /// </summary>
    public interface IValorantMapService
    {
        /// <summary>
        /// Gets every map, keyed by map UUID (case-insensitive). Cached for a day.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Maps by UUID, empty when the API cannot be reached.</returns>
        Task<IReadOnlyDictionary<string, ValorantApiMapDto>> GetMapsById(CancellationToken cancellationToken);
    }
}
