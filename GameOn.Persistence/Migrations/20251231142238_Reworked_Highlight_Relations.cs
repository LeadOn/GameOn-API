using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOn.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Reworked_Highlight_Relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FifaGame_Highlight",
                table: "Highlight");

            migrationBuilder.DropForeignKey(
                name: "FK_Highlight_Player_Created_By",
                table: "Highlight");

            migrationBuilder.AddForeignKey(
                name: "FK_FifaGame_Highlight",
                table: "Highlight",
                column: "fifa_game_id",
                principalTable: "FifaGamePlayed",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Highlight_Player_Created_By",
                table: "Highlight",
                column: "created_by_id",
                principalTable: "Player",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FifaGame_Highlight",
                table: "Highlight");

            migrationBuilder.DropForeignKey(
                name: "FK_Highlight_Player_Created_By",
                table: "Highlight");

            migrationBuilder.AddForeignKey(
                name: "FK_FifaGame_Highlight",
                table: "Highlight",
                column: "fifa_game_id",
                principalTable: "FifaGamePlayed",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Highlight_Player_Created_By",
                table: "Highlight",
                column: "created_by_id",
                principalTable: "Player",
                principalColumn: "id");
        }
    }
}
