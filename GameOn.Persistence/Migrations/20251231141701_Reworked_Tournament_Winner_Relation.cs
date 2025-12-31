using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOn.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Reworked_Tournament_Winner_Relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournament_Player_Winner",
                table: "Tournament");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournament_Player_Winner",
                table: "Tournament",
                column: "winner_id",
                principalTable: "Player",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournament_Player_Winner",
                table: "Tournament");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournament_Player_Winner",
                table: "Tournament",
                column: "winner_id",
                principalTable: "Player",
                principalColumn: "id");
        }
    }
}
