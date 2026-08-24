using Microsoft.EntityFrameworkCore.Migrations;

namespace Internalexamportal.DataAccessLayer.Migrations
{
    public partial class Added_test_status_fk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "SubmittedTest",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_SubmittedTest_StatusId",
                table: "SubmittedTest",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmittedTest_TestStatus_StatusId",
                table: "SubmittedTest",
                column: "StatusId",
                principalTable: "TestStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmittedTest_TestStatus_StatusId",
                table: "SubmittedTest");

            migrationBuilder.DropIndex(
                name: "IX_SubmittedTest_StatusId",
                table: "SubmittedTest");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "SubmittedTest");
        }
    }
}
