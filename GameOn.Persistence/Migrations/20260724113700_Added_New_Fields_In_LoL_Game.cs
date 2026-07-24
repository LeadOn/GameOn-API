using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOn.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Added_New_Fields_In_LoL_Game : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IndividualPosition",
                table: "LeagueOfLegendsGameParticipant",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TeamPosition",
                table: "LeagueOfLegendsGameParticipant",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "VisionScore",
                table: "LeagueOfLegendsGameParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LeagueOfLegendsGameParticipantChallenge",
                columns: table => new
                {
                    lol_game_participant_id = table.Column<int>(type: "int", nullable: false),
                    one_two_assist_streak_count = table.Column<int>(type: "int", nullable: false),
                    baron_buff_gold_advantage_over_threshold = table.Column<int>(type: "int", nullable: false),
                    control_ward_time_coverage_in_river_or_enemy_half = table.Column<float>(type: "real", nullable: false),
                    earliest_baron = table.Column<int>(type: "int", nullable: false),
                    earliest_dragon_takedown = table.Column<int>(type: "int", nullable: false),
                    earliest_elder_dragon = table.Column<int>(type: "int", nullable: false),
                    early_laning_phase_gold_exp_advantage = table.Column<int>(type: "int", nullable: false),
                    faster_support_quest_completion = table.Column<int>(type: "int", nullable: false),
                    fastest_legendary = table.Column<int>(type: "int", nullable: false),
                    had_afk_teammate = table.Column<int>(type: "int", nullable: false),
                    highest_champion_damage = table.Column<int>(type: "int", nullable: false),
                    highest_crowd_control_score = table.Column<int>(type: "int", nullable: false),
                    highest_ward_kills = table.Column<int>(type: "int", nullable: false),
                    jungler_kills_early_jungle = table.Column<int>(type: "int", nullable: false),
                    kills_on_laners_early_jungle_as_jungler = table.Column<int>(type: "int", nullable: false),
                    laning_phase_gold_exp_advantage = table.Column<int>(type: "int", nullable: false),
                    legendary_count = table.Column<int>(type: "int", nullable: false),
                    max_cs_advantage_on_lane_opponent = table.Column<float>(type: "real", nullable: false),
                    max_level_lead_lane_opponent = table.Column<int>(type: "int", nullable: false),
                    most_wards_destroyed_one_sweeper = table.Column<int>(type: "int", nullable: false),
                    mythic_item_used = table.Column<int>(type: "int", nullable: false),
                    played_champ_select_position = table.Column<int>(type: "int", nullable: false),
                    solo_turrets_lategame = table.Column<int>(type: "int", nullable: false),
                    takedowns_first_25_minutes = table.Column<int>(type: "int", nullable: false),
                    teleport_takedowns = table.Column<int>(type: "int", nullable: false),
                    third_inhibitor_destroyed_time = table.Column<int>(type: "int", nullable: false),
                    three_wards_one_sweeper_count = table.Column<int>(type: "int", nullable: false),
                    vision_score_advantage_lane_opponent = table.Column<float>(type: "real", nullable: false),
                    infernal_scale_pickup = table.Column<int>(type: "int", nullable: false),
                    fist_bump_participation = table.Column<int>(type: "int", nullable: false),
                    void_monster_kill = table.Column<int>(type: "int", nullable: false),
                    ability_uses = table.Column<int>(type: "int", nullable: false),
                    aces_before_15_minutes = table.Column<int>(type: "int", nullable: false),
                    allied_jungle_monster_kills = table.Column<float>(type: "real", nullable: false),
                    baron_takedowns = table.Column<int>(type: "int", nullable: false),
                    blast_cone_opposite_opponent_count = table.Column<int>(type: "int", nullable: false),
                    bounty_gold = table.Column<int>(type: "int", nullable: false),
                    buffs_stolen = table.Column<int>(type: "int", nullable: false),
                    complete_support_quest_in_time = table.Column<int>(type: "int", nullable: false),
                    control_wards_placed = table.Column<int>(type: "int", nullable: false),
                    damage_per_minute = table.Column<float>(type: "real", nullable: false),
                    damage_taken_on_team_percentage = table.Column<float>(type: "real", nullable: false),
                    danced_with_rift_herald = table.Column<int>(type: "int", nullable: false),
                    deaths_by_enemy_champs = table.Column<int>(type: "int", nullable: false),
                    dodge_skill_shots_small_window = table.Column<int>(type: "int", nullable: false),
                    double_aces = table.Column<int>(type: "int", nullable: false),
                    dragon_takedowns = table.Column<int>(type: "int", nullable: false),
                    effective_heal_and_shielding = table.Column<float>(type: "real", nullable: false),
                    elder_dragon_kills_with_opposing_soul = table.Column<int>(type: "int", nullable: false),
                    elder_dragon_multikills = table.Column<int>(type: "int", nullable: false),
                    enemy_champion_immobilizations = table.Column<int>(type: "int", nullable: false),
                    enemy_jungle_monster_kills = table.Column<float>(type: "real", nullable: false),
                    epic_monster_kills_near_enemy_jungler = table.Column<int>(type: "int", nullable: false),
                    epic_monster_kills_within_30_seconds_of_spawn = table.Column<int>(type: "int", nullable: false),
                    epic_monster_steals = table.Column<int>(type: "int", nullable: false),
                    epic_monster_stolen_without_smite = table.Column<int>(type: "int", nullable: false),
                    flawless_aces = table.Column<int>(type: "int", nullable: false),
                    full_team_takedown = table.Column<int>(type: "int", nullable: false),
                    game_length = table.Column<float>(type: "real", nullable: false),
                    gold_per_minute = table.Column<float>(type: "real", nullable: false),
                    had_open_nexus = table.Column<int>(type: "int", nullable: false),
                    immobilize_and_kill_with_ally = table.Column<int>(type: "int", nullable: false),
                    jungle_cs_before_10_minutes = table.Column<float>(type: "real", nullable: false),
                    jungler_takedowns_near_damaged_epic_monster = table.Column<int>(type: "int", nullable: false),
                    kda = table.Column<float>(type: "real", nullable: false),
                    kill_after_hidden_with_ally = table.Column<int>(type: "int", nullable: false),
                    kill_participation = table.Column<float>(type: "real", nullable: false),
                    kills_near_enemy_turret = table.Column<int>(type: "int", nullable: false),
                    kills_on_other_lanes_early_jungle_as_laner = table.Column<int>(type: "int", nullable: false),
                    kills_under_own_turret = table.Column<int>(type: "int", nullable: false),
                    kills_with_help_from_epic_monster = table.Column<int>(type: "int", nullable: false),
                    knock_enemy_into_team_and_kill = table.Column<int>(type: "int", nullable: false),
                    k_turrets_destroyed_before_plates_fall = table.Column<int>(type: "int", nullable: false),
                    land_skill_shots_early_game = table.Column<int>(type: "int", nullable: false),
                    lane_minions_first_10_minutes = table.Column<int>(type: "int", nullable: false),
                    lost_an_inhibitor = table.Column<int>(type: "int", nullable: false),
                    max_kill_deficit = table.Column<int>(type: "int", nullable: false),
                    mejais_full_stack_in_time = table.Column<int>(type: "int", nullable: false),
                    more_enemy_jungle_than_opponent = table.Column<float>(type: "real", nullable: false),
                    multi_kill_one_spell = table.Column<int>(type: "int", nullable: false),
                    multikills = table.Column<int>(type: "int", nullable: false),
                    multikills_after_aggressive_flash = table.Column<int>(type: "int", nullable: false),
                    multi_turret_rift_herald_count = table.Column<int>(type: "int", nullable: false),
                    outer_turret_executes_before_10_minutes = table.Column<int>(type: "int", nullable: false),
                    outnumbered_kills = table.Column<int>(type: "int", nullable: false),
                    outnumbered_nexus_kill = table.Column<int>(type: "int", nullable: false),
                    perfect_dragon_souls_taken = table.Column<int>(type: "int", nullable: false),
                    perfect_game = table.Column<int>(type: "int", nullable: false),
                    pick_kill_with_ally = table.Column<int>(type: "int", nullable: false),
                    poro_explosions = table.Column<int>(type: "int", nullable: false),
                    quick_cleanse = table.Column<int>(type: "int", nullable: false),
                    quick_first_turret = table.Column<int>(type: "int", nullable: false),
                    rift_herald_takedowns = table.Column<int>(type: "int", nullable: false),
                    save_ally_from_death = table.Column<int>(type: "int", nullable: false),
                    scuttle_crab_kills = table.Column<int>(type: "int", nullable: false),
                    skillshots_dodged = table.Column<int>(type: "int", nullable: false),
                    skillshots_hit = table.Column<int>(type: "int", nullable: false),
                    snowballs_hit = table.Column<int>(type: "int", nullable: false),
                    solo_baron_kills = table.Column<int>(type: "int", nullable: false),
                    solo_kills = table.Column<int>(type: "int", nullable: false),
                    stealth_wards_placed = table.Column<int>(type: "int", nullable: false),
                    survived_single_digit_hp_count = table.Column<int>(type: "int", nullable: false),
                    survived_three_immobilizes_in_fight = table.Column<int>(type: "int", nullable: false),
                    takedown_on_first_turret = table.Column<int>(type: "int", nullable: false),
                    takedowns = table.Column<int>(type: "int", nullable: false),
                    takedowns_after_gaining_level_advantage = table.Column<int>(type: "int", nullable: false),
                    takedowns_before_jungle_minion_spawn = table.Column<int>(type: "int", nullable: false),
                    takedowns_in_enemy_fountain = table.Column<int>(type: "int", nullable: false),
                    team_baron_kills = table.Column<int>(type: "int", nullable: false),
                    team_damage_percentage = table.Column<float>(type: "real", nullable: false),
                    team_elder_dragon_kills = table.Column<int>(type: "int", nullable: false),
                    team_rift_herald_kills = table.Column<int>(type: "int", nullable: false),
                    took_large_damage_survived = table.Column<int>(type: "int", nullable: false),
                    turret_plates_taken = table.Column<int>(type: "int", nullable: false),
                    turrets_taken_with_rift_herald = table.Column<int>(type: "int", nullable: false),
                    turret_takedowns = table.Column<int>(type: "int", nullable: false),
                    twenty_minions_in_3_seconds_count = table.Column<int>(type: "int", nullable: false),
                    unseen_recalls = table.Column<int>(type: "int", nullable: false),
                    vision_score_per_minute = table.Column<float>(type: "real", nullable: false),
                    wards_guarded = table.Column<int>(type: "int", nullable: false),
                    ward_takedowns = table.Column<int>(type: "int", nullable: false),
                    ward_takedowns_before_20m = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueOfLegendsGameParticipantChallenge", x => x.lol_game_participant_id);
                    table.ForeignKey(
                        name: "FK_LoL_Game_Participant_Challenge",
                        column: x => x.lol_game_participant_id,
                        principalTable: "LeagueOfLegendsGameParticipant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeagueOfLegendsGameParticipantChallenge");

            migrationBuilder.DropColumn(
                name: "IndividualPosition",
                table: "LeagueOfLegendsGameParticipant");

            migrationBuilder.DropColumn(
                name: "TeamPosition",
                table: "LeagueOfLegendsGameParticipant");

            migrationBuilder.DropColumn(
                name: "VisionScore",
                table: "LeagueOfLegendsGameParticipant");
        }
    }
}
