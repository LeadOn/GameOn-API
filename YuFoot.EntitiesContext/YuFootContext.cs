// <copyright file="YuFootContext.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.EntitiesContext
{
    using Microsoft.EntityFrameworkCore;
    using YuFoot.Common.Exceptions;
    using YuFoot.Entities;

    /// <summary>
    /// YuFoot database context.
    /// </summary>
    public class YuFootContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YuFootContext"/> class.
        /// </summary>
        public YuFootContext()
        {
            this.DbPath = Environment.GetEnvironmentVariable("SQLITE_PATH") ?? "/Users/leadon/Desktop/yufoot.db";
        }

        /// <summary>
        /// Gets or sets Players.
        /// </summary>
        public DbSet<Player> Players { get; set; }

        /// <summary>
        /// Gets or sets GamePlayed.
        /// </summary>
        public DbSet<GamePlayed> GamesPlayed { get; set; }

        /// <summary>
        /// Gets or sets TeamPlayer.
        /// </summary>
        public DbSet<TeamPlayer> TeamPlayers { get; set; }

        /// <summary>
        /// Gets or sets Platforms.
        /// </summary>
        public DbSet<Platform> Platforms { get; set; }

        /// <summary>
        /// Gets or sets Highlights.
        /// </summary>
        public DbSet<Highlight> Highlights { get; set; }

        /// <summary>
        /// Gets or sets FIFA Teams.
        /// </summary>
        public DbSet<FifaTeam> FifaTeams { get; set; }

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
            modelBuilder.Entity<FifaTeam>(entity =>
            {
                entity.ToTable("FifaTeam");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.HasMany(e => e.GamesPlayedTeam1)
                    .WithOne(f => f.Team1)
                    .HasForeignKey(e => e.Team1Id)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("FK_FifaTeam_GamePlayed_Team1");

                entity.HasMany(e => e.GamesPlayedTeam2)
                    .WithOne(f => f.Team2)
                    .HasForeignKey(e => e.Team2Id)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("FK_FifaTeam_GamePlayed_Team2");
            });

            modelBuilder.Entity<Highlight>(entity =>
            {
                entity.ToTable("Highlight");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.ExternalUrl)
                    .HasColumnName("external_url")
                    .HasMaxLength(3000);

                entity.HasOne(e => e.CreatedBy)
                    .WithMany(f => f.Highlights)
                    .HasForeignKey(e => e.CreatedById)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("FK_Highlight_Player");

                entity.HasOne(e => e.Game)
                    .WithMany(f => f.Highlights)
                    .HasForeignKey(e => e.GameId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("FK_Highlight_GamePlayed");
            });

            modelBuilder.Entity<Platform>(entity =>
            {
                entity.ToTable("Platform");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TeamPlayer>(entity =>
            {
                entity.ToTable("TeamPlayer");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.PlayerId)
                    .HasColumnName("player_id");

                entity.Property(e => e.Team)
                    .HasColumnName("team");

                entity.HasOne(e => e.Player)
                    .WithMany(f => f.TeamPlayers)
                    .HasForeignKey(e => e.PlayerId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("FK_Player_TeamPlayer");

                entity.HasOne(e => e.GamePlayed)
                    .WithMany(f => f.TeamPlayers)
                    .HasForeignKey(e => e.GamePlayedId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("FK_GamePlayed_TeamPlayer");
            });

            modelBuilder.Entity<GamePlayed>(entity =>
            {
                entity.ToTable("GamePlayed");

                entity.HasOne(e => e.CreatedBy)
                    .WithMany(f => f.GamesCreated)
                    .HasForeignKey(e => e.CreatedById)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("FK_GamePlayed_CreatedBy");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.PlayedOn)
                    .HasColumnName("played_on")
                    .HasDefaultValue(DateTime.Now);

                entity.Property(e => e.TeamScore1)
                    .HasColumnName("team_score_1")
                    .HasMaxLength(100);

                entity.Property(e => e.TeamScore2)
                    .HasColumnName("team_score_2")
                    .HasMaxLength(100);

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

                entity.HasOne(e => e.Platform)
                    .WithMany(f => f.GamesPlayed)
                    .HasForeignKey(e => e.PlatformId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("FK_GamePlayed_Platform");
            });

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
                      .HasMaxLength(100);

                entity.Property(e => e.ProfilePictureUrl)
                      .HasColumnName("profile_picture_url")
                      .HasMaxLength(500);

                entity.Property(e => e.CreatedOn)
                      .HasDefaultValue(DateTime.Now)
                      .HasColumnName("created_on");
            });
        }
    }
}