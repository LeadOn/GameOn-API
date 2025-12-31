using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOn.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Reworked_LolGame_Relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoL_Games_Participants",
                table: "LeagueOfLegendsGameParticipant");

            migrationBuilder.AddForeignKey(
                name: "FK_LoL_Games_Participants",
                table: "LeagueOfLegendsGameParticipant",
                column: "MatchId",
                principalTable: "LeagueOfLegendsGame",
                principalColumn: "match_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoL_Games_Participants",
                table: "LeagueOfLegendsGameParticipant");

            migrationBuilder.AddForeignKey(
                name: "FK_LoL_Games_Participants",
                table: "LeagueOfLegendsGameParticipant",
                column: "MatchId",
                principalTable: "LeagueOfLegendsGame",
                principalColumn: "match_id");
        }
    }
}
