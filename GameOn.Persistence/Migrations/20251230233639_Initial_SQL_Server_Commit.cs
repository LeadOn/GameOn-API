using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOn.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial_SQL_Server_Commit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Changelog",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    publication_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(1999, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    type = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    published = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    version = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    context = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    new_features = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    updated_features = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    removed_features = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Changelog", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "FifaTeam",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    type = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FifaTeam", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "LeagueOfLegendsGame",
                columns: table => new
                {
                    match_id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    game_id = table.Column<long>(type: "bigint", nullable: true),
                    end_of_game_result = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    game_version = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    retrieved_on = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(1999, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    game_start = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(1999, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    game_end = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(1999, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    winning_team_id = table.Column<int>(type: "int", nullable: true),
                    queue_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueOfLegendsGame", x => x.match_id);
                });

            migrationBuilder.CreateTable(
                name: "Platform",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platform", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    keycloak_id = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    full_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    nickname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    profile_picture_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    riot_games_nickname = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    riot_games_tag_line = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    riot_games_puuid = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    lol_summoner_id = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    LolSummonerLevel = table.Column<long>(type: "bigint", nullable: true),
                    lol_icon_id = table.Column<int>(type: "int", nullable: true),
                    lol_refreshed_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(1999, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    archived = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Season",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Season", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "LeagueOfLegendsGameTimelineFrame",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    match_id = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    timestamp = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueOfLegendsGameTimelineFrame", x => x.id);
                    table.ForeignKey(
                        name: "FK_LoL_Game_Frame",
                        column: x => x.match_id,
                        principalTable: "LeagueOfLegendsGame",
                        principalColumn: "match_id");
                });

            migrationBuilder.CreateTable(
                name: "LeagueOfLegendsGameParticipant",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    player_id = table.Column<int>(type: "int", nullable: true),
                    MatchId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    AllInPings = table.Column<int>(type: "int", nullable: false),
                    AssistMePings = table.Column<int>(type: "int", nullable: false),
                    assists = table.Column<int>(type: "int", nullable: false),
                    BaronKills = table.Column<int>(type: "int", nullable: false),
                    BountyLevel = table.Column<int>(type: "int", nullable: false),
                    ChampExperience = table.Column<int>(type: "int", nullable: false),
                    champLevel = table.Column<int>(type: "int", nullable: false),
                    champion_id = table.Column<int>(type: "int", nullable: false),
                    champion_name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CommandPings = table.Column<int>(type: "int", nullable: false),
                    ChampionTransform = table.Column<int>(type: "int", nullable: false),
                    ConsumablesPurchased = table.Column<int>(type: "int", nullable: false),
                    puuid = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    riot_id_game_name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    riot_id_tagline = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    team_id = table.Column<int>(type: "int", nullable: false),
                    kills = table.Column<int>(type: "int", nullable: false),
                    deaths = table.Column<int>(type: "int", nullable: false),
                    win = table.Column<bool>(type: "bit", nullable: false),
                    item0 = table.Column<int>(type: "int", nullable: false),
                    item1 = table.Column<int>(type: "int", nullable: false),
                    item2 = table.Column<int>(type: "int", nullable: false),
                    item3 = table.Column<int>(type: "int", nullable: false),
                    item4 = table.Column<int>(type: "int", nullable: false),
                    item5 = table.Column<int>(type: "int", nullable: false),
                    item6 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueOfLegendsGameParticipant", x => x.id);
                    table.ForeignKey(
                        name: "FK_LoL_Games_Participants",
                        column: x => x.MatchId,
                        principalTable: "LeagueOfLegendsGame",
                        principalColumn: "match_id");
                    table.ForeignKey(
                        name: "FK_Player_LoL_Game_Participant",
                        column: x => x.player_id,
                        principalTable: "Player",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "LeagueOfLegendsRankHistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    queue_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    tier = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    rank = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    league_points = table.Column<int>(type: "int", nullable: false),
                    wins = table.Column<int>(type: "int", nullable: false),
                    losses = table.Column<int>(type: "int", nullable: false),
                    hot_streak = table.Column<bool>(type: "bit", nullable: false),
                    veteran = table.Column<bool>(type: "bit", nullable: false),
                    fresh_blood = table.Column<bool>(type: "bit", nullable: false),
                    inactive = table.Column<bool>(type: "bit", nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(1999, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueOfLegendsRankHistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_Player_LoLRankHistory",
                        column: x => x.player_id,
                        principalTable: "Player",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Tournament",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    state = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    logo_url = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    phase2_challonge_url = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    planned_from = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(1999, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    planned_to = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(1999, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    winner_id = table.Column<int>(type: "int", nullable: true),
                    rules = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    win_points = table.Column<int>(type: "int", nullable: false, defaultValue: 3),
                    loose_points = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    draw_points = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    featured = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    phase_one_double_round = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournament", x => x.id);
                    table.ForeignKey(
                        name: "FK_Tournament_Player_Winner",
                        column: x => x.winner_id,
                        principalTable: "Player",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "LeagueOfLegendsGameTimelineFrameParticipant",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    timeline_frame_id = table.Column<int>(type: "int", nullable: false),
                    current_gold = table.Column<int>(type: "int", nullable: false),
                    gold_per_second = table.Column<int>(type: "int", nullable: false),
                    jungle_minions_killed = table.Column<int>(type: "int", nullable: false),
                    level = table.Column<int>(type: "int", nullable: false),
                    minions_killed = table.Column<int>(type: "int", nullable: false),
                    participantId = table.Column<int>(type: "int", nullable: false),
                    time_enemy_spent_controlled = table.Column<int>(type: "int", nullable: false),
                    total_gold = table.Column<int>(type: "int", nullable: false),
                    xp = table.Column<int>(type: "int", nullable: false),
                    participantPuuid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    magic_damage_done = table.Column<int>(type: "int", nullable: false),
                    magic_damage_done_to_champions = table.Column<int>(type: "int", nullable: false),
                    magic_damage_taken = table.Column<int>(type: "int", nullable: false),
                    physical_damage_done = table.Column<int>(type: "int", nullable: false),
                    physical_damage_done_to_champions = table.Column<int>(type: "int", nullable: false),
                    physical_damage_taken = table.Column<int>(type: "int", nullable: false),
                    total_damage_done = table.Column<int>(type: "int", nullable: false),
                    total_damage_done_to_champions = table.Column<int>(type: "int", nullable: false),
                    total_damage_taken = table.Column<int>(type: "int", nullable: false),
                    true_damage_done = table.Column<int>(type: "int", nullable: false),
                    true_damage_done_to_champions = table.Column<int>(type: "int", nullable: false),
                    true_damage_taken = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueOfLegendsGameTimelineFrameParticipant", x => x.id);
                    table.ForeignKey(
                        name: "FK_LoL_Game_Frame_Participant",
                        column: x => x.timeline_frame_id,
                        principalTable: "LeagueOfLegendsGameTimelineFrame",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "FifaGamePlayed",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    played_on = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(1999, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    team_1_id = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    team_2_id = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    team_score_1 = table.Column<int>(type: "int", maxLength: 100, nullable: false, defaultValue: 0),
                    team_score_2 = table.Column<int>(type: "int", maxLength: 100, nullable: false, defaultValue: 0),
                    platform_id = table.Column<int>(type: "int", nullable: false),
                    created_by_id = table.Column<int>(type: "int", nullable: false),
                    season_id = table.Column<int>(type: "int", nullable: false),
                    is_played = table.Column<bool>(type: "bit", nullable: false),
                    tournament_id = table.Column<int>(type: "int", nullable: true),
                    phase = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FifaGamePlayed", x => x.id);
                    table.ForeignKey(
                        name: "FK_FifaGamePlayed_FifaTeam1",
                        column: x => x.team_1_id,
                        principalTable: "FifaTeam",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_FifaGamePlayed_FifaTeam2",
                        column: x => x.team_2_id,
                        principalTable: "FifaTeam",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_FifaGamePlayed_Platform",
                        column: x => x.platform_id,
                        principalTable: "Platform",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_FifaGamePlayed_Player_Created_By",
                        column: x => x.created_by_id,
                        principalTable: "Player",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_FifaGamePlayed_Season",
                        column: x => x.season_id,
                        principalTable: "Season",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_FifaGamePlayed_Tournament",
                        column: x => x.tournament_id,
                        principalTable: "Tournament",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TournamentPlayer",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    fifa_team_id = table.Column<int>(type: "int", nullable: false),
                    tournament_id = table.Column<int>(type: "int", nullable: false),
                    phase_1_score = table.Column<int>(type: "int", nullable: true),
                    joined_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(1999, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentPlayer", x => x.id);
                    table.ForeignKey(
                        name: "FK_TournamentPlayer_FifaTeam",
                        column: x => x.fifa_team_id,
                        principalTable: "FifaTeam",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TournamentPlayer_Player",
                        column: x => x.player_id,
                        principalTable: "Player",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TournamentPlayer_Tournament",
                        column: x => x.tournament_id,
                        principalTable: "Tournament",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "FifaTeamPlayer",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    fifa_game_id = table.Column<int>(type: "int", nullable: false),
                    team = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FifaTeamPlayer", x => x.id);
                    table.ForeignKey(
                        name: "FK_FifaTeamPlayer_FifaGamePlayed",
                        column: x => x.fifa_game_id,
                        principalTable: "FifaGamePlayed",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FifaTeamPlayer_Player",
                        column: x => x.player_id,
                        principalTable: "Player",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Highlight",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    created_by_id = table.Column<int>(type: "int", nullable: false),
                    fifa_game_id = table.Column<int>(type: "int", nullable: false),
                    external_url = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Highlight", x => x.id);
                    table.ForeignKey(
                        name: "FK_FifaGame_Highlight",
                        column: x => x.fifa_game_id,
                        principalTable: "FifaGamePlayed",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Highlight_Player_Created_By",
                        column: x => x.created_by_id,
                        principalTable: "Player",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FifaGamePlayed_created_by_id",
                table: "FifaGamePlayed",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_FifaGamePlayed_platform_id",
                table: "FifaGamePlayed",
                column: "platform_id");

            migrationBuilder.CreateIndex(
                name: "IX_FifaGamePlayed_season_id",
                table: "FifaGamePlayed",
                column: "season_id");

            migrationBuilder.CreateIndex(
                name: "IX_FifaGamePlayed_team_1_id",
                table: "FifaGamePlayed",
                column: "team_1_id");

            migrationBuilder.CreateIndex(
                name: "IX_FifaGamePlayed_team_2_id",
                table: "FifaGamePlayed",
                column: "team_2_id");

            migrationBuilder.CreateIndex(
                name: "IX_FifaGamePlayed_tournament_id",
                table: "FifaGamePlayed",
                column: "tournament_id");

            migrationBuilder.CreateIndex(
                name: "IX_FifaTeamPlayer_fifa_game_id",
                table: "FifaTeamPlayer",
                column: "fifa_game_id");

            migrationBuilder.CreateIndex(
                name: "IX_FifaTeamPlayer_player_id",
                table: "FifaTeamPlayer",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_Highlight_created_by_id",
                table: "Highlight",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_Highlight_fifa_game_id",
                table: "Highlight",
                column: "fifa_game_id");

            migrationBuilder.CreateIndex(
                name: "IX_LeagueOfLegendsGameParticipant_MatchId",
                table: "LeagueOfLegendsGameParticipant",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_LeagueOfLegendsGameParticipant_player_id",
                table: "LeagueOfLegendsGameParticipant",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_LeagueOfLegendsGameTimelineFrame_match_id",
                table: "LeagueOfLegendsGameTimelineFrame",
                column: "match_id");

            migrationBuilder.CreateIndex(
                name: "IX_LeagueOfLegendsGameTimelineFrameParticipant_timeline_frame_id",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                column: "timeline_frame_id");

            migrationBuilder.CreateIndex(
                name: "IX_LeagueOfLegendsRankHistory_player_id",
                table: "LeagueOfLegendsRankHistory",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tournament_winner_id",
                table: "Tournament",
                column: "winner_id");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentPlayer_fifa_team_id",
                table: "TournamentPlayer",
                column: "fifa_team_id");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentPlayer_player_id",
                table: "TournamentPlayer",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentPlayer_tournament_id",
                table: "TournamentPlayer",
                column: "tournament_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Changelog");

            migrationBuilder.DropTable(
                name: "FifaTeamPlayer");

            migrationBuilder.DropTable(
                name: "Highlight");

            migrationBuilder.DropTable(
                name: "LeagueOfLegendsGameParticipant");

            migrationBuilder.DropTable(
                name: "LeagueOfLegendsGameTimelineFrameParticipant");

            migrationBuilder.DropTable(
                name: "LeagueOfLegendsRankHistory");

            migrationBuilder.DropTable(
                name: "TournamentPlayer");

            migrationBuilder.DropTable(
                name: "FifaGamePlayed");

            migrationBuilder.DropTable(
                name: "LeagueOfLegendsGameTimelineFrame");

            migrationBuilder.DropTable(
                name: "FifaTeam");

            migrationBuilder.DropTable(
                name: "Platform");

            migrationBuilder.DropTable(
                name: "Season");

            migrationBuilder.DropTable(
                name: "Tournament");

            migrationBuilder.DropTable(
                name: "LeagueOfLegendsGame");

            migrationBuilder.DropTable(
                name: "Player");
        }
    }
}
