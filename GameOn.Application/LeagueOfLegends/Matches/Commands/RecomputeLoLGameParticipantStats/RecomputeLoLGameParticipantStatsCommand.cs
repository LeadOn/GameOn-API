// <copyright file="RecomputeLoLGameParticipantStatsCommand.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Matches.Commands.RecomputeLoLGameParticipantStats
{
    using MediatR;

    /// <summary>
    /// RecomputeLoLGameParticipantStatsCommand class. Recomputes derived performance stats
    /// (<see cref="GameOn.Domain.LoLGameParticipantStat"/>) for every participant already in database.
    /// </summary>
    public class RecomputeLoLGameParticipantStatsCommand : IRequest<int>
    {
    }
}
