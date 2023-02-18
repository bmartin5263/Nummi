#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Nummi.Core.Database.Migrations.EFCore
{
    /// <inheritdoc />
    public partial class UpdateUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StrategyTemplates",
                keyColumn: "StrategyTemplateId",
                keyValue: "2Li4VJDZCZCC9hhUD2bXfs3JVUR");

            migrationBuilder.DeleteData(
                table: "StrategyTemplates",
                keyColumn: "StrategyTemplateId",
                keyValue: "2Li4VLZmsmmvSy9KFBGKf1Gmzlm");

            migrationBuilder.RenameColumn(
                name: "AlpacaSecret",
                table: "Users",
                newName: "AlpacaPaperUserId");

            migrationBuilder.RenameColumn(
                name: "AlpacaKeyId",
                table: "Users",
                newName: "AlpacaPaperSecret");

            migrationBuilder.AddColumn<string>(
                name: "AlpacaLiveSecret",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AlpacaLiveUserId",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "StrategyTemplates",
                columns: new[] { "StrategyTemplateId", "CreatedAt", "DeletedAt", "Frequency", "Name", "ParameterTypeName", "StateTypeName", "StrategyTemplateType", "StrategyTypeName", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { "2LkhPpGTOc3e6HC4ruEomsRylH9", new DateTimeOffset(new DateTime(2023, 2, 14, 18, 29, 26, 136, DateTimeKind.Unspecified).AddTicks(2030), new TimeSpan(0, -6, 0, 0, 0)), null, new TimeSpan(0, 0, 1, 0, 0), "Opportunist2", "Nummi.Core.Domain.New.OpportunistParameters", "Nummi.Core.Domain.New.OpportunistState", "csharp", "Nummi.Core.Domain.New.OpportunistStrategy", null, null },
                    { "2LkhPqsBZQPjf0DicoxSgA6MekN", new DateTimeOffset(new DateTime(2023, 2, 15, 0, 29, 26, 136, DateTimeKind.Unspecified).AddTicks(2000), new TimeSpan(0, 0, 0, 0, 0)), null, new TimeSpan(0, 0, 1, 0, 0), "Opportunist", "Nummi.Core.Domain.New.OpportunistParameters", "Nummi.Core.Domain.New.OpportunistState", "csharp", "Nummi.Core.Domain.New.OpportunistStrategy", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StrategyTemplates",
                keyColumn: "StrategyTemplateId",
                keyValue: "2LkhPpGTOc3e6HC4ruEomsRylH9");

            migrationBuilder.DeleteData(
                table: "StrategyTemplates",
                keyColumn: "StrategyTemplateId",
                keyValue: "2LkhPqsBZQPjf0DicoxSgA6MekN");

            migrationBuilder.DropColumn(
                name: "AlpacaLiveSecret",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AlpacaLiveUserId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "AlpacaPaperUserId",
                table: "Users",
                newName: "AlpacaSecret");

            migrationBuilder.RenameColumn(
                name: "AlpacaPaperSecret",
                table: "Users",
                newName: "AlpacaKeyId");

            migrationBuilder.InsertData(
                table: "StrategyTemplates",
                columns: new[] { "StrategyTemplateId", "CreatedAt", "DeletedAt", "Frequency", "Name", "ParameterTypeName", "StateTypeName", "StrategyTemplateType", "StrategyTypeName", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { "2Li4VJDZCZCC9hhUD2bXfs3JVUR", new DateTimeOffset(new DateTime(2023, 2, 13, 20, 9, 51, 29, DateTimeKind.Unspecified).AddTicks(5200), new TimeSpan(0, -6, 0, 0, 0)), null, new TimeSpan(0, 0, 1, 0, 0), "Opportunist2", "Nummi.Core.Domain.New.OpportunistParameters", "Nummi.Core.Domain.New.OpportunistState", "csharp", "Nummi.Core.Domain.New.OpportunistStrategy", null, null },
                    { "2Li4VLZmsmmvSy9KFBGKf1Gmzlm", new DateTimeOffset(new DateTime(2023, 2, 14, 2, 9, 51, 29, DateTimeKind.Unspecified).AddTicks(5170), new TimeSpan(0, 0, 0, 0, 0)), null, new TimeSpan(0, 0, 1, 0, 0), "Opportunist", "Nummi.Core.Domain.New.OpportunistParameters", "Nummi.Core.Domain.New.OpportunistState", "csharp", "Nummi.Core.Domain.New.OpportunistStrategy", null, null }
                });
        }
    }
}
