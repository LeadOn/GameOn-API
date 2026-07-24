using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOn.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Field_Types : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "third_inhibitor_destroyed_time",
                table: "LeagueOfLegendsGameParticipantChallenge",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "fastest_legendary",
                table: "LeagueOfLegendsGameParticipantChallenge",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "earliest_elder_dragon",
                table: "LeagueOfLegendsGameParticipantChallenge",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "earliest_dragon_takedown",
                table: "LeagueOfLegendsGameParticipantChallenge",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "earliest_baron",
                table: "LeagueOfLegendsGameParticipantChallenge",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "bounty_gold",
                table: "LeagueOfLegendsGameParticipantChallenge",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "third_inhibitor_destroyed_time",
                table: "LeagueOfLegendsGameParticipantChallenge",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<int>(
                name: "fastest_legendary",
                table: "LeagueOfLegendsGameParticipantChallenge",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<int>(
                name: "earliest_elder_dragon",
                table: "LeagueOfLegendsGameParticipantChallenge",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<int>(
                name: "earliest_dragon_takedown",
                table: "LeagueOfLegendsGameParticipantChallenge",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<int>(
                name: "earliest_baron",
                table: "LeagueOfLegendsGameParticipantChallenge",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<int>(
                name: "bounty_gold",
                table: "LeagueOfLegendsGameParticipantChallenge",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }
    }
}
