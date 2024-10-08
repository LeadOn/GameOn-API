﻿// <auto-generated />
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOn.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Added_FifaTeam_Type : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                 name: "type",
                 table: "FifaTeam",
                 type: "int",
                 nullable: false,
                 defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "FifaTeam");
        }
    }
}
