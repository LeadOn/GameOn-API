﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOn.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Added_Basic_LoLGameTimelineFrameParticipant_and_fixed_dates_issues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeagueOfLegendsGameTimelineFrameParticipant",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    timeline_frame_id = table.Column<int>(type: "int", nullable: false),
                    ParticipantId = table.Column<int>(type: "int", nullable: false),
                    level = table.Column<int>(type: "int", nullable: false),
                    minions_killed = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueOfLegendsGameTimelineFrameParticipant", x => x.id);
                    table.ForeignKey(
                        name: "FK_LoL_Game_Frame_Participant",
                        column: x => x.timeline_frame_id,
                        principalTable: "LeagueOfLegendsGameTimelineFrame",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_LeagueOfLegendsGameTimelineFrameParticipant_timeline_frame_id",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                column: "timeline_frame_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeagueOfLegendsGameTimelineFrameParticipant");
        }
    }
}
