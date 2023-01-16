using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nummi.Core.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIdGeneration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<uint>(
                name: "Id",
                table: "BotThread",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<uint>(
                name: "Id",
                table: "BotThread",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);
        }
    }
}
