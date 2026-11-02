// <copyright file="ValMapZoneResolver.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Matches.Services
{
    using System;
    using System.Linq;
    using GameOn.External.ValorantApi.Models.DTOs;

    /// <summary>
    /// Names the zone of a map a game position falls in (e.g. "A Main"), from the callouts of
    /// <c>valorant-api.com</c>, and projects positions onto the minimap.
    /// </summary>
    public static class ValMapZoneResolver
    {
        /// <summary>
        /// Gets the callout closest to a position. Callouts are reference points, not polygons, so the nearest
        /// one is the zone; it is good enough to tell "A Main" from "B Long", not to draw borders.
        /// </summary>
        /// <param name="map">Map, with its callouts.</param>
        /// <param name="x">X game coordinate.</param>
        /// <param name="y">Y game coordinate.</param>
        /// <returns>The zone name, or null when the map has no callout.</returns>
        public static string? Resolve(ValorantApiMapDto? map, int? x, int? y)
        {
            if (map?.Callouts is not { Count: > 0 } callouts || x is null || y is null)
            {
                return null;
            }

            var nearest = callouts
                .Where(c => c.Location is not null && !string.IsNullOrWhiteSpace(c.RegionName))
                .OrderBy(c => Math.Pow(c.Location!.X - x.Value, 2) + Math.Pow(c.Location.Y - y.Value, 2))
                .FirstOrDefault();

            return nearest is null
                ? null
                : $"{nearest.SuperRegionName} {nearest.RegionName}".Trim();
        }

        /// <summary>
        /// Projects a game position onto the minimap, both axes between 0 and 1. Valorant swaps the axes: the
        /// minimap X comes from the game Y, and the other way round.
        /// </summary>
        /// <param name="map">Map, with its minimap transform.</param>
        /// <param name="x">X game coordinate.</param>
        /// <param name="y">Y game coordinate.</param>
        /// <returns>The minimap coordinates, or null when the map has no transform.</returns>
        public static (double X, double Y)? ToMinimap(ValorantApiMapDto? map, int x, int y)
        {
            if (map is null || (map.XMultiplier == 0 && map.YMultiplier == 0))
            {
                return null;
            }

            return ((y * map.XMultiplier) + map.XScalarToAdd, (x * map.YMultiplier) + map.YScalarToAdd);
        }
    }
}
