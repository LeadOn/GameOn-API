// <copyright file="YuGamesContext.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuGames.EntitiesContext
{
    using Microsoft.EntityFrameworkCore;
    using YuGames.Common.Exceptions;
    using YuGames.Entities;

    /// <summary>
    /// YuGames database context.
    /// </summary>
    public class YuGamesContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YuGamesContext"/> class.
        /// </summary>
        public YuGamesContext()
        {
            this.DbPath = Environment.GetEnvironmentVariable("SQLITE_PATH") ?? throw new MissingEnvironmentVariableException("SQLITE_PATH");
        }

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
        /// Gets or sets the path of the SQLite file.
        /// </summary>
        public string DbPath { get; set; }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={this.DbPath}");

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>(entity =>
            {
                entity.ToTable("Player");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.KeycloakId)
                    .HasMaxLength(50)
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
            });

            modelBuilder.Entity<Platform>(entity =>
            {
                entity.ToTable("Platform");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

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
                    .HasColumnName("id");

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
                    .HasColumnName("id");

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

                entity.Property(e => e.CreatedById)
                    .HasColumnName("created_by_id");

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
            });

            modelBuilder.Entity<Highlight>(entity =>
            {
                entity.ToTable("Highlight");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

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
                    .HasColumnName("id");

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