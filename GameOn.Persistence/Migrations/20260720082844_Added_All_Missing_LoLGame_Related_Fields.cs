using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOn.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Added_All_Missing_LoLGame_Related_Fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ability_haste",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ability_power",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "armor",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "armor_pen",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "armor_pen_percent",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "attack_damage",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "attack_speed",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "bonus_armor_pen_percent",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "bonus_magic_pen_percent",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "cc_reduction",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "cooldown_reduction",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "health",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "health_max",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "health_regen",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "lifesteal",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "magic_pen",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "magic_pen_percent",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "magic_resist",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "movement_speed",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "omnivamp",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "physical_vamp",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "position_x",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "position_y",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "power",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "power_max",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "power_regen",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "spell_vamp",
                table: "LeagueOfLegendsGameTimelineFrameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "frame_interval",
                table: "LeagueOfLegendsGame",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LeagueOfLegendsGameTimelineEvent",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    timeline_frame_id = table.Column<int>(type: "int", nullable: false),
                    timestamp = table.Column<int>(type: "int", nullable: false),
                    real_timestamp = table.Column<long>(type: "bigint", nullable: true),
                    event_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    participant_id = table.Column<int>(type: "int", nullable: true),
                    killer_id = table.Column<int>(type: "int", nullable: true),
                    victim_id = table.Column<int>(type: "int", nullable: true),
                    killer_team_id = table.Column<int>(type: "int", nullable: true),
                    team_id = table.Column<int>(type: "int", nullable: true),
                    bounty = table.Column<int>(type: "int", nullable: true),
                    shutdown_bounty = table.Column<int>(type: "int", nullable: true),
                    kill_streak_length = table.Column<int>(type: "int", nullable: true),
                    multi_kill_length = table.Column<int>(type: "int", nullable: true),
                    kill_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    item_id = table.Column<int>(type: "int", nullable: true),
                    before_id = table.Column<int>(type: "int", nullable: true),
                    after_id = table.Column<int>(type: "int", nullable: true),
                    gold_gain = table.Column<int>(type: "int", nullable: true),
                    skill_slot = table.Column<int>(type: "int", nullable: true),
                    level_up_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    level = table.Column<int>(type: "int", nullable: true),
                    ward_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    creator_id = table.Column<int>(type: "int", nullable: true),
                    building_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    tower_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    lane_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    monster_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    monster_sub_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    transform_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    dragon_soul_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    position_x = table.Column<int>(type: "int", nullable: true),
                    position_y = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueOfLegendsGameTimelineEvent", x => x.id);
                    table.ForeignKey(
                        name: "FK_LoL_Game_Frame_Event",
                        column: x => x.timeline_frame_id,
                        principalTable: "LeagueOfLegendsGameTimelineFrame",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeagueOfLegendsGameTimelineEventAssist",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    timeline_event_id = table.Column<int>(type: "int", nullable: false),
                    participant_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueOfLegendsGameTimelineEventAssist", x => x.id);
                    table.ForeignKey(
                        name: "FK_LoL_Game_Event_Assist",
                        column: x => x.timeline_event_id,
                        principalTable: "LeagueOfLegendsGameTimelineEvent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeagueOfLegendsGameTimelineEvent_timeline_frame_id",
                table: "LeagueOfLegendsGameTimelineEvent",
                column: "timeline_frame_id");

            migrationBuilder.CreateIndex(
                name: "IX_LeagueOfLegendsGameTimelineEventAssist_timeline_event_id",
                table: "LeagueOfLegendsGameTimelineEventAssist",
                column: "timeline_event_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeagueOfLegendsGameTimelineEventAssist");

            migrationBuilder.DropTable(
                name: "LeagueOfLegendsGameTimelineEvent");

            migrationBuilder.DropColumn(
                name: "ability_haste",
                table: "LeagueOfLegendsGameTimelineFrameParticipant");

            migrationBuilder.DropColumn(
                name: "ability_power",
                table: "LeagueOfLegendsGameTimelineFrameParticipant");

            migrationBuilder.DropColumn(
                name: "armor",
                table: "LeagueOfLegendsGameTimelineFrameParticipant");

            migrationBuilder.DropColumn(
                name: "armor_pen",
                table: "LeagueOfLegendsGameTimelineFrameParticipant");

            migrationBuilder.DropColumn(
                name: "armor_pen_percent",
                table: "LeagueOfLegendsGameTimelineFrameParticipant");

            migrationBuilder.DropColumn(
                name: "attack_damage",
                table: "LeagueOfLegendsGameTimelineFrameParticipant");

            migrationBuilder.DropColumn(
                name: "attack_speed",
                table: "LeagueOfLegendsGameTimelineFrameParticipant");

            migrationBuilder.DropColumn(
                name: "bonus_armor_pen_percent",
                table: "LeagueOfLegendsGameTimelineFrameParticipant");

            migrationBuilder.DropColumn(
                name: "bonus_magic_pen_percent",
                table: "LeagueOfLegendsGameTimelineFrameParticipant");

            migrationBuilder.DropColumn(
                name: "cc_reduction",
                table: "LeagueOfLegendsGameTimelineFrameParticipant");

            migrationBuilder.DropColumn(
                name: "cooldown_reduction",
                table: "LeagueOfLegendsGameTimelineFrameParticipant");

            migrationBuilder.DropColumn(
                name: "health",
                table: "LeagueOfLegendsGameTimelineFrameParticipant");

            migrationBuilder.DropColumn(
                name: "health_max",
                table: "LeagueOfLegendsGameTimelineFrameParticipant");

            migrationBuilder.DropColumn(
                name: "health_regen",
                table: "LeagueOfLegendsGameTimelineFrameParticipant");

            migrationBuilder.DropColumn(
                name: "lifesteal",
                table: "LeagueOfLegendsGameTimelineFrameParticipant");

            migrationBuilder.DropColumn(
                name: "magic_pen",
                table: "LeagueOfLegendsGameTimelineFrameParticipant");

            migrationBuilder.DropColumn(
                name: "magic_pen_percent",
                table: "LeagueOfLegendsGameTimelineFrameParticipant");

            migrationBuilder.DropColumn(
                name: "magic_resist",
                table: "LeagueOfLegendsGameTimelineFrameParticipant");

            migrationBuilder.DropColumn(
                name: "movement_speed",
                table: "LeagueOfLegendsGameTimelineFrameParticipant");

            migrationBuilder.DropColumn(
                name: "omnivamp",
                table: "LeagueOfLegendsGameTimelineFrameParticipant");

            migrationBuilder.DropColumn(
                name: "physical_vamp",
                table: "LeagueOfLegendsGameTimelineFrameParticipant");

            migrationBuilder.DropColumn(
                name: "position_x",
                table: "LeagueOfLegendsGameTimelineFrameParticipant");

            migrationBuilder.DropColumn(
                name: "position_y",
                table: "LeagueOfLegendsGameTimelineFrameParticipant");

            migrationBuilder.DropColumn(
                name: "power",
                table: "LeagueOfLegendsGameTimelineFrameParticipant");

            migrationBuilder.DropColumn(
                name: "power_max",
                table: "LeagueOfLegendsGameTimelineFrameParticipant");

            migrationBuilder.DropColumn(
                name: "power_regen",
                table: "LeagueOfLegendsGameTimelineFrameParticipant");

            migrationBuilder.DropColumn(
                name: "spell_vamp",
                table: "LeagueOfLegendsGameTimelineFrameParticipant");

            migrationBuilder.DropColumn(
                name: "frame_interval",
                table: "LeagueOfLegendsGame");
        }
    }
}
