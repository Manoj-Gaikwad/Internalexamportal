using Microsoft.EntityFrameworkCore.Migrations;

namespace Internalexamportal.DataAccessLayer.Migrations
{
    public partial class Added_clientId_in_group_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Group",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Group_ClientId",
                table: "Group",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Group_Client_ClientId",
                table: "Group",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Group_Client_ClientId",
                table: "Group");

            migrationBuilder.DropIndex(
                name: "IX_Group_ClientId",
                table: "Group");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Group");
        }
    }
}
