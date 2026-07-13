// <copyright file="LoLQueueFilter.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends
{
    /// <summary>
    /// LoLQueueFilter enum. Restricts stats to a specific League of Legends queue.
    /// </summary>
    public enum LoLQueueFilter
    {
        /// <summary>
        /// Every queue.
        /// </summary>
        All,

        /// <summary>
        /// Ranked Solo/Duo queue only.
        /// </summary>
        Solo,

        /// <summary>
        /// Ranked Flex queue only.
        /// </summary>
        Flex,
    }
}
