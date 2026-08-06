using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOn.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Added_Missing_Pings_In_LoLGames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BaitPings",
                table: "LeagueOfLegendsGameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BasicPings",
                table: "LeagueOfLegendsGameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DangerPings",
                table: "LeagueOfLegendsGameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EnemyMissingPings",
                table: "LeagueOfLegendsGameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EnemyVisionPings",
                table: "LeagueOfLegendsGameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HoldPings",
                table: "LeagueOfLegendsGameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NeedVisionPings",
                table: "LeagueOfLegendsGameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OnMyWayPings",
                table: "LeagueOfLegendsGameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PushPings",
                table: "LeagueOfLegendsGameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VisionClearedPings",
                table: "LeagueOfLegendsGameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaitPings",
                table: "LeagueOfLegendsGameParticipant");

            migrationBuilder.DropColumn(
                name: "BasicPings",
                table: "LeagueOfLegendsGameParticipant");

            migrationBuilder.DropColumn(
                name: "DangerPings",
                table: "LeagueOfLegendsGameParticipant");

            migrationBuilder.DropColumn(
                name: "EnemyMissingPings",
                table: "LeagueOfLegendsGameParticipant");

            migrationBuilder.DropColumn(
                name: "EnemyVisionPings",
                table: "LeagueOfLegendsGameParticipant");

            migrationBuilder.DropColumn(
                name: "HoldPings",
                table: "LeagueOfLegendsGameParticipant");

            migrationBuilder.DropColumn(
                name: "NeedVisionPings",
                table: "LeagueOfLegendsGameParticipant");

            migrationBuilder.DropColumn(
                name: "OnMyWayPings",
                table: "LeagueOfLegendsGameParticipant");

            migrationBuilder.DropColumn(
                name: "PushPings",
                table: "LeagueOfLegendsGameParticipant");

            migrationBuilder.DropColumn(
                name: "VisionClearedPings",
                table: "LeagueOfLegendsGameParticipant");
        }
    }
}
