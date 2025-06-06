﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOn.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Added_Retrieved_Date_on_LoL_Game : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "championId",
                table: "LeagueOfLegendsGameParticipant",
                newName: "champion_id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "joined_at",
                table: "TournamentPlayer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 17, 2, 45, 129, DateTimeKind.Utc).AddTicks(2167),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 16, 57, 59, 975, DateTimeKind.Utc).AddTicks(216));

            migrationBuilder.AlterColumn<DateTime>(
                name: "planned_to",
                table: "Tournament",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 29, 17, 2, 45, 128, DateTimeKind.Utc).AddTicks(9764),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 29, 16, 57, 59, 974, DateTimeKind.Utc).AddTicks(8002));

            migrationBuilder.AlterColumn<DateTime>(
                name: "planned_from",
                table: "Tournament",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 17, 2, 45, 128, DateTimeKind.Utc).AddTicks(9552),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 16, 57, 59, 974, DateTimeKind.Utc).AddTicks(7796));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "Player",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 17, 2, 45, 128, DateTimeKind.Utc).AddTicks(8230),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 16, 57, 59, 974, DateTimeKind.Utc).AddTicks(6497));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "LeagueOfLegendsRankHistory",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 18, 2, 45, 133, DateTimeKind.Local).AddTicks(4817),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 17, 57, 59, 979, DateTimeKind.Local).AddTicks(1088));

            migrationBuilder.AddColumn<DateTime>(
                name: "retrieved_on",
                table: "LeagueOfLegendsGame",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 18, 2, 45, 134, DateTimeKind.Local).AddTicks(760));

            migrationBuilder.AlterColumn<DateTime>(
                name: "played_on",
                table: "FifaGamePlayed",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 18, 2, 45, 129, DateTimeKind.Local).AddTicks(5594),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 17, 57, 59, 975, DateTimeKind.Local).AddTicks(4027));

            migrationBuilder.AlterColumn<DateTime>(
                name: "publication_date",
                table: "Changelog",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 18, 2, 45, 133, DateTimeKind.Local).AddTicks(3956),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 17, 57, 59, 979, DateTimeKind.Local).AddTicks(40));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "retrieved_on",
                table: "LeagueOfLegendsGame");

            migrationBuilder.RenameColumn(
                name: "champion_id",
                table: "LeagueOfLegendsGameParticipant",
                newName: "championId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "joined_at",
                table: "TournamentPlayer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 16, 57, 59, 975, DateTimeKind.Utc).AddTicks(216),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 17, 2, 45, 129, DateTimeKind.Utc).AddTicks(2167));

            migrationBuilder.AlterColumn<DateTime>(
                name: "planned_to",
                table: "Tournament",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 29, 16, 57, 59, 974, DateTimeKind.Utc).AddTicks(8002),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 29, 17, 2, 45, 128, DateTimeKind.Utc).AddTicks(9764));

            migrationBuilder.AlterColumn<DateTime>(
                name: "planned_from",
                table: "Tournament",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 16, 57, 59, 974, DateTimeKind.Utc).AddTicks(7796),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 17, 2, 45, 128, DateTimeKind.Utc).AddTicks(9552));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "Player",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 16, 57, 59, 974, DateTimeKind.Utc).AddTicks(6497),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 17, 2, 45, 128, DateTimeKind.Utc).AddTicks(8230));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "LeagueOfLegendsRankHistory",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 17, 57, 59, 979, DateTimeKind.Local).AddTicks(1088),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 18, 2, 45, 133, DateTimeKind.Local).AddTicks(4817));

            migrationBuilder.AlterColumn<DateTime>(
                name: "played_on",
                table: "FifaGamePlayed",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 17, 57, 59, 975, DateTimeKind.Local).AddTicks(4027),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 18, 2, 45, 129, DateTimeKind.Local).AddTicks(5594));

            migrationBuilder.AlterColumn<DateTime>(
                name: "publication_date",
                table: "Changelog",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 17, 57, 59, 979, DateTimeKind.Local).AddTicks(40),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 18, 2, 45, 133, DateTimeKind.Local).AddTicks(3956));
        }
    }
}
