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
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
        /// Gets or sets LoL Game Participant Stats.
        /// </summary>
        public DbSet<LoLGameParticipantStat> LeagueOfLegendsGameParticipantStats { get; set; } = null!;

        /// <summary>
        /// Gets or sets LoL Game Participant Challenges.
        /// </summary>
        public DbSet<LoLGameParticipantChallenge> LeagueOfLegendsGameParticipantChallenges { get; set; } = null!;

        /// <summary>
        /// Gets or sets LoL Game Participant Rank Changes.
        /// </summary>
        public DbSet<LoLGameParticipantRankChange> LeagueOfLegendsGameParticipantRankChanges { get; set; } = null!;

        /// <summary>
        /// Gets or sets LoL Game Coach Reports.
        /// </summary>
        public DbSet<LoLGameCoachReport> LeagueOfLegendsGameCoachReports { get; set; } = null!;

        /// <summary>
        /// Gets or sets LoL Queues.
        /// </summary>
        public DbSet<LoLQueue> LeagueOfLegendsQueues { get; set; } = null!;

        /// <summary>
        /// Gets or sets LoL Game Timeline Events.
        /// </summary>
        public DbSet<LoLGameTimelineEvent> LeagueOfLegendsGameTimelineEvents { get; set; } = null!;

        /// <summary>
        /// Gets or sets LoL Game Timeline Event Assists.
        /// </summary>
        public DbSet<LoLGameTimelineEventAssist> LeagueOfLegendsGameTimelineEventAssists { get; set; } = null!;

        /// <summary>
        /// Gets or sets LoL Game Teams.
        /// </summary>
        public DbSet<LoLGameTeam> LeagueOfLegendsGameTeams { get; set; } = null!;

        /// <summary>
        /// Gets or sets Valorant Rank History.
        /// </summary>
        public DbSet<ValorantRankHistory> ValorantRankHistory { get; set; } = null!;

        /// <summary>
        /// Gets or sets Valorant Games.
        /// </summary>
        public DbSet<ValGame> ValorantGames { get; set; } = null!;

        /// <summary>
        /// Gets or sets Valorant Game Teams.
        /// </summary>
        public DbSet<ValGameTeam> ValorantGameTeams { get; set; } = null!;

        /// <summary>
        /// Gets or sets Valorant Game Participants.
        /// </summary>
        public DbSet<ValGameParticipant> ValorantGameParticipants { get; set; } = null!;

        /// <summary>
        /// Gets or sets Valorant Game Participant Stats.
        /// </summary>
        public DbSet<ValGameParticipantStat> ValorantGameParticipantStats { get; set; } = null!;

        /// <summary>
        /// Gets or sets Valorant Game Rounds.
        /// </summary>
        public DbSet<ValGameRound> ValorantGameRounds { get; set; } = null!;

        /// <summary>
        /// Gets or sets Valorant Game Kills.
        /// </summary>
        public DbSet<ValGameKill> ValorantGameKills { get; set; } = null!;

        /// <summary>
        /// Gets or sets Valorant Game Round Player Stats.
        /// </summary>
        public DbSet<ValGameRoundPlayerStat> ValorantGameRoundPlayerStats { get; set; } = null!;

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
            => options.UseSqlServer(this.connectionString);

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

                // No HasDefaultValue here on purpose: EF omits a property equal to its CLR default from
                // the INSERT, so a database default of true would silently drag every new account into
                // the crew. Existing rows were flipped to true by the migration instead.
                entity.Property(e => e.InCrew)
                    .HasColumnName("in_crew");

                entity.Property(e => e.RiotGamesNickname)
                    .HasColumnName("riot_games_nickname")
                    .HasMaxLength(150);

                entity.Property(e => e.RiotGamesTagLine)
                    .HasColumnName("riot_games_tag_line")
                    .HasMaxLength(10);

                entity.Property(e => e.RiotGamesPUUID)
                    .HasColumnName("riot_games_puuid")
                    .HasMaxLength(150);

                entity.Property(e => e.LolIconId)
                    .HasColumnName("lol_icon_id");

                entity.Property(e => e.LolRefreshedOn)
                    .HasColumnName("lol_refreshed_on");

                entity.Property(e => e.ValorantPuuid)
                    .HasColumnName("valorant_puuid")
                    .HasMaxLength(100);

                entity.Property(e => e.ValorantAccountLevel)
                    .HasColumnName("valorant_account_level");

                entity.Property(e => e.ValorantPlayerCardId)
                    .HasColumnName("valorant_player_card_id")
                    .HasMaxLength(100);

                entity.Property(e => e.ValorantPlayerTitleId)
                    .HasColumnName("valorant_player_title_id")
                    .HasMaxLength(100);

                entity.Property(e => e.ValorantRegion)
                    .HasColumnName("valorant_region")
                    .HasMaxLength(20);

                entity.Property(e => e.ValorantRefreshedOn)
                    .HasColumnName("valorant_refreshed_on");

                entity.Property(e => e.ValorantPeakTierId)
                    .HasColumnName("valorant_peak_tier_id");

                entity.Property(e => e.ValorantPeakTierName)
                    .HasColumnName("valorant_peak_tier_name")
                    .HasMaxLength(50);

                entity.Property(e => e.ValorantPeakRr)
                    .HasColumnName("valorant_peak_rr");

                entity.Property(e => e.ValorantPeakSeasonShort)
                    .HasColumnName("valorant_peak_season_short")
                    .HasMaxLength(20);

                entity.Property(e => e.PrimaryPlayerId)
                    .HasColumnName("primary_player_id");

                entity.HasIndex(e => e.PrimaryPlayerId);

                // Self-referencing: a smurf account points at the primary account it belongs to.
                // Restrict rather than cascade, both because SQL Server refuses cascades on a self
                // reference and because unlinking a smurf must never delete its games and rank history.
                entity.HasOne(e => e.PrimaryPlayer)
                    .WithMany(e => e.SmurfAccounts)
                    .HasForeignKey(e => e.PrimaryPlayerId)
                    .OnDelete(DeleteBehavior.Restrict);
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

                entity.Property(e => e.PhaseOneDoubleRound)
                    .HasColumnName("phase_one_double_round")
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
                      .OnDelete(DeleteBehavior.Restrict);
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
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Team2)
                    .WithMany(f => f.GamesPlayedTeam2)
                    .HasForeignKey(e => e.Team2Id)
                    .HasConstraintName("FK_FifaGamePlayed_FifaTeam2")
                    .OnDelete(DeleteBehavior.Restrict);

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
                    .OnDelete(DeleteBehavior.Cascade);
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
                    .HasConstraintName("FK_FifaTeamPlayer_FifaGamePlayed")
                    .OnDelete(DeleteBehavior.Cascade);
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

            modelBuilder.Entity<ValorantRankHistory>(entity =>
            {
                entity.ToTable("ValorantRankHistory");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .IsRequired();

                entity.HasKey(e => e.Id);

                entity.Property(e => e.PlayerId)
                    .HasColumnName("player_id")
                    .IsRequired();

                entity.Property(e => e.MatchId)
                    .HasColumnName("match_id")
                    .HasMaxLength(100);

                entity.Property(e => e.SeasonId)
                    .HasColumnName("season_id")
                    .HasMaxLength(100);

                entity.Property(e => e.SeasonShort)
                    .HasColumnName("season_short")
                    .HasMaxLength(20);

                entity.Property(e => e.TierId)
                    .HasColumnName("tier_id");

                entity.Property(e => e.TierName)
                    .HasColumnName("tier_name")
                    .HasMaxLength(50);

                entity.Property(e => e.RankedRating)
                    .HasColumnName("ranked_rating");

                entity.Property(e => e.Elo)
                    .HasColumnName("elo");

                entity.Property(e => e.LastChange)
                    .HasColumnName("last_change");

                entity.Property(e => e.RefundedRr)
                    .HasColumnName("refunded_rr");

                entity.Property(e => e.WasDerankProtected)
                    .HasColumnName("was_derank_protected");

                entity.Property(e => e.MapId)
                    .HasColumnName("map_id")
                    .HasMaxLength(200);

                entity.Property(e => e.MapName)
                    .HasColumnName("map_name")
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasDefaultValue(new DateTime(1999, 5, 21, 00, 00, 00));

                entity.HasIndex(e => new { e.PlayerId, e.CreatedOn });
                entity.HasIndex(e => new { e.PlayerId, e.MatchId });

                entity.HasOne(e => e.Player)
                    .WithMany(f => f.ValorantRankHistory)
                    .HasForeignKey(e => e.PlayerId)
                    .HasConstraintName("FK_Player_ValorantRankHistory")
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

                entity.Property(e => e.IsRemake)
                    .HasColumnName("is_remake")
                    .HasDefaultValue(false);

                entity.Property(e => e.QueueId)
                    .HasColumnName("queue_id");

                entity.Property(e => e.FrameInterval)
                    .HasColumnName("frame_interval");

                entity.Property(e => e.MvpParticipantId)
                    .HasColumnName("mvp_participant_id");

                entity.Property(e => e.AceParticipantId)
                    .HasColumnName("ace_participant_id");

                entity.HasMany(e => e.LeagueOfLegendsGameParticipants)
                    .WithOne(f => f.Game)
                    .HasForeignKey(f => f.MatchId)
                    .HasConstraintName("FK_LoL_Games_Participants")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Queue)
                    .WithMany(f => f.LeagueOfLegendsGames)
                    .HasForeignKey(e => e.QueueId)
                    .HasConstraintName("FK_LoLGame_LoLQueue")
                    .OnDelete(DeleteBehavior.SetNull);

                // Restrict (not Cascade/SetNull): LoLGame already cascades to LoLGameParticipant via
                // FK_LoL_Games_Participants above, SQL Server refuses a second path back to the same
                // table. UpdateLoLGameCommandHandler nulls both columns before removing old participants.
                entity.HasOne<LoLGameParticipant>()
                    .WithMany()
                    .HasForeignKey(e => e.MvpParticipantId)
                    .HasConstraintName("FK_LoLGame_Mvp_Participant")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<LoLGameParticipant>()
                    .WithMany()
                    .HasForeignKey(e => e.AceParticipantId)
                    .HasConstraintName("FK_LoLGame_Ace_Participant")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<LoLGameTeam>(entity =>
            {
                entity.ToTable("LeagueOfLegendsGameTeam");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .IsRequired();

                entity.HasKey(e => e.Id);

                entity.Property(e => e.MatchId)
                    .HasColumnName("match_id")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.TeamId)
                    .HasColumnName("team_id");

                entity.Property(e => e.Win)
                    .HasColumnName("win");

                entity.Property(e => e.ChampionKills)
                    .HasColumnName("champion_kills");

                entity.Property(e => e.TowerKills)
                    .HasColumnName("tower_kills");

                entity.Property(e => e.InhibitorKills)
                    .HasColumnName("inhibitor_kills");

                entity.Property(e => e.DragonKills)
                    .HasColumnName("dragon_kills");

                entity.Property(e => e.RiftHeraldKills)
                    .HasColumnName("rift_herald_kills");

                entity.Property(e => e.BaronKills)
                    .HasColumnName("baron_kills");

                entity.Property(e => e.HordeKills)
                    .HasColumnName("horde_kills");

                entity.Property(e => e.FirstBlood)
                    .HasColumnName("first_blood");

                entity.Property(e => e.FirstTower)
                    .HasColumnName("first_tower");

                entity.Property(e => e.FirstInhibitor)
                    .HasColumnName("first_inhibitor");

                entity.Property(e => e.FirstDragon)
                    .HasColumnName("first_dragon");

                entity.Property(e => e.FirstBaron)
                    .HasColumnName("first_baron");

                entity.Property(e => e.FirstRiftHerald)
                    .HasColumnName("first_rift_herald");

                entity.Property(e => e.FirstHorde)
                    .HasColumnName("first_horde");

                entity.HasOne(e => e.Game)
                    .WithMany(f => f.LeagueOfLegendsGameTeams)
                    .HasForeignKey(e => e.MatchId)
                    .HasConstraintName("FK_LoL_Game_Team")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LoLQueue>(entity =>
            {
                entity.ToTable("LeagueOfLegendsQueue");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever()
                    .IsRequired();

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Map)
                    .HasColumnName("map")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(200);

                entity.Property(e => e.Notes)
                    .HasColumnName("notes")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<LoLGameParticipant>(entity =>
            {
                entity.ToTable("LeagueOfLegendsGameParticipant");

                // Not persisted directly: raw Riot DTO carrier, mapped field-by-field onto LoLGameParticipantChallenge instead.
                entity.Ignore(e => e.RiotChallenges);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .IsRequired();

                entity.HasKey(e => e.Id);

                entity.Property(e => e.PlayerId)
                    .HasColumnName("player_id");

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

                entity.HasAlternateKey(e => new { e.MatchId, e.Puuid })
                    .HasName("AK_LoLGameParticipant_MatchId_Puuid");

                entity.HasOne(e => e.Player)
                    .WithMany(f => f.LeagueOfLegendsGameParticipants)
                    .HasForeignKey(e => e.PlayerId)
                    .HasConstraintName("FK_Player_LoL_Game_Participant")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LoLGameParticipantStat>(entity =>
            {
                entity.ToTable("LeagueOfLegendsGameParticipantStat");

                entity.Property(e => e.LoLGameParticipantId)
                    .ValueGeneratedNever()
                    .HasColumnName("lol_game_participant_id")
                    .IsRequired();

                entity.HasKey(e => e.LoLGameParticipantId);

                entity.Property(e => e.GameDurationSeconds)
                    .HasColumnName("game_duration_seconds");

                entity.Property(e => e.Kda)
                    .HasColumnName("kda");

                entity.Property(e => e.KillParticipationPercent)
                    .HasColumnName("kill_participation_percent");

                entity.Property(e => e.CreepScore)
                    .HasColumnName("creep_score");

                entity.Property(e => e.CsPerMinute)
                    .HasColumnName("cs_per_minute");

                entity.Property(e => e.GoldEarned)
                    .HasColumnName("gold_earned");

                entity.Property(e => e.GoldPerMinute)
                    .HasColumnName("gold_per_minute");

                entity.Property(e => e.DamageDealtToChampions)
                    .HasColumnName("damage_dealt_to_champions");

                entity.Property(e => e.DamagePerMinute)
                    .HasColumnName("damage_per_minute");

                entity.Property(e => e.DamageTaken)
                    .HasColumnName("damage_taken");

                entity.Property(e => e.WardsPlaced)
                    .HasColumnName("wards_placed");

                entity.Property(e => e.WardsKilled)
                    .HasColumnName("wards_killed");

                entity.Property(e => e.PhysicalDamageToChampions)
                    .HasColumnName("physical_damage_to_champions");

                entity.Property(e => e.MagicDamageToChampions)
                    .HasColumnName("magic_damage_to_champions");

                entity.Property(e => e.TrueDamageToChampions)
                    .HasColumnName("true_damage_to_champions");

                entity.Property(e => e.TimeCcOthersSeconds)
                    .HasColumnName("time_cc_others_seconds");

                entity.Property(e => e.Rating)
                    .HasColumnName("rating");

                entity.Property(e => e.ComputedOn)
                    .HasColumnName("computed_on");

                entity.HasOne(e => e.Participant)
                    .WithOne(p => p.Stats)
                    .HasForeignKey<LoLGameParticipantStat>(e => e.LoLGameParticipantId)
                    .HasConstraintName("FK_LoL_Game_Participant_Stat")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LoLGameCoachReport>(entity =>
            {
                entity.ToTable("LeagueOfLegendsGameCoachReport");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .IsRequired();

                entity.HasKey(e => e.Id);

                entity.Property(e => e.MatchId)
                    .HasColumnName("match_id")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Puuid)
                    .HasColumnName("puuid")
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.PlayerId)
                    .HasColumnName("player_id");

                entity.Property(e => e.PromptVersion)
                    .HasColumnName("prompt_version");

                entity.Property(e => e.ModelName)
                    .HasColumnName("model_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Summary)
                    .HasColumnName("summary")
                    .HasMaxLength(2000);

                entity.Property(e => e.Rating)
                    .HasColumnName("rating");

                entity.Property(e => e.ContentJson)
                    .HasColumnName("content_json");

                entity.Property(e => e.GeneratedOn)
                    .HasColumnName("generated_on");

                // One report per player per game. This is what makes the table a cache rather than a log:
                // asking twice for the same analysis serves the stored one instead of paying for it again.
                entity.HasIndex(e => new { e.MatchId, e.Puuid })
                    .IsUnique()
                    .HasDatabaseName("IX_LoL_Game_Coach_Report_Match_Puuid");

                entity.HasOne(e => e.Game)
                    .WithMany()
                    .HasForeignKey(e => e.MatchId)
                    .HasConstraintName("FK_LoL_Game_Coach_Report_Game")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Player)
                    .WithMany()
                    .HasForeignKey(e => e.PlayerId)
                    .HasConstraintName("FK_LoL_Game_Coach_Report_Player")
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<LoLGameParticipantChallenge>(entity =>
            {
                entity.ToTable("LeagueOfLegendsGameParticipantChallenge");

                entity.Property(e => e.LoLGameParticipantId)
                    .ValueGeneratedNever()
                    .HasColumnName("lol_game_participant_id")
                    .IsRequired();

                entity.HasKey(e => e.LoLGameParticipantId);

                entity.Property(e => e.OneTwoAssistStreakCount).HasColumnName("one_two_assist_streak_count");
                entity.Property(e => e.BaronBuffGoldAdvantageOverThreshold).HasColumnName("baron_buff_gold_advantage_over_threshold");
                entity.Property(e => e.ControlWardTimeCoverageInRiverOrEnemyHalf).HasColumnName("control_ward_time_coverage_in_river_or_enemy_half");
                entity.Property(e => e.EarliestBaron).HasColumnName("earliest_baron");
                entity.Property(e => e.EarliestDragonTakedown).HasColumnName("earliest_dragon_takedown");
                entity.Property(e => e.EarliestElderDragon).HasColumnName("earliest_elder_dragon");
                entity.Property(e => e.EarlyLaningPhaseGoldExpAdvantage).HasColumnName("early_laning_phase_gold_exp_advantage");
                entity.Property(e => e.FasterSupportQuestCompletion).HasColumnName("faster_support_quest_completion");
                entity.Property(e => e.FastestLegendary).HasColumnName("fastest_legendary");
                entity.Property(e => e.HadAfkTeammate).HasColumnName("had_afk_teammate");
                entity.Property(e => e.HighestChampionDamage).HasColumnName("highest_champion_damage");
                entity.Property(e => e.HighestCrowdControlScore).HasColumnName("highest_crowd_control_score");
                entity.Property(e => e.HighestWardKills).HasColumnName("highest_ward_kills");
                entity.Property(e => e.JunglerKillsEarlyJungle).HasColumnName("jungler_kills_early_jungle");
                entity.Property(e => e.KillsOnLanersEarlyJungleAsJungler).HasColumnName("kills_on_laners_early_jungle_as_jungler");
                entity.Property(e => e.LaningPhaseGoldExpAdvantage).HasColumnName("laning_phase_gold_exp_advantage");
                entity.Property(e => e.LegendaryCount).HasColumnName("legendary_count");
                entity.Property(e => e.MaxCsAdvantageOnLaneOpponent).HasColumnName("max_cs_advantage_on_lane_opponent");
                entity.Property(e => e.MaxLevelLeadLaneOpponent).HasColumnName("max_level_lead_lane_opponent");
                entity.Property(e => e.MostWardsDestroyedOneSweeper).HasColumnName("most_wards_destroyed_one_sweeper");
                entity.Property(e => e.MythicItemUsed).HasColumnName("mythic_item_used");
                entity.Property(e => e.PlayedChampSelectPosition).HasColumnName("played_champ_select_position");
                entity.Property(e => e.SoloTurretsLategame).HasColumnName("solo_turrets_lategame");
                entity.Property(e => e.TakedownsFirst25Minutes).HasColumnName("takedowns_first_25_minutes");
                entity.Property(e => e.TeleportTakedowns).HasColumnName("teleport_takedowns");
                entity.Property(e => e.ThirdInhibitorDestroyedTime).HasColumnName("third_inhibitor_destroyed_time");
                entity.Property(e => e.ThreeWardsOneSweeperCount).HasColumnName("three_wards_one_sweeper_count");
                entity.Property(e => e.VisionScoreAdvantageLaneOpponent).HasColumnName("vision_score_advantage_lane_opponent");
                entity.Property(e => e.InfernalScalePickup).HasColumnName("infernal_scale_pickup");
                entity.Property(e => e.FistBumpParticipation).HasColumnName("fist_bump_participation");
                entity.Property(e => e.VoidMonsterKill).HasColumnName("void_monster_kill");
                entity.Property(e => e.AbilityUses).HasColumnName("ability_uses");
                entity.Property(e => e.AcesBefore15Minutes).HasColumnName("aces_before_15_minutes");
                entity.Property(e => e.AlliedJungleMonsterKills).HasColumnName("allied_jungle_monster_kills");
                entity.Property(e => e.BaronTakedowns).HasColumnName("baron_takedowns");
                entity.Property(e => e.BlastConeOppositeOpponentCount).HasColumnName("blast_cone_opposite_opponent_count");
                entity.Property(e => e.BountyGold).HasColumnName("bounty_gold");
                entity.Property(e => e.BuffsStolen).HasColumnName("buffs_stolen");
                entity.Property(e => e.CompleteSupportQuestInTime).HasColumnName("complete_support_quest_in_time");
                entity.Property(e => e.ControlWardsPlaced).HasColumnName("control_wards_placed");
                entity.Property(e => e.DamagePerMinute).HasColumnName("damage_per_minute");
                entity.Property(e => e.DamageTakenOnTeamPercentage).HasColumnName("damage_taken_on_team_percentage");
                entity.Property(e => e.DancedWithRiftHerald).HasColumnName("danced_with_rift_herald");
                entity.Property(e => e.DeathsByEnemyChamps).HasColumnName("deaths_by_enemy_champs");
                entity.Property(e => e.DodgeSkillShotsSmallWindow).HasColumnName("dodge_skill_shots_small_window");
                entity.Property(e => e.DoubleAces).HasColumnName("double_aces");
                entity.Property(e => e.DragonTakedowns).HasColumnName("dragon_takedowns");
                entity.Property(e => e.EffectiveHealAndShielding).HasColumnName("effective_heal_and_shielding");
                entity.Property(e => e.ElderDragonKillsWithOpposingSoul).HasColumnName("elder_dragon_kills_with_opposing_soul");
                entity.Property(e => e.ElderDragonMultikills).HasColumnName("elder_dragon_multikills");
                entity.Property(e => e.EnemyChampionImmobilizations).HasColumnName("enemy_champion_immobilizations");
                entity.Property(e => e.EnemyJungleMonsterKills).HasColumnName("enemy_jungle_monster_kills");
                entity.Property(e => e.EpicMonsterKillsNearEnemyJungler).HasColumnName("epic_monster_kills_near_enemy_jungler");
                entity.Property(e => e.EpicMonsterKillsWithin30SecondsOfSpawn).HasColumnName("epic_monster_kills_within_30_seconds_of_spawn");
                entity.Property(e => e.EpicMonsterSteals).HasColumnName("epic_monster_steals");
                entity.Property(e => e.EpicMonsterStolenWithoutSmite).HasColumnName("epic_monster_stolen_without_smite");
                entity.Property(e => e.FlawlessAces).HasColumnName("flawless_aces");
                entity.Property(e => e.FullTeamTakedown).HasColumnName("full_team_takedown");
                entity.Property(e => e.GameLength).HasColumnName("game_length");
                entity.Property(e => e.GoldPerMinute).HasColumnName("gold_per_minute");
                entity.Property(e => e.HadOpenNexus).HasColumnName("had_open_nexus");
                entity.Property(e => e.ImmobilizeAndKillWithAlly).HasColumnName("immobilize_and_kill_with_ally");
                entity.Property(e => e.JungleCsBefore10Minutes).HasColumnName("jungle_cs_before_10_minutes");
                entity.Property(e => e.JunglerTakedownsNearDamagedEpicMonster).HasColumnName("jungler_takedowns_near_damaged_epic_monster");
                entity.Property(e => e.Kda).HasColumnName("kda");
                entity.Property(e => e.KillAfterHiddenWithAlly).HasColumnName("kill_after_hidden_with_ally");
                entity.Property(e => e.KillParticipation).HasColumnName("kill_participation");
                entity.Property(e => e.KillsNearEnemyTurret).HasColumnName("kills_near_enemy_turret");
                entity.Property(e => e.KillsOnOtherLanesEarlyJungleAsLaner).HasColumnName("kills_on_other_lanes_early_jungle_as_laner");
                entity.Property(e => e.KillsUnderOwnTurret).HasColumnName("kills_under_own_turret");
                entity.Property(e => e.KillsWithHelpFromEpicMonster).HasColumnName("kills_with_help_from_epic_monster");
                entity.Property(e => e.KnockEnemyIntoTeamAndKill).HasColumnName("knock_enemy_into_team_and_kill");
                entity.Property(e => e.KTurretsDestroyedBeforePlatesFall).HasColumnName("k_turrets_destroyed_before_plates_fall");
                entity.Property(e => e.LandSkillShotsEarlyGame).HasColumnName("land_skill_shots_early_game");
                entity.Property(e => e.LaneMinionsFirst10Minutes).HasColumnName("lane_minions_first_10_minutes");
                entity.Property(e => e.LostAnInhibitor).HasColumnName("lost_an_inhibitor");
                entity.Property(e => e.MaxKillDeficit).HasColumnName("max_kill_deficit");
                entity.Property(e => e.MejaisFullStackInTime).HasColumnName("mejais_full_stack_in_time");
                entity.Property(e => e.MoreEnemyJungleThanOpponent).HasColumnName("more_enemy_jungle_than_opponent");
                entity.Property(e => e.MultiKillOneSpell).HasColumnName("multi_kill_one_spell");
                entity.Property(e => e.Multikills).HasColumnName("multikills");
                entity.Property(e => e.MultikillsAfterAggressiveFlash).HasColumnName("multikills_after_aggressive_flash");
                entity.Property(e => e.MultiTurretRiftHeraldCount).HasColumnName("multi_turret_rift_herald_count");
                entity.Property(e => e.OuterTurretExecutesBefore10Minutes).HasColumnName("outer_turret_executes_before_10_minutes");
                entity.Property(e => e.OutnumberedKills).HasColumnName("outnumbered_kills");
                entity.Property(e => e.OutnumberedNexusKill).HasColumnName("outnumbered_nexus_kill");
                entity.Property(e => e.PerfectDragonSoulsTaken).HasColumnName("perfect_dragon_souls_taken");
                entity.Property(e => e.PerfectGame).HasColumnName("perfect_game");
                entity.Property(e => e.PickKillWithAlly).HasColumnName("pick_kill_with_ally");
                entity.Property(e => e.PoroExplosions).HasColumnName("poro_explosions");
                entity.Property(e => e.QuickCleanse).HasColumnName("quick_cleanse");
                entity.Property(e => e.QuickFirstTurret).HasColumnName("quick_first_turret");
                entity.Property(e => e.RiftHeraldTakedowns).HasColumnName("rift_herald_takedowns");
                entity.Property(e => e.SaveAllyFromDeath).HasColumnName("save_ally_from_death");
                entity.Property(e => e.ScuttleCrabKills).HasColumnName("scuttle_crab_kills");
                entity.Property(e => e.SkillshotsDodged).HasColumnName("skillshots_dodged");
                entity.Property(e => e.SkillshotsHit).HasColumnName("skillshots_hit");
                entity.Property(e => e.SnowballsHit).HasColumnName("snowballs_hit");
                entity.Property(e => e.SoloBaronKills).HasColumnName("solo_baron_kills");
                entity.Property(e => e.SoloKills).HasColumnName("solo_kills");
                entity.Property(e => e.StealthWardsPlaced).HasColumnName("stealth_wards_placed");
                entity.Property(e => e.SurvivedSingleDigitHpCount).HasColumnName("survived_single_digit_hp_count");
                entity.Property(e => e.SurvivedThreeImmobilizesInFight).HasColumnName("survived_three_immobilizes_in_fight");
                entity.Property(e => e.TakedownOnFirstTurret).HasColumnName("takedown_on_first_turret");
                entity.Property(e => e.Takedowns).HasColumnName("takedowns");
                entity.Property(e => e.TakedownsAfterGainingLevelAdvantage).HasColumnName("takedowns_after_gaining_level_advantage");
                entity.Property(e => e.TakedownsBeforeJungleMinionSpawn).HasColumnName("takedowns_before_jungle_minion_spawn");
                entity.Property(e => e.TakedownsInEnemyFountain).HasColumnName("takedowns_in_enemy_fountain");
                entity.Property(e => e.TeamBaronKills).HasColumnName("team_baron_kills");
                entity.Property(e => e.TeamDamagePercentage).HasColumnName("team_damage_percentage");
                entity.Property(e => e.TeamElderDragonKills).HasColumnName("team_elder_dragon_kills");
                entity.Property(e => e.TeamRiftHeraldKills).HasColumnName("team_rift_herald_kills");
                entity.Property(e => e.TookLargeDamageSurvived).HasColumnName("took_large_damage_survived");
                entity.Property(e => e.TurretPlatesTaken).HasColumnName("turret_plates_taken");
                entity.Property(e => e.TurretsTakenWithRiftHerald).HasColumnName("turrets_taken_with_rift_herald");
                entity.Property(e => e.TurretTakedowns).HasColumnName("turret_takedowns");
                entity.Property(e => e.TwentyMinionsIn3SecondsCount).HasColumnName("twenty_minions_in_3_seconds_count");
                entity.Property(e => e.UnseenRecalls).HasColumnName("unseen_recalls");
                entity.Property(e => e.VisionScorePerMinute).HasColumnName("vision_score_per_minute");
                entity.Property(e => e.WardsGuarded).HasColumnName("wards_guarded");
                entity.Property(e => e.WardTakedowns).HasColumnName("ward_takedowns");
                entity.Property(e => e.WardTakedownsBefore20M).HasColumnName("ward_takedowns_before_20m");

                entity.HasOne(e => e.Participant)
                    .WithOne(p => p.Challenges)
                    .HasForeignKey<LoLGameParticipantChallenge>(e => e.LoLGameParticipantId)
                    .HasConstraintName("FK_LoL_Game_Participant_Challenge")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LoLGameParticipantRankChange>(entity =>
            {
                entity.ToTable("LeagueOfLegendsGameParticipantRankChange");

                entity.Property(e => e.LoLGameParticipantId)
                    .ValueGeneratedNever()
                    .HasColumnName("lol_game_participant_id")
                    .IsRequired();

                entity.HasKey(e => e.LoLGameParticipantId);

                entity.Property(e => e.LeaguePointsChange)
                    .HasColumnName("league_points_change");

                entity.Property(e => e.TierBefore)
                    .HasColumnName("tier_before")
                    .HasMaxLength(20);

                entity.Property(e => e.RankBefore)
                    .HasColumnName("rank_before")
                    .HasMaxLength(10);

                entity.Property(e => e.LeaguePointsBefore)
                    .HasColumnName("league_points_before");

                entity.Property(e => e.TierAfter)
                    .HasColumnName("tier_after")
                    .HasMaxLength(20);

                entity.Property(e => e.RankAfter)
                    .HasColumnName("rank_after")
                    .HasMaxLength(10);

                entity.Property(e => e.LeaguePointsAfter)
                    .HasColumnName("league_points_after");

                entity.Property(e => e.ComputedOn)
                    .HasColumnName("computed_on");

                entity.HasOne(e => e.Participant)
                    .WithOne(p => p.RankChange)
                    .HasForeignKey<LoLGameParticipantRankChange>(e => e.LoLGameParticipantId)
                    .HasConstraintName("FK_LoL_Game_Participant_Rank_Change")
                    .OnDelete(DeleteBehavior.Cascade);
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

                entity.Property(e => e.PositionX)
                    .HasColumnName("position_x");

                entity.Property(e => e.PositionY)
                    .HasColumnName("position_y");

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

                entity.Property(e => e.AbilityHaste)
                    .HasColumnName("ability_haste");

                entity.Property(e => e.AbilityPower)
                    .HasColumnName("ability_power");

                entity.Property(e => e.Armor)
                    .HasColumnName("armor");

                entity.Property(e => e.ArmorPen)
                    .HasColumnName("armor_pen");

                entity.Property(e => e.ArmorPenPercent)
                    .HasColumnName("armor_pen_percent");

                entity.Property(e => e.AttackDamage)
                    .HasColumnName("attack_damage");

                entity.Property(e => e.AttackSpeed)
                    .HasColumnName("attack_speed");

                entity.Property(e => e.BonusArmorPenPercent)
                    .HasColumnName("bonus_armor_pen_percent");

                entity.Property(e => e.BonusMagicPenPercent)
                    .HasColumnName("bonus_magic_pen_percent");

                entity.Property(e => e.CcReduction)
                    .HasColumnName("cc_reduction");

                entity.Property(e => e.CooldownReduction)
                    .HasColumnName("cooldown_reduction");

                entity.Property(e => e.Health)
                    .HasColumnName("health");

                entity.Property(e => e.HealthMax)
                    .HasColumnName("health_max");

                entity.Property(e => e.HealthRegen)
                    .HasColumnName("health_regen");

                entity.Property(e => e.Lifesteal)
                    .HasColumnName("lifesteal");

                entity.Property(e => e.MagicPen)
                    .HasColumnName("magic_pen");

                entity.Property(e => e.MagicPenPercent)
                    .HasColumnName("magic_pen_percent");

                entity.Property(e => e.MagicResist)
                    .HasColumnName("magic_resist");

                entity.Property(e => e.MovementSpeed)
                    .HasColumnName("movement_speed");

                entity.Property(e => e.Omnivamp)
                    .HasColumnName("omnivamp");

                entity.Property(e => e.PhysicalVamp)
                    .HasColumnName("physical_vamp");

                entity.Property(e => e.Power)
                    .HasColumnName("power");

                entity.Property(e => e.PowerMax)
                    .HasColumnName("power_max");

                entity.Property(e => e.PowerRegen)
                    .HasColumnName("power_regen");

                entity.Property(e => e.SpellVamp)
                    .HasColumnName("spell_vamp");

                entity.HasOne(e => e.TimelineFrame)
                    .WithMany(f => f.LoLGameTimelineFrameParticipants)
                    .HasForeignKey(e => e.LoLGameTimelineFrameId)
                    .HasConstraintName("FK_LoL_Game_Frame_Participant")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LoLGameTimelineEvent>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .IsRequired();

                entity.HasKey(e => e.Id);

                entity.ToTable("LeagueOfLegendsGameTimelineEvent");

                entity.Property(e => e.LoLGameTimelineFrameId)
                    .HasColumnName("timeline_frame_id");

                entity.Property(e => e.MatchId)
                    .HasColumnName("match_id")
                    .HasMaxLength(100);

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp");

                entity.Property(e => e.RealTimestamp)
                    .HasColumnName("real_timestamp");

                entity.Property(e => e.EventType)
                    .HasColumnName("event_type")
                    .HasMaxLength(100);

                entity.Property(e => e.ParticipantId)
                    .HasColumnName("participant_id");

                entity.Property(e => e.ParticipantPUUID)
                    .HasColumnName("participant_puuid")
                    .HasMaxLength(150);

                entity.Property(e => e.KillerId)
                    .HasColumnName("killer_id");

                entity.Property(e => e.KillerPUUID)
                    .HasColumnName("killer_puuid")
                    .HasMaxLength(150);

                entity.Property(e => e.VictimId)
                    .HasColumnName("victim_id");

                entity.Property(e => e.VictimPUUID)
                    .HasColumnName("victim_puuid")
                    .HasMaxLength(150);

                entity.Property(e => e.KillerTeamId)
                    .HasColumnName("killer_team_id");

                entity.Property(e => e.TeamId)
                    .HasColumnName("team_id");

                entity.Property(e => e.Bounty)
                    .HasColumnName("bounty");

                entity.Property(e => e.ShutdownBounty)
                    .HasColumnName("shutdown_bounty");

                entity.Property(e => e.KillStreakLength)
                    .HasColumnName("kill_streak_length");

                entity.Property(e => e.MultiKillLength)
                    .HasColumnName("multi_kill_length");

                entity.Property(e => e.KillType)
                    .HasColumnName("kill_type")
                    .HasMaxLength(100);

                entity.Property(e => e.ItemId)
                    .HasColumnName("item_id");

                entity.Property(e => e.BeforeId)
                    .HasColumnName("before_id");

                entity.Property(e => e.AfterId)
                    .HasColumnName("after_id");

                entity.Property(e => e.GoldGain)
                    .HasColumnName("gold_gain");

                entity.Property(e => e.SkillSlot)
                    .HasColumnName("skill_slot");

                entity.Property(e => e.LevelUpType)
                    .HasColumnName("level_up_type")
                    .HasMaxLength(100);

                entity.Property(e => e.Level)
                    .HasColumnName("level");

                entity.Property(e => e.WardType)
                    .HasColumnName("ward_type")
                    .HasMaxLength(100);

                entity.Property(e => e.CreatorId)
                    .HasColumnName("creator_id");

                entity.Property(e => e.CreatorPUUID)
                    .HasColumnName("creator_puuid")
                    .HasMaxLength(150);

                entity.Property(e => e.BuildingType)
                    .HasColumnName("building_type")
                    .HasMaxLength(100);

                entity.Property(e => e.TowerType)
                    .HasColumnName("tower_type")
                    .HasMaxLength(100);

                entity.Property(e => e.LaneType)
                    .HasColumnName("lane_type")
                    .HasMaxLength(100);

                entity.Property(e => e.MonsterType)
                    .HasColumnName("monster_type")
                    .HasMaxLength(100);

                entity.Property(e => e.MonsterSubType)
                    .HasColumnName("monster_sub_type")
                    .HasMaxLength(100);

                entity.Property(e => e.TransformType)
                    .HasColumnName("transform_type")
                    .HasMaxLength(100);

                entity.Property(e => e.DragonSoulType)
                    .HasColumnName("dragon_soul_type")
                    .HasMaxLength(100);

                entity.Property(e => e.PositionX)
                    .HasColumnName("position_x");

                entity.Property(e => e.PositionY)
                    .HasColumnName("position_y");

                entity.HasOne(e => e.TimelineFrame)
                    .WithMany(f => f.LoLGameTimelineEvents)
                    .HasForeignKey(e => e.LoLGameTimelineFrameId)
                    .HasConstraintName("FK_LoL_Game_Frame_Event")
                    .OnDelete(DeleteBehavior.Cascade);

                // Restrict (not Cascade): LoLGame already cascades to LoLGameTimelineEvent via
                // LoLGameTimelineFrame above, SQL Server refuses a second cascade path through
                // LoLGameParticipant. The handler deletes/commits old frames+events before touching
                // participants, so Restrict never blocks the normal sync flow.
                entity.HasOne(e => e.Participant)
                    .WithMany()
                    .HasForeignKey(e => new { e.MatchId, e.ParticipantPUUID })
                    .HasPrincipalKey(p => new { p.MatchId, p.Puuid })
                    .HasConstraintName("FK_LoL_Event_Participant")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Killer)
                    .WithMany()
                    .HasForeignKey(e => new { e.MatchId, e.KillerPUUID })
                    .HasPrincipalKey(p => new { p.MatchId, p.Puuid })
                    .HasConstraintName("FK_LoL_Event_Killer")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Victim)
                    .WithMany()
                    .HasForeignKey(e => new { e.MatchId, e.VictimPUUID })
                    .HasPrincipalKey(p => new { p.MatchId, p.Puuid })
                    .HasConstraintName("FK_LoL_Event_Victim")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Creator)
                    .WithMany()
                    .HasForeignKey(e => new { e.MatchId, e.CreatorPUUID })
                    .HasPrincipalKey(p => new { p.MatchId, p.Puuid })
                    .HasConstraintName("FK_LoL_Event_Creator")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<LoLGameTimelineEventAssist>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .IsRequired();

                entity.HasKey(e => e.Id);

                entity.ToTable("LeagueOfLegendsGameTimelineEventAssist");

                entity.Property(e => e.LoLGameTimelineEventId)
                    .HasColumnName("timeline_event_id");

                entity.Property(e => e.MatchId)
                    .HasColumnName("match_id")
                    .HasMaxLength(100);

                entity.Property(e => e.ParticipantId)
                    .HasColumnName("participant_id");

                entity.Property(e => e.ParticipantPUUID)
                    .HasColumnName("participant_puuid")
                    .HasMaxLength(150);

                entity.HasOne(e => e.Event)
                    .WithMany(f => f.LoLGameTimelineEventAssists)
                    .HasForeignKey(e => e.LoLGameTimelineEventId)
                    .HasConstraintName("FK_LoL_Game_Event_Assist")
                    .OnDelete(DeleteBehavior.Cascade);

                // Restrict for the same multiple-cascade-paths reason as LoLGameTimelineEvent's participant links.
                entity.HasOne(e => e.Participant)
                    .WithMany()
                    .HasForeignKey(e => new { e.MatchId, e.ParticipantPUUID })
                    .HasPrincipalKey(p => new { p.MatchId, p.Puuid })
                    .HasConstraintName("FK_LoL_EventAssist_Participant")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ValGame>(entity =>
            {
                entity.ToTable("ValorantGame");

                entity.Property(e => e.MatchId)
                    .HasColumnName("match_id")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasKey(e => e.MatchId);

                entity.Property(e => e.MapId)
                    .HasColumnName("map_id")
                    .HasMaxLength(100);

                entity.Property(e => e.MapName)
                    .HasColumnName("map_name")
                    .HasMaxLength(100);

                entity.Property(e => e.GameVersion)
                    .HasColumnName("game_version")
                    .HasMaxLength(100);

                entity.Property(e => e.Region)
                    .HasColumnName("region")
                    .HasMaxLength(20);

                entity.Property(e => e.Cluster)
                    .HasColumnName("cluster")
                    .HasMaxLength(50);

                entity.Property(e => e.Platform)
                    .HasColumnName("platform")
                    .HasMaxLength(20);

                entity.Property(e => e.QueueId)
                    .HasColumnName("queue_id")
                    .HasMaxLength(50);

                entity.Property(e => e.QueueName)
                    .HasColumnName("queue_name")
                    .HasMaxLength(100);

                entity.Property(e => e.QueueModeType)
                    .HasColumnName("queue_mode_type")
                    .HasMaxLength(50);

                entity.Property(e => e.SeasonId)
                    .HasColumnName("season_id")
                    .HasMaxLength(100);

                entity.Property(e => e.SeasonShort)
                    .HasColumnName("season_short")
                    .HasMaxLength(20);

                entity.Property(e => e.RetrievedOn)
                    .HasColumnName("retrieved_on")
                    .HasDefaultValue(new DateTime(1999, 5, 21, 00, 00, 00));

                entity.Property(e => e.GameStart)
                    .HasColumnName("game_start")
                    .HasDefaultValue(new DateTime(1999, 5, 21, 00, 00, 00));

                entity.Property(e => e.GameLengthMs)
                    .HasColumnName("game_length_ms");

                entity.Property(e => e.GameEnd)
                    .HasColumnName("game_end")
                    .HasDefaultValue(new DateTime(1999, 5, 21, 00, 00, 00));

                entity.Property(e => e.IsCompleted)
                    .HasColumnName("is_completed")
                    .HasDefaultValue(true);

                entity.Property(e => e.IsRemake)
                    .HasColumnName("is_remake")
                    .HasDefaultValue(false);

                entity.Property(e => e.WinningTeamId)
                    .HasColumnName("winning_team_id")
                    .HasMaxLength(100);

                entity.Property(e => e.MvpParticipantId)
                    .HasColumnName("mvp_participant_id");

                entity.Property(e => e.AceParticipantId)
                    .HasColumnName("ace_participant_id");

                entity.HasIndex(e => e.GameStart);
                entity.HasIndex(e => e.QueueId);

                entity.HasMany(e => e.ValorantGameParticipants)
                    .WithOne(f => f.Game)
                    .HasForeignKey(f => f.MatchId)
                    .HasConstraintName("FK_Val_Games_Participants")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.ValorantGameTeams)
                    .WithOne(f => f.Game)
                    .HasForeignKey(f => f.MatchId)
                    .HasConstraintName("FK_Val_Games_Teams")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.ValorantGameRounds)
                    .WithOne(f => f.Game)
                    .HasForeignKey(f => f.MatchId)
                    .HasConstraintName("FK_Val_Games_Rounds")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.ValorantGameKills)
                    .WithOne(f => f.Game)
                    .HasForeignKey(f => f.MatchId)
                    .HasConstraintName("FK_Val_Games_Kills")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.ValorantGameRoundPlayerStats)
                    .WithOne(f => f.Game)
                    .HasForeignKey(f => f.MatchId)
                    .HasConstraintName("FK_Val_Games_Round_Player_Stats")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<ValGameParticipant>()
                    .WithMany()
                    .HasForeignKey(e => e.MvpParticipantId)
                    .HasConstraintName("FK_ValGame_Mvp_Participant")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<ValGameParticipant>()
                    .WithMany()
                    .HasForeignKey(e => e.AceParticipantId)
                    .HasConstraintName("FK_ValGame_Ace_Participant")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ValGameTeam>(entity =>
            {
                entity.ToTable("ValorantGameTeam");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .IsRequired();

                entity.HasKey(e => e.Id);

                entity.Property(e => e.MatchId)
                    .HasColumnName("match_id")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.TeamId)
                    .HasColumnName("team_id")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Won)
                    .HasColumnName("won");

                entity.Property(e => e.RoundsWon)
                    .HasColumnName("rounds_won");

                entity.Property(e => e.RoundsLost)
                    .HasColumnName("rounds_lost");
            });

            modelBuilder.Entity<ValGameParticipant>(entity =>
            {
                entity.ToTable("ValorantGameParticipant");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .IsRequired();

                entity.HasKey(e => e.Id);

                entity.Property(e => e.MatchId)
                    .HasColumnName("match_id")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PlayerId)
                    .HasColumnName("player_id");

                entity.Property(e => e.Puuid)
                    .HasColumnName("puuid")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.RiotIdGameName)
                    .HasColumnName("riot_id_game_name")
                    .HasMaxLength(150);

                entity.Property(e => e.RiotIdTagLine)
                    .HasColumnName("riot_id_tagline")
                    .HasMaxLength(50);

                entity.Property(e => e.TeamId)
                    .HasColumnName("team_id")
                    .HasMaxLength(100);

                entity.Property(e => e.PartyId)
                    .HasColumnName("party_id")
                    .HasMaxLength(100);

                entity.Property(e => e.Platform)
                    .HasColumnName("platform")
                    .HasMaxLength(20);

                entity.Property(e => e.AccountLevel)
                    .HasColumnName("account_level");

                entity.Property(e => e.PlayerCardId)
                    .HasColumnName("player_card_id")
                    .HasMaxLength(100);

                entity.Property(e => e.PlayerTitleId)
                    .HasColumnName("player_title_id")
                    .HasMaxLength(100);

                entity.Property(e => e.AgentId)
                    .HasColumnName("agent_id")
                    .HasMaxLength(100);

                entity.Property(e => e.AgentName)
                    .HasColumnName("agent_name")
                    .HasMaxLength(50);

                entity.Property(e => e.TierId)
                    .HasColumnName("tier_id");

                entity.Property(e => e.TierName)
                    .HasColumnName("tier_name")
                    .HasMaxLength(50);

                entity.Property(e => e.Score)
                    .HasColumnName("score");

                entity.Property(e => e.Kills)
                    .HasColumnName("kills");

                entity.Property(e => e.Deaths)
                    .HasColumnName("deaths");

                entity.Property(e => e.Assists)
                    .HasColumnName("assists");

                entity.Property(e => e.Headshots)
                    .HasColumnName("headshots");

                entity.Property(e => e.Bodyshots)
                    .HasColumnName("bodyshots");

                entity.Property(e => e.Legshots)
                    .HasColumnName("legshots");

                entity.Property(e => e.DamageDealt)
                    .HasColumnName("damage_dealt");

                entity.Property(e => e.DamageReceived)
                    .HasColumnName("damage_received");

                entity.Property(e => e.GrenadeCasts)
                    .HasColumnName("grenade_casts");

                entity.Property(e => e.Ability1Casts)
                    .HasColumnName("ability1_casts");

                entity.Property(e => e.Ability2Casts)
                    .HasColumnName("ability2_casts");

                entity.Property(e => e.UltimateCasts)
                    .HasColumnName("ultimate_casts");

                entity.Property(e => e.SpentOverall)
                    .HasColumnName("spent_overall");

                entity.Property(e => e.SpentAverage)
                    .HasColumnName("spent_average");

                entity.Property(e => e.LoadoutValueOverall)
                    .HasColumnName("loadout_value_overall");

                entity.Property(e => e.LoadoutValueAverage)
                    .HasColumnName("loadout_value_average");

                entity.Property(e => e.AfkRounds)
                    .HasColumnName("afk_rounds");

                entity.Property(e => e.RoundsInSpawn)
                    .HasColumnName("rounds_in_spawn");

                entity.Property(e => e.FriendlyFireIncoming)
                    .HasColumnName("friendly_fire_incoming");

                entity.Property(e => e.FriendlyFireOutgoing)
                    .HasColumnName("friendly_fire_outgoing");

                entity.Property(e => e.SessionPlaytimeMs)
                    .HasColumnName("session_playtime_ms");

                entity.Property(e => e.Win)
                    .HasColumnName("win");

                entity.Property(e => e.RrChange)
                    .HasColumnName("rr_change");

                entity.Property(e => e.PostGameTierId)
                    .HasColumnName("post_game_tier_id");

                entity.Property(e => e.PostGameTierName)
                    .HasColumnName("post_game_tier_name")
                    .HasMaxLength(50);

                entity.Property(e => e.PostGameRankedRating)
                    .HasColumnName("post_game_ranked_rating");

                entity.HasAlternateKey(e => new { e.MatchId, e.Puuid })
                    .HasName("AK_ValGameParticipant_MatchId_Puuid");

                entity.HasIndex(e => e.PlayerId);
                entity.HasIndex(e => e.Puuid);

                entity.HasOne(e => e.Player)
                    .WithMany(f => f.ValorantGameParticipants)
                    .HasForeignKey(e => e.PlayerId)
                    .HasConstraintName("FK_Player_Val_Game_Participant")
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<ValGameParticipantStat>(entity =>
            {
                entity.ToTable("ValorantGameParticipantStat");

                entity.Property(e => e.ValGameParticipantId)
                    .ValueGeneratedNever()
                    .HasColumnName("val_game_participant_id")
                    .IsRequired();

                entity.HasKey(e => e.ValGameParticipantId);

                entity.Property(e => e.RoundsPlayed)
                    .HasColumnName("rounds_played");

                entity.Property(e => e.Acs)
                    .HasColumnName("acs");

                entity.Property(e => e.Kda)
                    .HasColumnName("kda");

                entity.Property(e => e.KdRatio)
                    .HasColumnName("kd_ratio");

                entity.Property(e => e.KillParticipationPercent)
                    .HasColumnName("kill_participation_percent");

                entity.Property(e => e.Adr)
                    .HasColumnName("adr");

                entity.Property(e => e.DamageDeltaPerRound)
                    .HasColumnName("damage_delta_per_round");

                entity.Property(e => e.HeadshotPercent)
                    .HasColumnName("headshot_percent");

                entity.Property(e => e.FirstKills)
                    .HasColumnName("first_kills");

                entity.Property(e => e.FirstDeaths)
                    .HasColumnName("first_deaths");

                entity.Property(e => e.KastPercent)
                    .HasColumnName("kast_percent");

                entity.Property(e => e.DoubleKills)
                    .HasColumnName("double_kills");

                entity.Property(e => e.TripleKills)
                    .HasColumnName("triple_kills");

                entity.Property(e => e.QuadraKills)
                    .HasColumnName("quadra_kills");

                entity.Property(e => e.Aces)
                    .HasColumnName("aces");

                entity.Property(e => e.ClutchesWon)
                    .HasColumnName("clutches_won");

                entity.Property(e => e.SpikePlants)
                    .HasColumnName("spike_plants");

                entity.Property(e => e.SpikeDefuses)
                    .HasColumnName("spike_defuses");

                entity.Property(e => e.KnifeKills)
                    .HasColumnName("knife_kills");

                entity.Property(e => e.EconRating)
                    .HasColumnName("econ_rating");

                entity.Property(e => e.Rating)
                    .HasColumnName("rating");

                entity.Property(e => e.ComputedOn)
                    .HasColumnName("computed_on");

                entity.HasOne(e => e.Participant)
                    .WithOne(p => p.Stats)
                    .HasForeignKey<ValGameParticipantStat>(e => e.ValGameParticipantId)
                    .HasConstraintName("FK_Val_Game_Participant_Stat")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ValGameRound>(entity =>
            {
                entity.ToTable("ValorantGameRound");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .IsRequired();

                entity.HasKey(e => e.Id);

                entity.Property(e => e.MatchId)
                    .HasColumnName("match_id")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.RoundNumber)
                    .HasColumnName("round_number");

                entity.Property(e => e.Result)
                    .HasColumnName("result")
                    .HasMaxLength(100);

                entity.Property(e => e.Ceremony)
                    .HasColumnName("ceremony")
                    .HasMaxLength(100);

                entity.Property(e => e.WinningTeam)
                    .HasColumnName("winning_team")
                    .HasMaxLength(100);

                entity.Property(e => e.WinningTeamRole)
                    .HasColumnName("winning_team_role")
                    .HasMaxLength(50);

                entity.Property(e => e.FirstBloodPuuid)
                    .HasColumnName("first_blood_puuid")
                    .HasMaxLength(100);

                entity.Property(e => e.FirstBloodTeam)
                    .HasColumnName("first_blood_team")
                    .HasMaxLength(100);

                entity.Property(e => e.CeremonyPlayerPuuid)
                    .HasColumnName("ceremony_player_puuid")
                    .HasMaxLength(100);

                entity.Property(e => e.CeremonyTeam)
                    .HasColumnName("ceremony_team")
                    .HasMaxLength(100);

                entity.Property(e => e.PlantTimeMs)
                    .HasColumnName("plant_time_ms");

                entity.Property(e => e.PlantSite)
                    .HasColumnName("plant_site")
                    .HasMaxLength(10);

                entity.Property(e => e.PlantPlayerPuuid)
                    .HasColumnName("plant_player_puuid")
                    .HasMaxLength(100);

                entity.Property(e => e.DefuseTimeMs)
                    .HasColumnName("defuse_time_ms");

                entity.Property(e => e.DefusePlayerPuuid)
                    .HasColumnName("defuse_player_puuid")
                    .HasMaxLength(100);

                entity.HasIndex(e => new { e.MatchId, e.RoundNumber });
            });

            modelBuilder.Entity<ValGameKill>(entity =>
            {
                entity.ToTable("ValorantGameKill");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .IsRequired();

                entity.HasKey(e => e.Id);

                entity.Property(e => e.MatchId)
                    .HasColumnName("match_id")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.RoundNumber)
                    .HasColumnName("round_number");

                entity.Property(e => e.TimeInRoundMs)
                    .HasColumnName("time_in_round_ms");

                entity.Property(e => e.TimeInMatchMs)
                    .HasColumnName("time_in_match_ms");

                entity.Property(e => e.KillerPuuid)
                    .HasColumnName("killer_puuid")
                    .HasMaxLength(100);

                entity.Property(e => e.KillerTeam)
                    .HasColumnName("killer_team")
                    .HasMaxLength(100);

                entity.Property(e => e.VictimPuuid)
                    .HasColumnName("victim_puuid")
                    .HasMaxLength(100);

                entity.Property(e => e.VictimTeam)
                    .HasColumnName("victim_team")
                    .HasMaxLength(100);

                entity.Property(e => e.AssistantsCount)
                    .HasColumnName("assistants_count");

                entity.Property(e => e.AssistantPuuids)
                    .HasColumnName("assistant_puuids")
                    .HasMaxLength(500);

                entity.Property(e => e.WeaponId)
                    .HasColumnName("weapon_id")
                    .HasMaxLength(100);

                entity.Property(e => e.WeaponName)
                    .HasColumnName("weapon_name")
                    .HasMaxLength(100);

                entity.Property(e => e.WeaponType)
                    .HasColumnName("weapon_type")
                    .HasMaxLength(50);

                entity.Property(e => e.SecondaryFireMode)
                    .HasColumnName("secondary_fire_mode");

                entity.HasIndex(e => new { e.MatchId, e.RoundNumber });
            });

            modelBuilder.Entity<ValGameRoundPlayerStat>(entity =>
            {
                entity.ToTable("ValorantGameRoundPlayerStat");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .IsRequired();

                entity.HasKey(e => e.Id);

                entity.Property(e => e.MatchId)
                    .HasColumnName("match_id")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.RoundNumber)
                    .HasColumnName("round_number");

                entity.Property(e => e.Puuid)
                    .HasColumnName("puuid")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Team)
                    .HasColumnName("team")
                    .HasMaxLength(100);

                entity.Property(e => e.LoadoutValue)
                    .HasColumnName("loadout_value");

                entity.Property(e => e.Remaining)
                    .HasColumnName("remaining");

                entity.Property(e => e.WeaponId)
                    .HasColumnName("weapon_id")
                    .HasMaxLength(100);

                entity.Property(e => e.WeaponName)
                    .HasColumnName("weapon_name")
                    .HasMaxLength(100);

                entity.Property(e => e.ArmorId)
                    .HasColumnName("armor_id")
                    .HasMaxLength(100);

                entity.Property(e => e.ArmorName)
                    .HasColumnName("armor_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Score)
                    .HasColumnName("score");

                entity.Property(e => e.Kills)
                    .HasColumnName("kills");

                entity.Property(e => e.Headshots)
                    .HasColumnName("headshots");

                entity.Property(e => e.Bodyshots)
                    .HasColumnName("bodyshots");

                entity.Property(e => e.Legshots)
                    .HasColumnName("legshots");

                entity.Property(e => e.Damage)
                    .HasColumnName("damage");

                entity.Property(e => e.WasAfk)
                    .HasColumnName("was_afk");

                entity.HasIndex(e => new { e.MatchId, e.RoundNumber });
            });

            ConfigureUtcDates(modelBuilder);
        }

        /// <summary>
        /// Forces every date of the model through a UTC round-trip.
        /// <para>
        /// SQL Server datetime2 columns carry no offset, so EF materializes every date with an
        /// <see cref="DateTimeKind.Unspecified"/> kind, and System.Text.Json then serializes it without the
        /// trailing "Z" — leaving clients to guess the zone, and getting it wrong by their own UTC offset.
        /// Everything is written in UTC, so the kind is restored on read and enforced on write.
        /// </para>
        /// </summary>
        /// <param name="modelBuilder">Model builder.</param>
        private static void ConfigureUtcDates(ModelBuilder modelBuilder)
        {
            var toUtc = new ValueConverter<DateTime, DateTime>(
                v => v.Kind == DateTimeKind.Local ? v.ToUniversalTime() : DateTime.SpecifyKind(v, DateTimeKind.Utc),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            var nullableToUtc = new ValueConverter<DateTime?, DateTime?>(
                v => v.HasValue
                    ? (v.Value.Kind == DateTimeKind.Local ? v.Value.ToUniversalTime() : DateTime.SpecifyKind(v.Value, DateTimeKind.Utc))
                    : v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(toUtc);
                    }
                    else if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(nullableToUtc);
                    }
                }
            }
        }
    }
}