using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOn.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Reworked_LolGameTimelineFrame_Relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoL_Game_Frame",
                table: "LeagueOfLegendsGameTimelineFrame");

            migrationBuilder.AddForeignKey(
                name: "FK_LoL_Game_Frame",
                table: "LeagueOfLegendsGameTimelineFrame",
                column: "match_id",
                principalTable: "LeagueOfLegendsGame",
                principalColumn: "match_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoL_Game_Frame",
                table: "LeagueOfLegendsGameTimelineFrame");

            migrationBuilder.AddForeignKey(
                name: "FK_LoL_Game_Frame",
                table: "LeagueOfLegendsGameTimelineFrame",
                column: "match_id",
                principalTable: "LeagueOfLegendsGame",
                principalColumn: "match_id");
        }
    }
}
