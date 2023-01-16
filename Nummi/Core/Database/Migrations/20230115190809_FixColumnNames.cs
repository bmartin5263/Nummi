using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nummi.Core.Database.Migrations
{
    /// <inheritdoc />
    public partial class FixColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OpportunistState",
                table: "OpportunistStrategy",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "OpportunistParameters",
                table: "OpportunistStrategy",
                newName: "Parameters");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "State",
                table: "OpportunistStrategy",
                newName: "OpportunistState");

            migrationBuilder.RenameColumn(
                name: "Parameters",
                table: "OpportunistStrategy",
                newName: "OpportunistParameters");
        }
    }
}
