// <copyright file="ValGameResultLabel.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Valorant.Matches.Services
{
    /// <summary>
    /// Turns a participation's outcome into the label exposed by <c>PlayerDto.RecentResultsValorant</c>.
    /// The draw flag must follow the rule of <see cref="ValPerformanceLine.Projection"/>:
    /// <c>!Win &amp;&amp; Game.WinningTeamId == null</c>.
    /// </summary>
    public static class ValGameResultLabel
    {
        /// <summary>
        /// Label of a won game.
        /// </summary>
        public const string Win = "Win";

        /// <summary>
        /// Label of a lost game.
        /// </summary>
        public const string Loss = "Loss";

        /// <summary>
        /// Label of a drawn game (no winning team).
        /// </summary>
        public const string Draw = "Draw";

        /// <summary>
        /// Gets the label of a participation's outcome.
        /// </summary>
        /// <param name="win">Whether the participant won.</param>
        /// <param name="isDraw">Whether the game ended without a winning team.</param>
        /// <returns><see cref="Win"/>, <see cref="Draw"/> or <see cref="Loss"/>.</returns>
        public static string From(bool win, bool isDraw)
        {
            if (win)
            {
                return Win;
            }

            return isDraw ? Draw : Loss;
        }
    }
}
