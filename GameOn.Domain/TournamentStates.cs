// <copyright file="TournamentStates.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    /// <summary>
    /// Tournament states class.
    /// </summary>
    public static class TournamentStates
    {
        /// <summary>
        /// Gets Draft state.
        /// </summary>
#pragma warning disable SA1401 // Fields should be private
        public static int Draft = 0;
#pragma warning restore SA1401 // Fields should be private

        /// <summary>
        /// Gets Planned state.
        /// </summary>
#pragma warning disable SA1401 // Fields should be private
        public static int Planned = 1;
#pragma warning restore SA1401 // Fields should be private

        /// <summary>
        /// Gets Phase 1 state.
        /// </summary>
#pragma warning disable SA1401 // Fields should be private
        public static int Phase1 = 2;
#pragma warning restore SA1401 // Fields should be private
    }
}
