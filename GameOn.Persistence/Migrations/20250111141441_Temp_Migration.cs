using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOn.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Temp_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "joined_at",
                table: "TournamentPlayer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 11, 14, 14, 40, 935, DateTimeKind.Utc).AddTicks(700),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 19, 0, 27, 48, DateTimeKind.Utc).AddTicks(2713));

            migrationBuilder.AlterColumn<DateTime>(
                name: "planned_to",
                table: "Tournament",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 12, 14, 14, 40, 933, DateTimeKind.Utc).AddTicks(380),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 29, 19, 0, 27, 48, DateTimeKind.Utc).AddTicks(369));

            migrationBuilder.AlterColumn<DateTime>(
                name: "planned_from",
                table: "Tournament",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 11, 14, 14, 40, 933, DateTimeKind.Utc).AddTicks(200),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 19, 0, 27, 48, DateTimeKind.Utc).AddTicks(166));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "Player",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 11, 14, 14, 40, 930, DateTimeKind.Utc).AddTicks(9720),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 19, 0, 27, 47, DateTimeKind.Utc).AddTicks(8743));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "LeagueOfLegendsRankHistory",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 11, 15, 14, 40, 946, DateTimeKind.Local).AddTicks(4140),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 20, 0, 27, 52, DateTimeKind.Local).AddTicks(6219));

            migrationBuilder.AlterColumn<DateTime>(
                name: "retrieved_on",
                table: "LeagueOfLegendsGame",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 11, 15, 14, 40, 947, DateTimeKind.Local).AddTicks(2670),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 20, 0, 27, 53, DateTimeKind.Local).AddTicks(1158));

            migrationBuilder.AlterColumn<DateTime>(
                name: "played_on",
                table: "FifaGamePlayed",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 11, 15, 14, 40, 940, DateTimeKind.Local).AddTicks(2810),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 20, 0, 27, 48, DateTimeKind.Local).AddTicks(7314));

            migrationBuilder.AlterColumn<DateTime>(
                name: "publication_date",
                table: "Changelog",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 11, 15, 14, 40, 945, DateTimeKind.Local).AddTicks(8140),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 20, 0, 27, 52, DateTimeKind.Local).AddTicks(5224));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "joined_at",
                table: "TournamentPlayer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 19, 0, 27, 48, DateTimeKind.Utc).AddTicks(2713),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 1, 11, 14, 14, 40, 935, DateTimeKind.Utc).AddTicks(700));

            migrationBuilder.AlterColumn<DateTime>(
                name: "planned_to",
                table: "Tournament",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 29, 19, 0, 27, 48, DateTimeKind.Utc).AddTicks(369),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 1, 12, 14, 14, 40, 933, DateTimeKind.Utc).AddTicks(380));

            migrationBuilder.AlterColumn<DateTime>(
                name: "planned_from",
                table: "Tournament",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 19, 0, 27, 48, DateTimeKind.Utc).AddTicks(166),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 1, 11, 14, 14, 40, 933, DateTimeKind.Utc).AddTicks(200));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "Player",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 19, 0, 27, 47, DateTimeKind.Utc).AddTicks(8743),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 1, 11, 14, 14, 40, 930, DateTimeKind.Utc).AddTicks(9720));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "LeagueOfLegendsRankHistory",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 20, 0, 27, 52, DateTimeKind.Local).AddTicks(6219),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 1, 11, 15, 14, 40, 946, DateTimeKind.Local).AddTicks(4140));

            migrationBuilder.AlterColumn<DateTime>(
                name: "retrieved_on",
                table: "LeagueOfLegendsGame",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 20, 0, 27, 53, DateTimeKind.Local).AddTicks(1158),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 1, 11, 15, 14, 40, 947, DateTimeKind.Local).AddTicks(2670));

            migrationBuilder.AlterColumn<DateTime>(
                name: "played_on",
                table: "FifaGamePlayed",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 20, 0, 27, 48, DateTimeKind.Local).AddTicks(7314),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 1, 11, 15, 14, 40, 940, DateTimeKind.Local).AddTicks(2810));

            migrationBuilder.AlterColumn<DateTime>(
                name: "publication_date",
                table: "Changelog",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 20, 0, 27, 52, DateTimeKind.Local).AddTicks(5224),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 1, 11, 15, 14, 40, 945, DateTimeKind.Local).AddTicks(8140));
        }
    }
}
