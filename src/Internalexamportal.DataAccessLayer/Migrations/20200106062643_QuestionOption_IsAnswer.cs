using Microsoft.EntityFrameworkCore.Migrations;

namespace Internalexamportal.DataAccessLayer.Migrations
{
    public partial class QuestionOption_IsAnswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAnswer",
                table: "QuestionOption",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAnswer",
                table: "QuestionOption");
        }
    }
}
