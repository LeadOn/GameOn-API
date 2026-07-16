using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOn.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Added_LoLQueue_link_with_LoLGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "queue_id",
                table: "LeagueOfLegendsGame",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeagueOfLegendsGame_queue_id",
                table: "LeagueOfLegendsGame",
                column: "queue_id");

            migrationBuilder.AddForeignKey(
                name: "FK_LoLGame_LoLQueue",
                table: "LeagueOfLegendsGame",
                column: "queue_id",
                principalTable: "LeagueOfLegendsQueue",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoLGame_LoLQueue",
                table: "LeagueOfLegendsGame");

            migrationBuilder.DropIndex(
                name: "IX_LeagueOfLegendsGame_queue_id",
                table: "LeagueOfLegendsGame");

            migrationBuilder.DropColumn(
                name: "queue_id",
                table: "LeagueOfLegendsGame");
        }
    }
}
