using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOn.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Added_LoL_Rating_Mvp_Ace_Teams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "magic_damage_to_champions",
                table: "LeagueOfLegendsGameParticipantStat",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "physical_damage_to_champions",
                table: "LeagueOfLegendsGameParticipantStat",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "rating",
                table: "LeagueOfLegendsGameParticipantStat",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "time_cc_others_seconds",
                table: "LeagueOfLegendsGameParticipantStat",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "true_damage_to_champions",
                table: "LeagueOfLegendsGameParticipantStat",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ace_participant_id",
                table: "LeagueOfLegendsGame",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "mvp_participant_id",
                table: "LeagueOfLegendsGame",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LeagueOfLegendsGameTeam",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    match_id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    team_id = table.Column<int>(type: "int", nullable: false),
                    win = table.Column<bool>(type: "bit", nullable: false),
                    champion_kills = table.Column<int>(type: "int", nullable: false),
                    tower_kills = table.Column<int>(type: "int", nullable: false),
                    inhibitor_kills = table.Column<int>(type: "int", nullable: false),
                    dragon_kills = table.Column<int>(type: "int", nullable: false),
                    rift_herald_kills = table.Column<int>(type: "int", nullable: false),
                    baron_kills = table.Column<int>(type: "int", nullable: false),
                    horde_kills = table.Column<int>(type: "int", nullable: false),
                    first_blood = table.Column<bool>(type: "bit", nullable: false),
                    first_tower = table.Column<bool>(type: "bit", nullable: false),
                    first_inhibitor = table.Column<bool>(type: "bit", nullable: false),
                    first_dragon = table.Column<bool>(type: "bit", nullable: false),
                    first_baron = table.Column<bool>(type: "bit", nullable: false),
                    first_rift_herald = table.Column<bool>(type: "bit", nullable: false),
                    first_horde = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueOfLegendsGameTeam", x => x.id);
                    table.ForeignKey(
                        name: "FK_LoL_Game_Team",
                        column: x => x.match_id,
                        principalTable: "LeagueOfLegendsGame",
                        principalColumn: "match_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeagueOfLegendsGame_ace_participant_id",
                table: "LeagueOfLegendsGame",
                column: "ace_participant_id");

            migrationBuilder.CreateIndex(
                name: "IX_LeagueOfLegendsGame_mvp_participant_id",
                table: "LeagueOfLegendsGame",
                column: "mvp_participant_id");

            migrationBuilder.CreateIndex(
                name: "IX_LeagueOfLegendsGameTeam_match_id",
                table: "LeagueOfLegendsGameTeam",
                column: "match_id");

            migrationBuilder.AddForeignKey(
                name: "FK_LoLGame_Ace_Participant",
                table: "LeagueOfLegendsGame",
                column: "ace_participant_id",
                principalTable: "LeagueOfLegendsGameParticipant",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoLGame_Mvp_Participant",
                table: "LeagueOfLegendsGame",
                column: "mvp_participant_id",
                principalTable: "LeagueOfLegendsGameParticipant",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoLGame_Ace_Participant",
                table: "LeagueOfLegendsGame");

            migrationBuilder.DropForeignKey(
                name: "FK_LoLGame_Mvp_Participant",
                table: "LeagueOfLegendsGame");

            migrationBuilder.DropTable(
                name: "LeagueOfLegendsGameTeam");

            migrationBuilder.DropIndex(
                name: "IX_LeagueOfLegendsGame_ace_participant_id",
                table: "LeagueOfLegendsGame");

            migrationBuilder.DropIndex(
                name: "IX_LeagueOfLegendsGame_mvp_participant_id",
                table: "LeagueOfLegendsGame");

            migrationBuilder.DropColumn(
                name: "magic_damage_to_champions",
                table: "LeagueOfLegendsGameParticipantStat");

            migrationBuilder.DropColumn(
                name: "physical_damage_to_champions",
                table: "LeagueOfLegendsGameParticipantStat");

            migrationBuilder.DropColumn(
                name: "rating",
                table: "LeagueOfLegendsGameParticipantStat");

            migrationBuilder.DropColumn(
                name: "time_cc_others_seconds",
                table: "LeagueOfLegendsGameParticipantStat");

            migrationBuilder.DropColumn(
                name: "true_damage_to_champions",
                table: "LeagueOfLegendsGameParticipantStat");

            migrationBuilder.DropColumn(
                name: "ace_participant_id",
                table: "LeagueOfLegendsGame");

            migrationBuilder.DropColumn(
                name: "mvp_participant_id",
                table: "LeagueOfLegendsGame");
        }
    }
}
