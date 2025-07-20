// <copyright file="GameOnContext.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Persistence
{
    using GameOn.Common.Exceptions;
    using GameOn.Common.Interfaces;
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
        /// Gets or sets Changelog.
        /// </summary>
        public DbSet<Changelog> Changelogs { get; set; } = null!;

        /// <summary>
        /// Gets or sets LoL Rank History.
        /// </summary>
        public DbSet<LeagueOfLegendsRankHistory> LeagueOfLegendsRankHistory { get; set; } = null!;

        /// <summary>
        /// Gets or sets LoL Game.
        /// </summary>
        public DbSet<LoLGame> LeagueOfLegendsGames { get; set; } = null!;

        /// <summary>
        /// Gets or sets LoL Game Participant.
        /// </summary>
        public DbSet<LoLGameParticipant> LeagueOfLegendsGameParticipants { get; set; } = null!;

        /// <summary>
        /// Gets or sets LoL Game Timeline Frame.
        /// </summary>
        public DbSet<LoLGameTimelineFrame> LeagueOfLegendsGameTimelineFrames { get; set; } = null!;

        /// <summary>
        /// Gets or sets LoL Game Timeline Frame Participant.
        /// </summary>
        public DbSet<LoLGameTimelineFrameParticipant> LeagueOfLegendsGameTimelineFrameParticipants { get; set; } = null!;

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
                    .HasDefaultValue(new DateTime(1999, 5, 21, 00, 00, 00))
                    .HasColumnName("created_on");

                entity.Property(e => e.Archived)
                    .HasDefaultValue(false)
                    .HasColumnName("archived");

                entity.Property(e => e.RiotGamesNickname)
                    .HasColumnName("riot_games_nickname")
                    .HasMaxLength(150);

                entity.Property(e => e.RiotGamesTagLine)
                    .HasColumnName("riot_games_tag_line")
                    .HasMaxLength(10);

                entity.Property(e => e.RiotGamesPUUID)
                    .HasColumnName("riot_games_puuid")
                    .HasMaxLength(150);

                entity.Property(e => e.LolSummonerId)
                    .HasColumnName("lol_summoner_id")
                    .HasMaxLength(150);

                entity.Property(e => e.LolIconId)
                    .HasColumnName("lol_icon_id");

                entity.Property(e => e.LolRefreshedOn)
                    .HasColumnName("lol_refreshed_on");
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
                    .HasMaxLength(5000)
                    .IsRequired();

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
                    .HasDefaultValue(new DateTime(1999, 5, 21, 00, 00, 00));

                entity.Property(e => e.PlannedTo)
                    .HasColumnName("planned_to")
                    .IsRequired()
                    .HasDefaultValue(new DateTime(1999, 5, 21, 00, 00, 00));

                entity.Property(e => e.WinnerId)
                    .HasColumnName("winner_id");

                entity.Property(e => e.Rules)
                    .HasColumnName("rules")
                    .HasMaxLength(5000);

                entity.Property(e => e.WinPoints)
                    .HasColumnName("win_points")
                    .HasDefaultValue(3);

                entity.Property(e => e.LoosePoints)
                    .HasColumnName("loose_points")
                    .HasDefaultValue(0);

                entity.Property(e => e.DrawPoints)
                    .HasColumnName("draw_points")
                    .HasDefaultValue(1);

                entity.Property(e => e.Featured)
                    .HasColumnName("featured")
                    .HasDefaultValue(false);

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
                    .HasDefaultValue(new DateTime(1999, 5, 21, 00, 00, 00));

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

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .IsRequired()
                    .HasDefaultValue(0);
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
                    .HasDefaultValue(new DateTime(1999, 5, 21, 00, 00, 00));

                entity.Property(e => e.Team1Id)
                    .HasColumnName("team_1_id")
                    .HasDefaultValue(1);

                entity.Property(e => e.Team2Id)
                    .HasColumnName("team_2_id")
                    .HasDefaultValue(1);

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

            modelBuilder.Entity<Changelog>(entity =>
            {
                entity.ToTable("Changelog");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .IsRequired();

                entity.HasKey(e => e.Id);

                entity.Property(e => e.PublicationDate)
                    .HasColumnName("publication_date")
                    .HasDefaultValue(new DateTime(1999, 5, 21, 00, 00, 00));

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasDefaultValue(0);

                entity.Property(e => e.Published)
                    .HasColumnName("published")
                    .HasDefaultValue(false);

                entity.Property(e => e.Version)
                    .HasColumnName("version")
                    .HasMaxLength(10);

                entity.Property(e => e.Context)
                    .HasColumnName("context")
                    .HasMaxLength(500);

                entity.Property(e => e.NewFeatures)
                    .HasColumnName("new_features")
                    .HasMaxLength(500);

                entity.Property(e => e.UpdatedFeatures)
                    .HasColumnName("updated_features")
                    .HasMaxLength(500);

                entity.Property(e => e.RemovedFeatures)
                    .HasColumnName("removed_features")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<LeagueOfLegendsRankHistory>(entity =>
            {
                entity.ToTable("LeagueOfLegendsRankHistory");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .IsRequired();

                entity.HasKey(e => e.Id);

                entity.Property(e => e.PlayerId)
                    .HasColumnName("player_id")
                    .IsRequired();

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasDefaultValue(new DateTime(1999, 5, 21, 00, 00, 00));

                entity.Property(e => e.QueueType)
                    .HasColumnName("queue_type")
                    .HasMaxLength(100);

                entity.Property(e => e.Tier)
                    .HasColumnName("tier")
                    .HasMaxLength(10);

                entity.Property(e => e.Rank)
                    .HasColumnName("rank")
                    .HasMaxLength(100);

                entity.Property(e => e.LeaguePoints)
                    .HasColumnName("league_points");

                entity.Property(e => e.Wins)
                    .HasColumnName("wins");

                entity.Property(e => e.Losses)
                    .HasColumnName("losses");

                entity.Property(e => e.HotStreak)
                    .HasColumnName("hot_streak");

                entity.Property(e => e.Veteran)
                    .HasColumnName("veteran");

                entity.Property(e => e.FreshBlood)
                    .HasColumnName("fresh_blood");

                entity.Property(e => e.Inactive)
                    .HasColumnName("inactive");

                entity.HasOne(e => e.Player)
                      .WithMany(f => f.LeagueOfLegendsRankHistory)
                      .HasConstraintName("FK_Player_LoLRankHistory")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LoLGame>(entity =>
            {
                entity.ToTable("LeagueOfLegendsGame");

                entity.Property(e => e.GameId)
                    .HasColumnName("game_id");

                entity.Property(e => e.MatchId)
                    .HasColumnName("match_id")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasKey(e => e.MatchId);

                entity.Property(e => e.EndOfGameResult)
                    .HasColumnName("end_of_game_result")
                    .HasMaxLength(100);

                entity.Property(e => e.GameVersion)
                    .HasColumnName("game_version")
                    .HasMaxLength(100);

                entity.Property(e => e.RetrievedOn)
                    .HasColumnName("retrieved_on")
                    .HasDefaultValue(new DateTime(1999, 5, 21, 00, 00, 00));

                entity.Property(e => e.GameStart)
                    .HasColumnName("game_start")
                    .HasDefaultValue(new DateTime(1999, 5, 21, 00, 00, 00));

                entity.Property(e => e.GameEnd)
                    .HasColumnName("game_end")
                    .HasDefaultValue(new DateTime(1999, 5, 21, 00, 00, 00));

                entity.Property(e => e.WinningTeamId)
                    .HasColumnName("winning_team_id");

                entity.Property(e => e.QueueType)
                    .HasColumnName("queue_type")
                    .HasMaxLength(50);

                entity.HasMany(e => e.LeagueOfLegendsGameParticipants)
                    .WithOne(f => f.Game)
                    .HasForeignKey(f => f.MatchId)
                    .HasConstraintName("FK_LoL_Games_Participants")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LoLGameParticipant>(entity =>
            {
                entity.ToTable("LeagueOfLegendsGameParticipant");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .IsRequired();

                entity.HasKey(e => e.Id);

                entity.Property(e => e.PlayerId)
                    .HasColumnName("player_id");

                entity.HasOne(e => e.Player)
                    .WithMany(f => f.LeagueOfLegendsGameParticipants)
                    .HasForeignKey(e => e.PlayerId)
                    .HasConstraintName("FK_Player_LoL_Game_Participant")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.Puuid)
                    .HasColumnName("puuid")
                    .HasMaxLength(150);

                entity.Property(e => e.RiotIdTagLine)
                    .HasColumnName("riot_id_tagline")
                    .HasMaxLength(10);

                entity.Property(e => e.RiotIdGameName)
                    .HasColumnName("riot_id_game_name")
                    .HasMaxLength(150);

                entity.Property(e => e.ChampionId)
                    .HasColumnName("champion_id");

                entity.Property(e => e.ChampionName)
                    .HasColumnName("champion_name")
                    .HasMaxLength(150);

                entity.Property(e => e.ChampLevel)
                    .HasColumnName("champLevel");

                entity.Property(e => e.TeamId)
                    .HasColumnName("team_id");

                entity.Property(e => e.Kills)
                    .HasColumnName("kills");

                entity.Property(e => e.Deaths)
                    .HasColumnName("deaths");

                entity.Property(e => e.Assists)
                    .HasColumnName("assists");

                entity.Property(e => e.Item0)
                    .HasColumnName("item0");

                entity.Property(e => e.Item1)
                    .HasColumnName("item1");

                entity.Property(e => e.Item2)
                    .HasColumnName("item2");

                entity.Property(e => e.Item3)
                    .HasColumnName("item3");

                entity.Property(e => e.Item4)
                    .HasColumnName("item4");

                entity.Property(e => e.Item5)
                    .HasColumnName("item5");

                entity.Property(e => e.Item6)
                    .HasColumnName("item6");

                entity.Property(e => e.Win)
                    .HasColumnName("win");
            });

            modelBuilder.Entity<LoLGameTimelineFrame>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .IsRequired();

                entity.HasKey(e => e.Id);

                entity.ToTable("LeagueOfLegendsGameTimelineFrame");

                entity.Property(e => e.MatchId)
                    .HasColumnName("match_id");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp");

                entity.HasOne(e => e.Game)
                    .WithMany(f => f.LoLGameTimelineFrames)
                    .HasForeignKey(e => e.MatchId)
                    .HasConstraintName("FK_LoL_Game_Frame")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LoLGameTimelineFrameParticipant>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .IsRequired();

                entity.HasKey(e => e.Id);

                entity.ToTable("LeagueOfLegendsGameTimelineFrameParticipant");

                entity.Property(e => e.LoLGameTimelineFrameId)
                    .HasColumnName("timeline_frame_id");

                entity.Property(e => e.CurrentGold)
                    .HasColumnName("current_gold");

                entity.Property(e => e.GoldPerSecond)
                    .HasColumnName("gold_per_second");

                entity.Property(e => e.JungleMinionsKilled)
                    .HasColumnName("jungle_minions_killed");

                entity.Property(e => e.Level)
                    .HasColumnName("level");

                entity.Property(e => e.MinionsKilled)
                    .HasColumnName("minions_killed");

                entity.Property(e => e.ParticipantId)
                    .HasColumnName("participantId");

                entity.Property(e => e.ParticipantPUUID)
                    .HasColumnName("participantPuuid");

                entity.Property(e => e.TimeEnemySpentControlled)
                    .HasColumnName("time_enemy_spent_controlled");

                entity.Property(e => e.TotalGold)
                    .HasColumnName("total_gold");

                entity.Property(e => e.Xp)
                    .HasColumnName("xp");

                entity.Property(e => e.MagicDamageDone)
                    .HasColumnName("magic_damage_done");

                entity.Property(e => e.MagicDamageDoneToChampions)
                    .HasColumnName("magic_damage_done_to_champions");

                entity.Property(e => e.MagicDamageTaken)
                    .HasColumnName("magic_damage_taken");

                entity.Property(e => e.PhysicalDamageDone)
                    .HasColumnName("physical_damage_done");

                entity.Property(e => e.PhysicalDamageDoneToChampions)
                    .HasColumnName("physical_damage_done_to_champions");

                entity.Property(e => e.PhysicalDamageTaken)
                    .HasColumnName("physical_damage_taken");

                entity.Property(e => e.TotalDamageDone)
                    .HasColumnName("total_damage_done");

                entity.Property(e => e.TotalDamageDoneToChampions)
                    .HasColumnName("total_damage_done_to_champions");

                entity.Property(e => e.TotalDamageTaken)
                    .HasColumnName("total_damage_taken");

                entity.Property(e => e.TrueDamageDone)
                    .HasColumnName("true_damage_done");

                entity.Property(e => e.TrueDamageDoneToChampions)
                    .HasColumnName("true_damage_done_to_champions");

                entity.Property(e => e.TrueDamageTaken)
                    .HasColumnName("true_damage_taken");

                entity.HasOne(e => e.TimelineFrame)
                    .WithMany(f => f.LoLGameTimelineFrameParticipants)
                    .HasForeignKey(e => e.LoLGameTimelineFrameId)
                    .HasConstraintName("FK_LoL_Game_Frame_Participant")
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}