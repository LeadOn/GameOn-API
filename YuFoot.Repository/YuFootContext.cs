// <copyright file="YuFootContext.cs" company="LeadOn's Corp'">
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