using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YuFoot.EntitiesContext.Migrations
{
    /// <inheritdoc />
    public partial class Updated_FifaTeam_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "logo_url",
                table: "FifaTeam");

            migrationBuilder.DropColumn(
                name: "short_name",
                table: "FifaTeam");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "Player",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 13, 0, 38, 22, 327, DateTimeKind.Local).AddTicks(7355),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 8, 12, 22, 59, 47, 683, DateTimeKind.Local).AddTicks(7777));

            migrationBuilder.AlterColumn<DateTime>(
                name: "played_on",
                table: "GamePlayed",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 13, 0, 38, 22, 327, DateTimeKind.Local).AddTicks(5536),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 8, 12, 22, 59, 47, 683, DateTimeKind.Local).AddTicks(6101));

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "FifaTeam",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "logo",
                table: "FifaTeam",
                type: "TEXT",
                maxLength: 200000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "logo",
                table: "FifaTeam");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "Player",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 12, 22, 59, 47, 683, DateTimeKind.Local).AddTicks(7777),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 8, 13, 0, 38, 22, 327, DateTimeKind.Local).AddTicks(7355));

            migrationBuilder.AlterColumn<DateTime>(
                name: "played_on",
                table: "GamePlayed",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 12, 22, 59, 47, 683, DateTimeKind.Local).AddTicks(6101),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 8, 13, 0, 38, 22, 327, DateTimeKind.Local).AddTicks(5536));

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "FifaTeam",
                type: "TEXT",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "logo_url",
                table: "FifaTeam",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "short_name",
                table: "FifaTeam",
                type: "TEXT",
                maxLength: 200,
                nullable: true);
        }
    }
}
