using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nummi.Core.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddApiCallsToStrategyLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApiCalls",
                table: "StrategyLog",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TotalApiCallTime",
                table: "StrategyLog",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApiCalls",
                table: "StrategyLog");

            migrationBuilder.DropColumn(
                name: "TotalApiCallTime",
                table: "StrategyLog");
        }
    }
}
