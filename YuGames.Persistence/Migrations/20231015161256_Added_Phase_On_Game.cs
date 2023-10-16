using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YuGames.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Added_Phase_On_Game : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "joined_at",
                table: "TournamentPlayer",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 15, 16, 12, 56, 397, DateTimeKind.Utc).AddTicks(1670),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 10, 5, 11, 43, 14, 560, DateTimeKind.Utc).AddTicks(8400));

            migrationBuilder.AlterColumn<DateTime>(
                name: "planned_to",
                table: "Tournament",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 16, 16, 12, 56, 397, DateTimeKind.Utc).AddTicks(1290),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 10, 6, 11, 43, 14, 560, DateTimeKind.Utc).AddTicks(8080));

            migrationBuilder.AlterColumn<DateTime>(
                name: "planned_from",
                table: "Tournament",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 15, 16, 12, 56, 397, DateTimeKind.Utc).AddTicks(1210),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 10, 5, 11, 43, 14, 560, DateTimeKind.Utc).AddTicks(8000));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "Player",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 15, 16, 12, 56, 397, DateTimeKind.Utc).AddTicks(3950),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 10, 5, 11, 43, 14, 561, DateTimeKind.Utc).AddTicks(580));

            migrationBuilder.AlterColumn<DateTime>(
                name: "played_on",
                table: "FifaGamePlayed",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 15, 18, 12, 56, 397, DateTimeKind.Local).AddTicks(4530),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 10, 5, 13, 43, 14, 561, DateTimeKind.Local).AddTicks(1220));

            migrationBuilder.AddColumn<int>(
                name: "phase",
                table: "FifaGamePlayed",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "phase",
                table: "FifaGamePlayed");

            migrationBuilder.AlterColumn<DateTime>(
                name: "joined_at",
                table: "TournamentPlayer",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 5, 11, 43, 14, 560, DateTimeKind.Utc).AddTicks(8400),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 10, 15, 16, 12, 56, 397, DateTimeKind.Utc).AddTicks(1670));

            migrationBuilder.AlterColumn<DateTime>(
                name: "planned_to",
                table: "Tournament",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 6, 11, 43, 14, 560, DateTimeKind.Utc).AddTicks(8080),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 10, 16, 16, 12, 56, 397, DateTimeKind.Utc).AddTicks(1290));

            migrationBuilder.AlterColumn<DateTime>(
                name: "planned_from",
                table: "Tournament",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 5, 11, 43, 14, 560, DateTimeKind.Utc).AddTicks(8000),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 10, 15, 16, 12, 56, 397, DateTimeKind.Utc).AddTicks(1210));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "Player",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 5, 11, 43, 14, 561, DateTimeKind.Utc).AddTicks(580),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 10, 15, 16, 12, 56, 397, DateTimeKind.Utc).AddTicks(3950));

            migrationBuilder.AlterColumn<DateTime>(
                name: "played_on",
                table: "FifaGamePlayed",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 5, 13, 43, 14, 561, DateTimeKind.Local).AddTicks(1220),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 10, 15, 18, 12, 56, 397, DateTimeKind.Local).AddTicks(4530));
        }
    }
}
