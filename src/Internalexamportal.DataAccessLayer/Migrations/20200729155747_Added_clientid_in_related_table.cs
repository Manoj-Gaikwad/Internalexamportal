using Microsoft.EntityFrameworkCore.Migrations;

namespace Internalexamportal.DataAccessLayer.Migrations
{
    public partial class Added_clientid_in_related_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "TestInstruction",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Test",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Subject",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Question",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_TestInstruction_ClientId",
                table: "TestInstruction",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Test_ClientId",
                table: "Test",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Subject_ClientId",
                table: "Subject",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_ClientId",
                table: "Question",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Client_ClientId",
                table: "Question",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subject_Client_ClientId",
                table: "Subject",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Client_ClientId",
                table: "Test",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TestInstruction_Client_ClientId",
                table: "TestInstruction",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_Client_ClientId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Subject_Client_ClientId",
                table: "Subject");

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Client_ClientId",
                table: "Test");

            migrationBuilder.DropForeignKey(
                name: "FK_TestInstruction_Client_ClientId",
                table: "TestInstruction");

            migrationBuilder.DropIndex(
                name: "IX_TestInstruction_ClientId",
                table: "TestInstruction");

            migrationBuilder.DropIndex(
                name: "IX_Test_ClientId",
                table: "Test");

            migrationBuilder.DropIndex(
                name: "IX_Subject_ClientId",
                table: "Subject");

            migrationBuilder.DropIndex(
                name: "IX_Question_ClientId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "TestInstruction");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Subject");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Question");
        }
    }
}
