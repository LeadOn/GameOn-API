﻿// <copyright file="GameOnContext.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Persistence
{
    using GameOn.Application.Common.Interfaces;
    using GameOn.Common.Exceptions;
    using GameOn.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;

    /// <summary>
    /// GameOn database context.
    /// </summary>
    public class GameOnContext : DbContext, IApplicationDbContext
    {
        // SQL Connection string
        private readonly string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? throw new MissingEnvironmentVariableException("DB_CONNECTION_STRING");

        /// <summary>
        /// Gets or sets Players.
        /// </summary>
        public DbSet<Player> Players { get; set; } = null!;

        /// <summary>
        /// Gets or sets Platforms.
        /// </summary>
        public DbSet<Platform> Platforms { get; set; } = null!;

        /// <summary>
        /// Gets or sets FIFA Teams.
        /// </summary>
        public DbSet<FifaTeam> FifaTeams { get; set; } = null!;

        /// <summary>
        /// Gets or sets GamePlayed.
        /// </summary>
        public DbSet<FifaGamePlayed> FifaGamesPlayed { get; set; } = null!;

        /// <summary>
        /// Gets or sets TeamPlayer.
        /// </summary>
        public DbSet<FifaTeamPlayer> FifaTeamPlayers { get; set; } = null!;

        /// <summary>
        /// Gets or sets Highlights.
        /// </summary>
        public DbSet<Highlight> Highlights { get; set; } = null!;

        /// <summary>
        /// Gets or sets Seasons.
        /// </summary>
        public DbSet<Season> Seasons { get; set; } = null!;

        /// <summary>
        /// Gets or sets Tournaments.
        /// </summary>
        public DbSet<Tournament> Tournaments { get; set; } = null!;

        /// <summary>
        /// Gets or sets TournamentPlayers.
        /// </summary>
        public DbSet<TournamentPlayer> TournamentPlayers { get; set; } = null!;

        /// <summary>
        /// Returns Database object from DbContext.
        /// </summary>
        /// <returns><see cref="DatabaseFacade"/>.</returns>
        public DatabaseFacade GetDatabase()
        {
            return this.Database;
        }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseMySql(this.connectionString, ServerVersion.AutoDetect(this.connectionString));

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>(entity =>
            {
                entity.ToTable("Player");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .IsRequired();

                entity.HasKey(e => e.Id);

                entity.Property(e => e.KeycloakId)
                    .HasMaxLength(200)
                    .HasColumnName("keycloak_id");

                entity.Property(e => e.FullName)
                    .HasColumnName("full_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Nickname)
                    .HasColumnName("nickname")
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ProfilePictureUrl)
                    .HasColumnName("profile_picture_url")
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedOn)
                    .HasDefaultValue(DateTime.UtcNow)
                    .HasColumnName("created_on");

                entity.Property(e => e.Archived)
                    .HasDefaultValue(false)
                    .HasColumnName("archived");
            });

            modelBuilder.Entity<Tournament>(entity =>
            {
                entity.ToTable("Tournament");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .IsRequired();

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(5000);

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .IsRequired()
                    .HasDefaultValue(TournamentStates.Draft);

                entity.Property(e => e.LogoUrl)
                    .HasColumnName("logo_url")
                    .HasMaxLength(3000);

                entity.Property(e => e.Phase2ChallongeUrl)
                    .HasColumnName("phase2_challonge_url")
                    .HasMaxLength(3000);

                entity.Property(e => e.PlannedFrom)
                    .HasColumnName("planned_from")
                    .IsRequired()
                    .HasDefaultValue(DateTime.UtcNow);

                entity.Property(e => e.PlannedTo)
                    .HasColumnName("planned_to")
                    .IsRequired()
                    .HasDefaultValue(DateTime.UtcNow.AddDays(1));

                entity.Property(e => e.WinnerId)
                    .HasColumnName("winner_id");

                entity.HasOne(e => e.Winner)
                    .WithMany(f => f.TournamentsWon)
                    .HasForeignKey(e => e.WinnerId)
                    .HasConstraintName("FK_Tournament_Player_Winner")
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<TournamentPlayer>(entity =>
            {
                entity.ToTable("TournamentPlayer");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .IsRequired();

                entity.HasKey(e => e.Id);

                entity.Property(e => e.PlayerId)
                    .HasColumnName("player_id")
                    .IsRequired();

                entity.Property(e => e.FifaTeamId)
                    .HasColumnName("fifa_team_id")
                    .IsRequired();

                entity.Property(e => e.TournamentId)
                   .HasColumnName("tournament_id")
                   .IsRequired();

                entity.Property(e => e.JoinedAt)
                    .HasColumnName("joined_at")
                    .IsRequired()
                    .HasDefaultValue(DateTime.UtcNow);

                entity.Property(e => e.Phase1Score)
                    .HasColumnName("phase_1_score");

                entity.HasOne(e => e.Player)
                      .WithMany(f => f.TournamentPlayed)
                      .HasForeignKey(e => e.PlayerId)
                      .HasConstraintName("FK_TournamentPlayer_Player")
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Tournament)
                      .WithMany(f => f.Players)
                      .HasForeignKey(e => e.TournamentId)
                      .HasConstraintName("FK_TournamentPlayer_Tournament")
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.FifaTeam)
                      .WithMany(f => f.TournamentPlayers)
                      .HasForeignKey(e => e.FifaTeamId)
                      .HasConstraintName("FK_TournamentPlayer_FifaTeam")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Season>(entity =>
            {
                entity.ToTable("Season");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .IsRequired();

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsRequired();
            });

            modelBuilder.Entity<Platform>(entity =>
            {
                entity.ToTable("Platform");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .IsRequired();

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<FifaTeam>(entity =>
            {
                entity.ToTable("FifaTeam");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .IsRequired();

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<FifaGamePlayed>(entity =>
            {
                entity.ToTable("FifaGamePlayed");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .IsRequired();

                entity.HasKey(e => e.Id);

                entity.Property(e => e.PlayedOn)
                    .HasColumnName("played_on")
                    .HasDefaultValue(DateTime.Now);

                entity.Property(e => e.TeamCode1)
                    .HasColumnName("team_code_1")
                    .HasMaxLength(10);

                entity.Property(e => e.Team1Id)
                    .HasColumnName("team_1_id");

                entity.Property(e => e.Team2Id)
                    .HasColumnName("team_2_id");

                entity.Property(e => e.TeamCode2)
                    .HasColumnName("team_code_2")
                    .HasMaxLength(10);

                entity.Property(e => e.TeamScore1)
                    .HasColumnName("team_score_1")
                    .HasDefaultValue(0)
                    .HasMaxLength(100);

                entity.Property(e => e.TeamScore2)
                    .HasColumnName("team_score_2")
                    .HasDefaultValue(0)
                    .HasMaxLength(100);

                entity.Property(e => e.PlatformId)
                    .HasColumnName("platform_id");

                entity.Property(e => e.Phase)
                    .HasColumnName("phase");

                entity.Property(e => e.CreatedById)
                    .HasColumnName("created_by_id");

                entity.Property(e => e.SeasonId)
                    .HasColumnName("season_id")
                    .IsRequired();

                entity.Property(e => e.IsPlayed)
                    .HasColumnName("is_played")
                    .IsRequired();

                entity.Property(e => e.TournamentId)
                    .HasColumnName("tournament_id");

                entity.HasOne(e => e.Team1)
                    .WithMany(f => f.GamesPlayedTeam1)
                    .HasForeignKey(e => e.Team1Id)
                    .HasConstraintName("FK_FifaGamePlayed_FifaTeam1")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Team2)
                    .WithMany(f => f.GamesPlayedTeam2)
                    .HasForeignKey(e => e.Team2Id)
                    .HasConstraintName("FK_FifaGamePlayed_FifaTeam2")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Platform)
                    .WithMany(f => f.GamesPlayed)
                    .HasForeignKey(e => e.PlatformId)
                    .HasConstraintName("FK_FifaGamePlayed_Platform")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.CreatedBy)
                    .WithMany(f => f.FifaGameCreated)
                    .HasForeignKey(e => e.CreatedById)
                    .HasConstraintName("FK_FifaGamePlayed_Player_Created_By")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Season)
                    .WithMany(f => f.FifaGamePlayed)
                    .HasForeignKey(e => e.SeasonId)
                    .HasConstraintName("FK_FifaGamePlayed_Season")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Tournament)
                    .WithMany(f => f.Games)
                    .HasForeignKey(e => e.TournamentId)
                    .HasConstraintName("FK_FifaGamePlayed_Tournament")
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Highlight>(entity =>
            {
                entity.ToTable("Highlight");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .IsRequired();

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedById)
                    .IsRequired()
                    .HasColumnName("created_by_id");

                entity.Property(e => e.FifaGameId)
                    .IsRequired()
                    .HasColumnName("fifa_game_id");

                entity.Property(e => e.ExternalUrl)
                    .HasColumnName("external_url")
                    .HasMaxLength(3000);

                entity.HasOne(e => e.CreatedBy)
                    .WithMany(f => f.Highlights)
                    .HasForeignKey(e => e.CreatedById)
                    .HasConstraintName("FK_Highlight_Player_Created_By")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.FifaGame)
                    .WithMany(f => f.Highlights)
                    .HasForeignKey(e => e.FifaGameId)
                    .HasConstraintName("FK_FifaGame_Highlight")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<FifaTeamPlayer>(entity =>
            {
                entity.ToTable("FifaTeamPlayer");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .IsRequired();

                entity.HasKey(e => e.Id);

                entity.Property(e => e.PlayerId)
                    .IsRequired()
                    .HasColumnName("player_id");

                entity.Property(e => e.FifaGameId)
                    .IsRequired()
                    .HasColumnName("fifa_game_id");

                entity.Property(e => e.Team)
                    .IsRequired()
                    .HasDefaultValue(0)
                    .HasColumnName("team");

                entity.HasOne(e => e.Player)
                    .WithMany(f => f.FifaTeamPlayers)
                    .HasForeignKey(e => e.PlayerId)
                    .HasConstraintName("FK_FifaTeamPlayer_Player")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.FifaGamePlayed)
                    .WithMany(f => f.TeamPlayers)
                    .HasForeignKey(e => e.FifaGameId)
                    .HasConstraintName("FK_FifaTeamPlayer_FifaGamePlayed");
            });
        }
    }
}