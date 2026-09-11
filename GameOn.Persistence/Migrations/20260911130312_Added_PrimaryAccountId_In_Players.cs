using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOn.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Added_PrimaryAccountId_In_Players : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "primary_player_id",
                table: "Player",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Player_primary_player_id",
                table: "Player",
                column: "primary_player_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Player_primary_player_id",
                table: "Player",
                column: "primary_player_id",
                principalTable: "Player",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_Player_primary_player_id",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Player_primary_player_id",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "primary_player_id",
                table: "Player");
        }
    }
}
