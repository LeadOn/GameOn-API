using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOn.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Reworked_LeagueOfLegendsRankHistory_Relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_LoLRankHistory",
                table: "LeagueOfLegendsRankHistory");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_LoLRankHistory",
                table: "LeagueOfLegendsRankHistory",
                column: "player_id",
                principalTable: "Player",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_LoLRankHistory",
                table: "LeagueOfLegendsRankHistory");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_LoLRankHistory",
                table: "LeagueOfLegendsRankHistory",
                column: "player_id",
                principalTable: "Player",
                principalColumn: "id");
        }
    }
}
