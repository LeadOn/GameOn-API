using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YuFoot.EntitiesContext.Migrations
{
    /// <inheritdoc />
    public partial class Added_FifaTeam_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "Player",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 12, 22, 59, 47, 683, DateTimeKind.Local).AddTicks(7777),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 8, 6, 14, 11, 8, 753, DateTimeKind.Local).AddTicks(5219));

            migrationBuilder.AlterColumn<DateTime>(
                name: "played_on",
                table: "GamePlayed",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 12, 22, 59, 47, 683, DateTimeKind.Local).AddTicks(6101),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 8, 6, 14, 11, 8, 753, DateTimeKind.Local).AddTicks(3596));

            migrationBuilder.CreateTable(
                name: "FifaTeam",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    short_name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    code = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    logo_url = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FifaTeam", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FifaTeam");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "Player",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 6, 14, 11, 8, 753, DateTimeKind.Local).AddTicks(5219),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 8, 12, 22, 59, 47, 683, DateTimeKind.Local).AddTicks(7777));

            migrationBuilder.AlterColumn<DateTime>(
                name: "played_on",
                table: "GamePlayed",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 6, 14, 11, 8, 753, DateTimeKind.Local).AddTicks(3596),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 8, 12, 22, 59, 47, 683, DateTimeKind.Local).AddTicks(6101));
        }
    }
}
