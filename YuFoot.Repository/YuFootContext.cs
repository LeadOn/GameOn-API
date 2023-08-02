﻿// <copyright file="YuFootContext.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.Repository
{
    using Microsoft.EntityFrameworkCore;
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
        /// Gets or sets the path of the SQLite file.
        /// </summary>
        public string DbPath { get; set; }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={this.DbPath}");

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

                entity.Property(e => e.TeamCode2)
                    .HasColumnName("team_code_2")
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.ToTable("Player");

                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd()
                      .HasColumnName("id");

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