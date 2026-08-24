using Microsoft.EntityFrameworkCore.Migrations;

namespace Internalexamportal.DataAccessLayer.Migrations
{
    public partial class Added_multiple_setting_types_in_test_settings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Setting_TestId",
                table: "Setting");

            migrationBuilder.CreateIndex(
                name: "IX_Setting_TestId",
                table: "Setting",
                column: "TestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Setting_TestId",
                table: "Setting");

            migrationBuilder.CreateIndex(
                name: "IX_Setting_TestId",
                table: "Setting",
                column: "TestId",
                unique: true);
        }
    }
}
