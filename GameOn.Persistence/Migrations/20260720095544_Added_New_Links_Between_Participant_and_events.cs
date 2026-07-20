using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOn.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Added_New_Links_Between_Participant_and_events : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "participant_puuid",
                table: "LeagueOfLegendsGameTimelineEventAssist",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "creator_puuid",
                table: "LeagueOfLegendsGameTimelineEvent",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "killer_puuid",
                table: "LeagueOfLegendsGameTimelineEvent",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "participant_puuid",
                table: "LeagueOfLegendsGameTimelineEvent",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "victim_puuid",
                table: "LeagueOfLegendsGameTimelineEvent",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "participant_puuid",
                table: "LeagueOfLegendsGameTimelineEventAssist");

            migrationBuilder.DropColumn(
                name: "creator_puuid",
                table: "LeagueOfLegendsGameTimelineEvent");

            migrationBuilder.DropColumn(
                name: "killer_puuid",
                table: "LeagueOfLegendsGameTimelineEvent");

            migrationBuilder.DropColumn(
                name: "participant_puuid",
                table: "LeagueOfLegendsGameTimelineEvent");

            migrationBuilder.DropColumn(
                name: "victim_puuid",
                table: "LeagueOfLegendsGameTimelineEvent");
        }
    }
}
