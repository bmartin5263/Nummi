using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nummi.Core.Database.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCandlestickKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HistoricalMinuteCandlestick",
                table: "HistoricalMinuteCandlestick");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HistoricalMinuteCandlestick",
                table: "HistoricalMinuteCandlestick",
                columns: new[] { "Symbol", "OpenTimeEpoch" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HistoricalMinuteCandlestick",
                table: "HistoricalMinuteCandlestick");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HistoricalMinuteCandlestick",
                table: "HistoricalMinuteCandlestick",
                columns: new[] { "Symbol", "OpenTime" });
        }
    }
}
