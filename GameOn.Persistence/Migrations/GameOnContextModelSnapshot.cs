﻿// <auto-generated />
using System;
using GameOn.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GameOn.Persistence.Migrations
{
    [DbContext(typeof(GameOnContext))]
    partial class GameOnContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("GameOn.Domain.Changelog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Context")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)")
                        .HasColumnName("context");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("name");

                    b.Property<string>("NewFeatures")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)")
                        .HasColumnName("new_features");

                    b.Property<DateTime>("PublicationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValue(new DateTime(2024, 12, 18, 23, 6, 23, 154, DateTimeKind.Local).AddTicks(9402))
                        .HasColumnName("publication_date");

                    b.Property<string>("RemovedFeatures")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)")
                        .HasColumnName("removed_features");

                    b.Property<int>("Type")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0)
                        .HasColumnName("type");

                    b.Property<string>("UpdatedFeatures")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)")
                        .HasColumnName("updated_features");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("version");

                    b.HasKey("Id");

                    b.ToTable("Changelog", (string)null);
                });

            modelBuilder.Entity("GameOn.Domain.FifaGamePlayed", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CreatedById")
                        .HasColumnType("int")
                        .HasColumnName("created_by_id");

                    b.Property<bool>("IsPlayed")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_played");

                    b.Property<int?>("Phase")
                        .HasColumnType("int")
                        .HasColumnName("phase");

                    b.Property<int>("PlatformId")
                        .HasColumnType("int")
                        .HasColumnName("platform_id");

                    b.Property<DateTime>("PlayedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValue(new DateTime(2024, 12, 18, 23, 6, 23, 150, DateTimeKind.Local).AddTicks(6739))
                        .HasColumnName("played_on");

                    b.Property<int>("SeasonId")
                        .HasColumnType("int")
                        .HasColumnName("season_id");

                    b.Property<int>("Team1Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1)
                        .HasColumnName("team_1_id");

                    b.Property<int>("Team2Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1)
                        .HasColumnName("team_2_id");

                    b.Property<int>("TeamScore1")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100)
                        .HasColumnType("int")
                        .HasDefaultValue(0)
                        .HasColumnName("team_score_1");

                    b.Property<int>("TeamScore2")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100)
                        .HasColumnType("int")
                        .HasDefaultValue(0)
                        .HasColumnName("team_score_2");

                    b.Property<int?>("TournamentId")
                        .HasColumnType("int")
                        .HasColumnName("tournament_id");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("PlatformId");

                    b.HasIndex("SeasonId");

                    b.HasIndex("Team1Id");

                    b.HasIndex("Team2Id");

                    b.HasIndex("TournamentId");

                    b.ToTable("FifaGamePlayed", (string)null);
                });

            modelBuilder.Entity("GameOn.Domain.FifaTeam", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("name");

                    b.Property<int>("Type")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0)
                        .HasColumnName("type");

                    b.HasKey("Id");

                    b.ToTable("FifaTeam", (string)null);
                });

            modelBuilder.Entity("GameOn.Domain.FifaTeamPlayer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FifaGameId")
                        .HasColumnType("int")
                        .HasColumnName("fifa_game_id");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int")
                        .HasColumnName("player_id");

                    b.Property<int>("Team")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0)
                        .HasColumnName("team");

                    b.HasKey("Id");

                    b.HasIndex("FifaGameId");

                    b.HasIndex("PlayerId");

                    b.ToTable("FifaTeamPlayer", (string)null);
                });

            modelBuilder.Entity("GameOn.Domain.Highlight", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CreatedById")
                        .HasColumnType("int")
                        .HasColumnName("created_by_id");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)")
                        .HasColumnName("description");

                    b.Property<string>("ExternalUrl")
                        .HasMaxLength(3000)
                        .HasColumnType("varchar(3000)")
                        .HasColumnName("external_url");

                    b.Property<int>("FifaGameId")
                        .HasColumnType("int")
                        .HasColumnName("fifa_game_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("FifaGameId");

                    b.ToTable("Highlight", (string)null);
                });

            modelBuilder.Entity("GameOn.Domain.Platform", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("Platform", (string)null);
                });

            modelBuilder.Entity("GameOn.Domain.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Archived")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false)
                        .HasColumnName("archived");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValue(new DateTime(2024, 12, 18, 22, 6, 23, 149, DateTimeKind.Utc).AddTicks(7160))
                        .HasColumnName("created_on");

                    b.Property<string>("FullName")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("full_name");

                    b.Property<string>("KeycloakId")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("keycloak_id");

                    b.Property<string>("LolSummonerId")
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)")
                        .HasColumnName("lol_summoner_id");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("nickname");

                    b.Property<string>("ProfilePictureUrl")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)")
                        .HasColumnName("profile_picture_url");

                    b.Property<string>("RiotGamesNickname")
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)")
                        .HasColumnName("riot_games_nickname");

                    b.Property<string>("RiotGamesPUUID")
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)")
                        .HasColumnName("riot_games_puuid");

                    b.Property<string>("RiotGamesTagLine")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("riot_games_tag_line");

                    b.HasKey("Id");

                    b.ToTable("Player", (string)null);
                });

            modelBuilder.Entity("GameOn.Domain.Season", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("Season", (string)null);
                });

            modelBuilder.Entity("GameOn.Domain.SoccerFive", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CreatedById")
                        .HasColumnType("int")
                        .HasColumnName("created_by_id");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("name");

                    b.Property<DateTime?>("PlannedOn")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("planned_on");

                    b.Property<int>("State")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0)
                        .HasColumnName("state");

                    b.Property<string>("VoteQuestion")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)")
                        .HasColumnName("vote_question");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("SoccerFive", (string)null);
                });

            modelBuilder.Entity("GameOn.Domain.SoccerFiveVoteAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("PlayerId")
                        .HasColumnType("int")
                        .HasColumnName("player_id");

                    b.Property<int>("VoteChoiceId")
                        .HasColumnType("int")
                        .HasColumnName("vote_choice_id");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("VoteChoiceId");

                    b.ToTable("SoccerFiveVoteAnswer", (string)null);
                });

            modelBuilder.Entity("GameOn.Domain.SoccerFiveVoteChoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("label");

                    b.Property<int>("Order")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1)
                        .HasColumnName("order");

                    b.Property<int>("SoccerFiveId")
                        .HasColumnType("int")
                        .HasColumnName("soccer_five_id");

                    b.HasKey("Id");

                    b.HasIndex("SoccerFiveId");

                    b.ToTable("SoccerFiveVoteChoice", (string)null);
                });

            modelBuilder.Entity("GameOn.Domain.Tournament", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(5000)
                        .HasColumnType("varchar(5000)")
                        .HasColumnName("description");

                    b.Property<int>("DrawPoints")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1)
                        .HasColumnName("draw_points");

                    b.Property<bool>("Featured")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false)
                        .HasColumnName("featured");

                    b.Property<string>("LogoUrl")
                        .HasMaxLength(3000)
                        .HasColumnType("varchar(3000)")
                        .HasColumnName("logo_url");

                    b.Property<int>("LoosePoints")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0)
                        .HasColumnName("loose_points");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("name");

                    b.Property<string>("Phase2ChallongeUrl")
                        .HasMaxLength(3000)
                        .HasColumnType("varchar(3000)")
                        .HasColumnName("phase2_challonge_url");

                    b.Property<DateTime>("PlannedFrom")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValue(new DateTime(2024, 12, 18, 22, 6, 23, 149, DateTimeKind.Utc).AddTicks(8849))
                        .HasColumnName("planned_from");

                    b.Property<DateTime>("PlannedTo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValue(new DateTime(2024, 12, 19, 22, 6, 23, 149, DateTimeKind.Utc).AddTicks(9123))
                        .HasColumnName("planned_to");

                    b.Property<string>("Rules")
                        .HasMaxLength(5000)
                        .HasColumnType("varchar(5000)")
                        .HasColumnName("rules");

                    b.Property<int>("State")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0)
                        .HasColumnName("state");

                    b.Property<int>("WinPoints")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(3)
                        .HasColumnName("win_points");

                    b.Property<int?>("WinnerId")
                        .HasColumnType("int")
                        .HasColumnName("winner_id");

                    b.HasKey("Id");

                    b.HasIndex("WinnerId");

                    b.ToTable("Tournament", (string)null);
                });

            modelBuilder.Entity("GameOn.Domain.TournamentPlayer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FifaTeamId")
                        .HasColumnType("int")
                        .HasColumnName("fifa_team_id");

                    b.Property<DateTime>("JoinedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValue(new DateTime(2024, 12, 18, 22, 6, 23, 150, DateTimeKind.Utc).AddTicks(2156))
                        .HasColumnName("joined_at");

                    b.Property<int?>("Phase1Score")
                        .HasColumnType("int")
                        .HasColumnName("phase_1_score");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int")
                        .HasColumnName("player_id");

                    b.Property<int>("TournamentId")
                        .HasColumnType("int")
                        .HasColumnName("tournament_id");

                    b.HasKey("Id");

                    b.HasIndex("FifaTeamId");

                    b.HasIndex("PlayerId");

                    b.HasIndex("TournamentId");

                    b.ToTable("TournamentPlayer", (string)null);
                });

            modelBuilder.Entity("GameOn.Domain.FifaGamePlayed", b =>
                {
                    b.HasOne("GameOn.Domain.Player", "CreatedBy")
                        .WithMany("FifaGameCreated")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_FifaGamePlayed_Player_Created_By");

                    b.HasOne("GameOn.Domain.Platform", "Platform")
                        .WithMany("GamesPlayed")
                        .HasForeignKey("PlatformId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_FifaGamePlayed_Platform");

                    b.HasOne("GameOn.Domain.Season", "Season")
                        .WithMany("FifaGamePlayed")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_FifaGamePlayed_Season");

                    b.HasOne("GameOn.Domain.FifaTeam", "Team1")
                        .WithMany("GamesPlayedTeam1")
                        .HasForeignKey("Team1Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_FifaGamePlayed_FifaTeam1");

                    b.HasOne("GameOn.Domain.FifaTeam", "Team2")
                        .WithMany("GamesPlayedTeam2")
                        .HasForeignKey("Team2Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_FifaGamePlayed_FifaTeam2");

                    b.HasOne("GameOn.Domain.Tournament", "Tournament")
                        .WithMany("Games")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_FifaGamePlayed_Tournament");

                    b.Navigation("CreatedBy");

                    b.Navigation("Platform");

                    b.Navigation("Season");

                    b.Navigation("Team1");

                    b.Navigation("Team2");

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("GameOn.Domain.FifaTeamPlayer", b =>
                {
                    b.HasOne("GameOn.Domain.FifaGamePlayed", "FifaGamePlayed")
                        .WithMany("TeamPlayers")
                        .HasForeignKey("FifaGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_FifaTeamPlayer_FifaGamePlayed");

                    b.HasOne("GameOn.Domain.Player", "Player")
                        .WithMany("FifaTeamPlayers")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_FifaTeamPlayer_Player");

                    b.Navigation("FifaGamePlayed");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("GameOn.Domain.Highlight", b =>
                {
                    b.HasOne("GameOn.Domain.Player", "CreatedBy")
                        .WithMany("Highlights")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Highlight_Player_Created_By");

                    b.HasOne("GameOn.Domain.FifaGamePlayed", "FifaGame")
                        .WithMany("Highlights")
                        .HasForeignKey("FifaGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_FifaGame_Highlight");

                    b.Navigation("CreatedBy");

                    b.Navigation("FifaGame");
                });

            modelBuilder.Entity("GameOn.Domain.SoccerFive", b =>
                {
                    b.HasOne("GameOn.Domain.Player", "CreatedBy")
                        .WithMany("SoccerFivesCreated")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_SoccerFive_Player_Created_By");

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("GameOn.Domain.SoccerFiveVoteAnswer", b =>
                {
                    b.HasOne("GameOn.Domain.Player", "Player")
                        .WithMany("SoccerFiveVoteAnswers")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Player_VoteAnswer");

                    b.HasOne("GameOn.Domain.SoccerFiveVoteChoice", "VoteChoice")
                        .WithMany("Answers")
                        .HasForeignKey("VoteChoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_VoteChoice_VoteAnswer");

                    b.Navigation("Player");

                    b.Navigation("VoteChoice");
                });

            modelBuilder.Entity("GameOn.Domain.SoccerFiveVoteChoice", b =>
                {
                    b.HasOne("GameOn.Domain.SoccerFive", "SoccerFive")
                        .WithMany("VotesChoices")
                        .HasForeignKey("SoccerFiveId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_SoccerFive_VoteChoice");

                    b.Navigation("SoccerFive");
                });

            modelBuilder.Entity("GameOn.Domain.Tournament", b =>
                {
                    b.HasOne("GameOn.Domain.Player", "Winner")
                        .WithMany("TournamentsWon")
                        .HasForeignKey("WinnerId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_Tournament_Player_Winner");

                    b.Navigation("Winner");
                });

            modelBuilder.Entity("GameOn.Domain.TournamentPlayer", b =>
                {
                    b.HasOne("GameOn.Domain.FifaTeam", "FifaTeam")
                        .WithMany("TournamentPlayers")
                        .HasForeignKey("FifaTeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_TournamentPlayer_FifaTeam");

                    b.HasOne("GameOn.Domain.Player", "Player")
                        .WithMany("TournamentPlayed")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_TournamentPlayer_Player");

                    b.HasOne("GameOn.Domain.Tournament", "Tournament")
                        .WithMany("Players")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_TournamentPlayer_Tournament");

                    b.Navigation("FifaTeam");

                    b.Navigation("Player");

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("GameOn.Domain.FifaGamePlayed", b =>
                {
                    b.Navigation("Highlights");

                    b.Navigation("TeamPlayers");
                });

            modelBuilder.Entity("GameOn.Domain.FifaTeam", b =>
                {
                    b.Navigation("GamesPlayedTeam1");

                    b.Navigation("GamesPlayedTeam2");

                    b.Navigation("TournamentPlayers");
                });

            modelBuilder.Entity("GameOn.Domain.Platform", b =>
                {
                    b.Navigation("GamesPlayed");
                });

            modelBuilder.Entity("GameOn.Domain.Player", b =>
                {
                    b.Navigation("FifaGameCreated");

                    b.Navigation("FifaTeamPlayers");

                    b.Navigation("Highlights");

                    b.Navigation("SoccerFiveVoteAnswers");

                    b.Navigation("SoccerFivesCreated");

                    b.Navigation("TournamentPlayed");

                    b.Navigation("TournamentsWon");
                });

            modelBuilder.Entity("GameOn.Domain.Season", b =>
                {
                    b.Navigation("FifaGamePlayed");
                });

            modelBuilder.Entity("GameOn.Domain.SoccerFive", b =>
                {
                    b.Navigation("VotesChoices");
                });

            modelBuilder.Entity("GameOn.Domain.SoccerFiveVoteChoice", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("GameOn.Domain.Tournament", b =>
                {
                    b.Navigation("Games");

                    b.Navigation("Players");
                });
#pragma warning restore 612, 618
        }
    }
}
