using Microsoft.EntityFrameworkCore.Migrations;

namespace Internalexamportal.DataAccessLayer.Migrations
{
    public partial class Added_submittedTestId_in_submittedOption_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmittedOption_Test_TestId",
                table: "SubmittedOption");

            migrationBuilder.DropForeignKey(
                name: "FK_SubmittedOption_User_UserId",
                table: "SubmittedOption");

            migrationBuilder.DropIndex(
                name: "IX_SubmittedOption_UserId",
                table: "SubmittedOption");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SubmittedOption");

            migrationBuilder.RenameColumn(
                name: "TestId",
                table: "SubmittedOption",
                newName: "SubmittedTestId");

            migrationBuilder.RenameIndex(
                name: "IX_SubmittedOption_TestId",
                table: "SubmittedOption",
                newName: "IX_SubmittedOption_SubmittedTestId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmittedOption_SubmittedTest_SubmittedTestId",
                table: "SubmittedOption",
                column: "SubmittedTestId",
                principalTable: "SubmittedTest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmittedOption_SubmittedTest_SubmittedTestId",
                table: "SubmittedOption");

            migrationBuilder.RenameColumn(
                name: "SubmittedTestId",
                table: "SubmittedOption",
                newName: "TestId");

            migrationBuilder.RenameIndex(
                name: "IX_SubmittedOption_SubmittedTestId",
                table: "SubmittedOption",
                newName: "IX_SubmittedOption_TestId");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "SubmittedOption",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubmittedOption_UserId",
                table: "SubmittedOption",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmittedOption_Test_TestId",
                table: "SubmittedOption",
                column: "TestId",
                principalTable: "Test",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubmittedOption_User_UserId",
                table: "SubmittedOption",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
