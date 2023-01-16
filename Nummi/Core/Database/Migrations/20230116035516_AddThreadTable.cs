using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nummi.Core.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddThreadTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BotThread",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BotId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotThread", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BotThread_Bot_BotId",
                        column: x => x.BotId,
                        principalTable: "Bot",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BotThread_BotId",
                table: "BotThread",
                column: "BotId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BotThread");
        }
    }
}
