﻿// <auto-generated />

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOn.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class First_Release_MySQL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FifaTeam",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FifaTeam", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Platform",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platform", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    KeycloakId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password_salt = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password_hash = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    full_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nickname = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    profile_picture_url = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_on = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2024, 8, 5, 8, 57, 16, 328, DateTimeKind.Utc).AddTicks(1244))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Season",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Season", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tournament",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    logo_url = table.Column<string>(type: "varchar(3000)", maxLength: 3000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phase2_challonge_url = table.Column<string>(type: "varchar(3000)", maxLength: 3000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    planned_from = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2024, 8, 5, 8, 57, 16, 328, DateTimeKind.Utc).AddTicks(2008)),
                    planned_to = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2024, 8, 6, 8, 57, 16, 328, DateTimeKind.Utc).AddTicks(2177))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournament", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FifaGamePlayed",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    played_on = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2024, 8, 5, 10, 57, 16, 328, DateTimeKind.Local).AddTicks(5822)),
                    team_code_1 = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    team_1_id = table.Column<int>(type: "int", nullable: true),
                    team_2_id = table.Column<int>(type: "int", nullable: true),
                    team_code_2 = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    team_score_1 = table.Column<int>(type: "int", maxLength: 100, nullable: false, defaultValue: 0),
                    team_score_2 = table.Column<int>(type: "int", maxLength: 100, nullable: false, defaultValue: 0),
                    platform_id = table.Column<int>(type: "int", nullable: false),
                    created_by_id = table.Column<int>(type: "int", nullable: false),
                    season_id = table.Column<int>(type: "int", nullable: false),
                    is_played = table.Column<bool>(type: "tinyint(1)", nullable: false),
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
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FifaGamePlayed_FifaTeam2",
                        column: x => x.team_2_id,
                        principalTable: "FifaTeam",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FifaGamePlayed_Platform",
                        column: x => x.platform_id,
                        principalTable: "Platform",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FifaGamePlayed_Player_Created_By",
                        column: x => x.created_by_id,
                        principalTable: "Player",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FifaGamePlayed_Season",
                        column: x => x.season_id,
                        principalTable: "Season",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FifaGamePlayed_Tournament",
                        column: x => x.tournament_id,
                        principalTable: "Tournament",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TournamentPlayer",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    player_id = table.Column<int>(type: "int", nullable: false),
                    fifa_team_id = table.Column<int>(type: "int", nullable: false),
                    tournament_id = table.Column<int>(type: "int", nullable: false),
                    phase_1_score = table.Column<int>(type: "int", nullable: true),
                    joined_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2024, 8, 5, 8, 57, 16, 328, DateTimeKind.Utc).AddTicks(2722))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentPlayer", x => x.id);
                    table.ForeignKey(
                        name: "FK_TournamentPlayer_FifaTeam",
                        column: x => x.fifa_team_id,
                        principalTable: "FifaTeam",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TournamentPlayer_Player",
                        column: x => x.player_id,
                        principalTable: "Player",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TournamentPlayer_Tournament",
                        column: x => x.tournament_id,
                        principalTable: "Tournament",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FifaTeamPlayer",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
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
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Highlight",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_by_id = table.Column<int>(type: "int", nullable: false),
                    fifa_game_id = table.Column<int>(type: "int", nullable: false),
                    external_url = table.Column<string>(type: "varchar(3000)", maxLength: 3000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Highlight", x => x.id);
                    table.ForeignKey(
                        name: "FK_FifaGame_Highlight",
                        column: x => x.fifa_game_id,
                        principalTable: "FifaGamePlayed",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Highlight_Player_Created_By",
                        column: x => x.created_by_id,
                        principalTable: "Player",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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
                name: "FifaTeamPlayer");

            migrationBuilder.DropTable(
                name: "Highlight");

            migrationBuilder.DropTable(
                name: "TournamentPlayer");

            migrationBuilder.DropTable(
                name: "FifaGamePlayed");

            migrationBuilder.DropTable(
                name: "FifaTeam");

            migrationBuilder.DropTable(
                name: "Platform");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "Season");

            migrationBuilder.DropTable(
                name: "Tournament");
        }
    }
}
