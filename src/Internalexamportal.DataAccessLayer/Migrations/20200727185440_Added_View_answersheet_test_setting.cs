using Microsoft.EntityFrameworkCore.Migrations;

namespace Internalexamportal.DataAccessLayer.Migrations
{
    public partial class Added_View_answersheet_test_setting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TestSettingType",
                columns: new[] { "Id", "Type" },
                values: new object[] { 5, "Display Answer Sheet" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TestSettingType",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
