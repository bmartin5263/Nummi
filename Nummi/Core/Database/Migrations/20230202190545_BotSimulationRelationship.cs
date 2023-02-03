using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nummi.Core.Database.Migrations
{
    /// <inheritdoc />
    public partial class BotSimulationRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BotId",
                table: "Simulation",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StrategyId",
                table: "Simulation",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Simulation_BotId",
                table: "Simulation",
                column: "BotId");

            migrationBuilder.CreateIndex(
                name: "IX_Simulation_StrategyId",
                table: "Simulation",
                column: "StrategyId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Simulation_Bot_BotId",
                table: "Simulation",
                column: "BotId",
                principalTable: "Bot",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Simulation_Strategy_StrategyId",
                table: "Simulation",
                column: "StrategyId",
                principalTable: "Strategy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Simulation_Bot_BotId",
                table: "Simulation");

            migrationBuilder.DropForeignKey(
                name: "FK_Simulation_Strategy_StrategyId",
                table: "Simulation");

            migrationBuilder.DropIndex(
                name: "IX_Simulation_BotId",
                table: "Simulation");

            migrationBuilder.DropIndex(
                name: "IX_Simulation_StrategyId",
                table: "Simulation");

            migrationBuilder.DropColumn(
                name: "BotId",
                table: "Simulation");

            migrationBuilder.DropColumn(
                name: "StrategyId",
                table: "Simulation");
        }
    }
}
