// <copyright file="LcuTimelineDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Common.DTOs.LeagueOfLegends.LeagueClient
{
    using System.Collections.Generic;

    /// <summary>
    /// LcuTimelineDto class. A game timeline as served by the League client
    /// (<c>/lol-match-history/v1/game-timelines/{gameId}</c>).
    /// </summary>
    public class LcuTimelineDto
    {
        /// <summary>
        /// Gets or sets the frames. The client sends no frame interval: it is derived from the
        /// frames' own timestamps on import.
        /// </summary>
        public List<LcuFrameDto> Frames { get; set; } = new List<LcuFrameDto>();
    }
}
