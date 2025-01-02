﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOn.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Added_Fields_On_LoL_Game_Participants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "joined_at",
                table: "TournamentPlayer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 16, 43, 36, 558, DateTimeKind.Utc).AddTicks(1933),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 15, 39, 23, 654, DateTimeKind.Utc).AddTicks(5044));

            migrationBuilder.AlterColumn<DateTime>(
                name: "planned_to",
                table: "Tournament",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 29, 16, 43, 36, 557, DateTimeKind.Utc).AddTicks(9550),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 29, 15, 39, 23, 654, DateTimeKind.Utc).AddTicks(2286));

            migrationBuilder.AlterColumn<DateTime>(
                name: "planned_from",
                table: "Tournament",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 16, 43, 36, 557, DateTimeKind.Utc).AddTicks(9322),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 15, 39, 23, 654, DateTimeKind.Utc).AddTicks(2045));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "Player",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 16, 43, 36, 557, DateTimeKind.Utc).AddTicks(7442),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 15, 39, 23, 654, DateTimeKind.Utc).AddTicks(412));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "LeagueOfLegendsRankHistory",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 17, 43, 36, 562, DateTimeKind.Local).AddTicks(2522),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 16, 39, 23, 658, DateTimeKind.Local).AddTicks(8068));

            migrationBuilder.AddColumn<string>(
                name: "puuid",
                table: "LeagueOfLegendsGameParticipant",
                type: "varchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "riot_id_game_name",
                table: "LeagueOfLegendsGameParticipant",
                type: "varchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "riot_id_tagline",
                table: "LeagueOfLegendsGameParticipant",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "played_on",
                table: "FifaGamePlayed",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 17, 43, 36, 558, DateTimeKind.Local).AddTicks(5321),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 16, 39, 23, 654, DateTimeKind.Local).AddTicks(9856));

            migrationBuilder.AlterColumn<DateTime>(
                name: "publication_date",
                table: "Changelog",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 17, 43, 36, 562, DateTimeKind.Local).AddTicks(1633),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 16, 39, 23, 658, DateTimeKind.Local).AddTicks(6752));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "puuid",
                table: "LeagueOfLegendsGameParticipant");

            migrationBuilder.DropColumn(
                name: "riot_id_game_name",
                table: "LeagueOfLegendsGameParticipant");

            migrationBuilder.DropColumn(
                name: "riot_id_tagline",
                table: "LeagueOfLegendsGameParticipant");

            migrationBuilder.AlterColumn<DateTime>(
                name: "joined_at",
                table: "TournamentPlayer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 15, 39, 23, 654, DateTimeKind.Utc).AddTicks(5044),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 16, 43, 36, 558, DateTimeKind.Utc).AddTicks(1933));

            migrationBuilder.AlterColumn<DateTime>(
                name: "planned_to",
                table: "Tournament",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 29, 15, 39, 23, 654, DateTimeKind.Utc).AddTicks(2286),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 29, 16, 43, 36, 557, DateTimeKind.Utc).AddTicks(9550));

            migrationBuilder.AlterColumn<DateTime>(
                name: "planned_from",
                table: "Tournament",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 15, 39, 23, 654, DateTimeKind.Utc).AddTicks(2045),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 16, 43, 36, 557, DateTimeKind.Utc).AddTicks(9322));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "Player",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 15, 39, 23, 654, DateTimeKind.Utc).AddTicks(412),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 16, 43, 36, 557, DateTimeKind.Utc).AddTicks(7442));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "LeagueOfLegendsRankHistory",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 16, 39, 23, 658, DateTimeKind.Local).AddTicks(8068),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 17, 43, 36, 562, DateTimeKind.Local).AddTicks(2522));

            migrationBuilder.AlterColumn<DateTime>(
                name: "played_on",
                table: "FifaGamePlayed",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 16, 39, 23, 654, DateTimeKind.Local).AddTicks(9856),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 17, 43, 36, 558, DateTimeKind.Local).AddTicks(5321));

            migrationBuilder.AlterColumn<DateTime>(
                name: "publication_date",
                table: "Changelog",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 16, 39, 23, 658, DateTimeKind.Local).AddTicks(6752),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 28, 17, 43, 36, 562, DateTimeKind.Local).AddTicks(1633));
        }
    }
}