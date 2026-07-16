using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOn.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Added_Support_For_LoL_Remake_Games : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "GameEndedInEarlySurrender",
                table: "LeagueOfLegendsGameParticipant",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_remake",
                table: "LeagueOfLegendsGame",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameEndedInEarlySurrender",
                table: "LeagueOfLegendsGameParticipant");

            migrationBuilder.DropColumn(
                name: "is_remake",
                table: "LeagueOfLegendsGame");
        }
    }
}
