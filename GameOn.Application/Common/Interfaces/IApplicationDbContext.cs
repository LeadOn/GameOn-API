// <copyright file="IApplicationDbContext.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.Common.Interfaces
{
    using GameOn.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;

    /// <summary>
    /// Application Database Context interface.
    /// </summary>
    public interface IApplicationDbContext
    {
        /// <summary>
        /// Gets or sets Players.
        /// </summary>
        DbSet<Player> Players { get; set; }

        /// <summary>
        /// Gets or sets Platforms.
        /// </summary>
        DbSet<Platform> Platforms { get; set; }

        /// <summary>
        /// Gets or sets FIFA Teams.
        /// </summary>
        DbSet<FifaTeam> FifaTeams { get; set; }

        /// <summary>
        /// Gets or sets GamePlayed.
        /// </summary>
        DbSet<FifaGamePlayed> FifaGamesPlayed { get; set; }

        /// <summary>
        /// Gets or sets TeamPlayer.
        /// </summary>
        DbSet<FifaTeamPlayer> FifaTeamPlayers { get; set; }

        /// <summary>
        /// Gets or sets Highlights.
        /// </summary>
        DbSet<Highlight> Highlights { get; set; }

        /// <summary>
        /// Gets or sets Seasons.
        /// </summary>
        DbSet<Season> Seasons { get; set; }

        /// <summary>
        /// Gets or sets Tournaments.
        /// </summary>
        DbSet<Tournament> Tournaments { get; set; }

        /// <summary>
        /// Gets or sets TournamentPlayers.
        /// </summary>
        DbSet<TournamentPlayer> TournamentPlayers { get; set; }

        /// <summary>
        /// Gets or sets SoccerFives.
        /// </summary>
        DbSet<SoccerFive> SoccerFives { get; set; }

        /// <summary>
        /// Gets or sets SoccerFiveVoteChoices.
        /// </summary>
        DbSet<SoccerFiveVoteChoice> SoccerFiveVoteChoices { get; set; }

        /// <summary>
        /// Gets or sets SoccerFiveVoteAnswers.
        /// </summary>
        DbSet<SoccerFiveVoteAnswer> SoccerFiveVoteAnswers { get; set; }

        /// <summary>
        /// Gets or sets Changelogs.
        /// </summary>
        DbSet<Changelog> Changelogs { get; set; }

        /// <summary>
        /// Returns Database object from DbContext.
        /// </summary>
        /// <returns><see cref="DatabaseFacade" /> object.</returns>
        DatabaseFacade GetDatabase();

        /// <summary>
        /// Saves changes to the database context.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/>.</param>
        /// <returns>Task result as integer.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
