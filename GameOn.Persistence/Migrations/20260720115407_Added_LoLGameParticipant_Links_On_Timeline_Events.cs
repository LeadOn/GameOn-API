using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOn.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Added_LoLGameParticipant_Links_On_Timeline_Events : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LeagueOfLegendsGameParticipant_MatchId",
                table: "LeagueOfLegendsGameParticipant");

            migrationBuilder.AddColumn<string>(
                name: "match_id",
                table: "LeagueOfLegendsGameTimelineEventAssist",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "match_id",
                table: "LeagueOfLegendsGameTimelineEvent",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_LoLGameParticipant_MatchId_Puuid",
                table: "LeagueOfLegendsGameParticipant",
                columns: new[] { "MatchId", "puuid" });

            migrationBuilder.CreateIndex(
                name: "IX_LeagueOfLegendsGameTimelineEventAssist_match_id_participant_puuid",
                table: "LeagueOfLegendsGameTimelineEventAssist",
                columns: new[] { "match_id", "participant_puuid" });

            migrationBuilder.CreateIndex(
                name: "IX_LeagueOfLegendsGameTimelineEvent_match_id_creator_puuid",
                table: "LeagueOfLegendsGameTimelineEvent",
                columns: new[] { "match_id", "creator_puuid" });

            migrationBuilder.CreateIndex(
                name: "IX_LeagueOfLegendsGameTimelineEvent_match_id_killer_puuid",
                table: "LeagueOfLegendsGameTimelineEvent",
                columns: new[] { "match_id", "killer_puuid" });

            migrationBuilder.CreateIndex(
                name: "IX_LeagueOfLegendsGameTimelineEvent_match_id_participant_puuid",
                table: "LeagueOfLegendsGameTimelineEvent",
                columns: new[] { "match_id", "participant_puuid" });

            migrationBuilder.CreateIndex(
                name: "IX_LeagueOfLegendsGameTimelineEvent_match_id_victim_puuid",
                table: "LeagueOfLegendsGameTimelineEvent",
                columns: new[] { "match_id", "victim_puuid" });

            migrationBuilder.AddForeignKey(
                name: "FK_LoL_Event_Creator",
                table: "LeagueOfLegendsGameTimelineEvent",
                columns: new[] { "match_id", "creator_puuid" },
                principalTable: "LeagueOfLegendsGameParticipant",
                principalColumns: new[] { "MatchId", "puuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoL_Event_Killer",
                table: "LeagueOfLegendsGameTimelineEvent",
                columns: new[] { "match_id", "killer_puuid" },
                principalTable: "LeagueOfLegendsGameParticipant",
                principalColumns: new[] { "MatchId", "puuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoL_Event_Participant",
                table: "LeagueOfLegendsGameTimelineEvent",
                columns: new[] { "match_id", "participant_puuid" },
                principalTable: "LeagueOfLegendsGameParticipant",
                principalColumns: new[] { "MatchId", "puuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoL_Event_Victim",
                table: "LeagueOfLegendsGameTimelineEvent",
                columns: new[] { "match_id", "victim_puuid" },
                principalTable: "LeagueOfLegendsGameParticipant",
                principalColumns: new[] { "MatchId", "puuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoL_EventAssist_Participant",
                table: "LeagueOfLegendsGameTimelineEventAssist",
                columns: new[] { "match_id", "participant_puuid" },
                principalTable: "LeagueOfLegendsGameParticipant",
                principalColumns: new[] { "MatchId", "puuid" },
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoL_Event_Creator",
                table: "LeagueOfLegendsGameTimelineEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_LoL_Event_Killer",
                table: "LeagueOfLegendsGameTimelineEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_LoL_Event_Participant",
                table: "LeagueOfLegendsGameTimelineEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_LoL_Event_Victim",
                table: "LeagueOfLegendsGameTimelineEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_LoL_EventAssist_Participant",
                table: "LeagueOfLegendsGameTimelineEventAssist");

            migrationBuilder.DropIndex(
                name: "IX_LeagueOfLegendsGameTimelineEventAssist_match_id_participant_puuid",
                table: "LeagueOfLegendsGameTimelineEventAssist");

            migrationBuilder.DropIndex(
                name: "IX_LeagueOfLegendsGameTimelineEvent_match_id_creator_puuid",
                table: "LeagueOfLegendsGameTimelineEvent");

            migrationBuilder.DropIndex(
                name: "IX_LeagueOfLegendsGameTimelineEvent_match_id_killer_puuid",
                table: "LeagueOfLegendsGameTimelineEvent");

            migrationBuilder.DropIndex(
                name: "IX_LeagueOfLegendsGameTimelineEvent_match_id_participant_puuid",
                table: "LeagueOfLegendsGameTimelineEvent");

            migrationBuilder.DropIndex(
                name: "IX_LeagueOfLegendsGameTimelineEvent_match_id_victim_puuid",
                table: "LeagueOfLegendsGameTimelineEvent");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_LoLGameParticipant_MatchId_Puuid",
                table: "LeagueOfLegendsGameParticipant");

            migrationBuilder.DropColumn(
                name: "match_id",
                table: "LeagueOfLegendsGameTimelineEventAssist");

            migrationBuilder.DropColumn(
                name: "match_id",
                table: "LeagueOfLegendsGameTimelineEvent");

            migrationBuilder.CreateIndex(
                name: "IX_LeagueOfLegendsGameParticipant_MatchId",
                table: "LeagueOfLegendsGameParticipant",
                column: "MatchId");
        }
    }
}
