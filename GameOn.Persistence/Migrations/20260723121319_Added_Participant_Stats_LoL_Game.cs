using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOn.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Added_Participant_Stats_LoL_Game : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeagueOfLegendsGameParticipantStat",
                columns: table => new
                {
                    lol_game_participant_id = table.Column<int>(type: "int", nullable: false),
                    game_duration_seconds = table.Column<int>(type: "int", nullable: false),
                    kda = table.Column<double>(type: "float", nullable: false),
                    kill_participation_percent = table.Column<double>(type: "float", nullable: false),
                    creep_score = table.Column<int>(type: "int", nullable: false),
                    cs_per_minute = table.Column<double>(type: "float", nullable: false),
                    gold_earned = table.Column<int>(type: "int", nullable: false),
                    gold_per_minute = table.Column<double>(type: "float", nullable: false),
                    damage_dealt_to_champions = table.Column<int>(type: "int", nullable: false),
                    damage_per_minute = table.Column<double>(type: "float", nullable: false),
                    damage_taken = table.Column<int>(type: "int", nullable: false),
                    wards_placed = table.Column<int>(type: "int", nullable: false),
                    wards_killed = table.Column<int>(type: "int", nullable: false),
                    computed_on = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueOfLegendsGameParticipantStat", x => x.lol_game_participant_id);
                    table.ForeignKey(
                        name: "FK_LoL_Game_Participant_Stat",
                        column: x => x.lol_game_participant_id,
                        principalTable: "LeagueOfLegendsGameParticipant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeagueOfLegendsGameParticipantStat");
        }
    }
}
