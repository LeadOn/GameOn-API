﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOn.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Fixed_Missing_PlayerID_on_LoL_Rank_History : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "LeagueOfLegendsRankHistory",
                newName: "player_id");

            migrationBuilder.RenameIndex(
                name: "IX_LeagueOfLegendsRankHistory_PlayerId",
                table: "LeagueOfLegendsRankHistory",
                newName: "IX_LeagueOfLegendsRankHistory_player_id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "joined_at",
                table: "TournamentPlayer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 19, 17, 55, 15, 359, DateTimeKind.Utc).AddTicks(8449),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 19, 17, 48, 22, 828, DateTimeKind.Utc).AddTicks(363));

            migrationBuilder.AlterColumn<DateTime>(
                name: "planned_to",
                table: "Tournament",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 20, 17, 55, 15, 359, DateTimeKind.Utc).AddTicks(5697),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 20, 17, 48, 22, 827, DateTimeKind.Utc).AddTicks(7290));

            migrationBuilder.AlterColumn<DateTime>(
                name: "planned_from",
                table: "Tournament",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 19, 17, 55, 15, 359, DateTimeKind.Utc).AddTicks(5445),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 19, 17, 48, 22, 827, DateTimeKind.Utc).AddTicks(7049));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "Player",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 19, 17, 55, 15, 359, DateTimeKind.Utc).AddTicks(3805),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 19, 17, 48, 22, 827, DateTimeKind.Utc).AddTicks(5488));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "LeagueOfLegendsRankHistory",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 19, 18, 55, 15, 364, DateTimeKind.Local).AddTicks(2454),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 19, 18, 48, 22, 832, DateTimeKind.Local).AddTicks(4492));

            migrationBuilder.AlterColumn<DateTime>(
                name: "played_on",
                table: "FifaGamePlayed",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 19, 18, 55, 15, 360, DateTimeKind.Local).AddTicks(2588),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 19, 18, 48, 22, 828, DateTimeKind.Local).AddTicks(4816));

            migrationBuilder.AlterColumn<DateTime>(
                name: "publication_date",
                table: "Changelog",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 19, 18, 55, 15, 364, DateTimeKind.Local).AddTicks(1498),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 19, 18, 48, 22, 832, DateTimeKind.Local).AddTicks(3577));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "player_id",
                table: "LeagueOfLegendsRankHistory",
                newName: "PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_LeagueOfLegendsRankHistory_player_id",
                table: "LeagueOfLegendsRankHistory",
                newName: "IX_LeagueOfLegendsRankHistory_PlayerId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "joined_at",
                table: "TournamentPlayer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 19, 17, 48, 22, 828, DateTimeKind.Utc).AddTicks(363),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 19, 17, 55, 15, 359, DateTimeKind.Utc).AddTicks(8449));

            migrationBuilder.AlterColumn<DateTime>(
                name: "planned_to",
                table: "Tournament",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 20, 17, 48, 22, 827, DateTimeKind.Utc).AddTicks(7290),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 20, 17, 55, 15, 359, DateTimeKind.Utc).AddTicks(5697));

            migrationBuilder.AlterColumn<DateTime>(
                name: "planned_from",
                table: "Tournament",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 19, 17, 48, 22, 827, DateTimeKind.Utc).AddTicks(7049),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 19, 17, 55, 15, 359, DateTimeKind.Utc).AddTicks(5445));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "Player",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 19, 17, 48, 22, 827, DateTimeKind.Utc).AddTicks(5488),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 19, 17, 55, 15, 359, DateTimeKind.Utc).AddTicks(3805));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "LeagueOfLegendsRankHistory",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 19, 18, 48, 22, 832, DateTimeKind.Local).AddTicks(4492),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 19, 18, 55, 15, 364, DateTimeKind.Local).AddTicks(2454));

            migrationBuilder.AlterColumn<DateTime>(
                name: "played_on",
                table: "FifaGamePlayed",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 19, 18, 48, 22, 828, DateTimeKind.Local).AddTicks(4816),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 19, 18, 55, 15, 360, DateTimeKind.Local).AddTicks(2588));

            migrationBuilder.AlterColumn<DateTime>(
                name: "publication_date",
                table: "Changelog",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 19, 18, 48, 22, 832, DateTimeKind.Local).AddTicks(3577),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 19, 18, 55, 15, 364, DateTimeKind.Local).AddTicks(1498));
        }
    }
}
