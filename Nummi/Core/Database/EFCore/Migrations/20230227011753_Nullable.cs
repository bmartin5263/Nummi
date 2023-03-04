using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nummi.Core.Database.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class Nullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Strategy_BotActivation_BotActivationId",
                table: "Strategy");

            migrationBuilder.DropForeignKey(
                name: "FK_Strategy_Simulation_SimulationId",
                table: "Strategy");

            migrationBuilder.AlterColumn<Guid>(
                name: "SimulationId",
                table: "Strategy",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "BotActivationId",
                table: "Strategy",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Strategy_BotActivation_BotActivationId",
                table: "Strategy",
                column: "BotActivationId",
                principalTable: "BotActivation",
                principalColumn: "BotActivationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Strategy_Simulation_SimulationId",
                table: "Strategy",
                column: "SimulationId",
                principalTable: "Simulation",
                principalColumn: "SimulationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Strategy_BotActivation_BotActivationId",
                table: "Strategy");

            migrationBuilder.DropForeignKey(
                name: "FK_Strategy_Simulation_SimulationId",
                table: "Strategy");

            migrationBuilder.AlterColumn<Guid>(
                name: "SimulationId",
                table: "Strategy",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "BotActivationId",
                table: "Strategy",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Strategy_BotActivation_BotActivationId",
                table: "Strategy",
                column: "BotActivationId",
                principalTable: "BotActivation",
                principalColumn: "BotActivationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Strategy_Simulation_SimulationId",
                table: "Strategy",
                column: "SimulationId",
                principalTable: "Simulation",
                principalColumn: "SimulationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
