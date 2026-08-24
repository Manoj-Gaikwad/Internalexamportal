using Microsoft.EntityFrameworkCore.Migrations;

namespace Internalexamportal.DataAccessLayer.Migrations
{
    public partial class Added_time_taken_column_in_submitted_answer_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TimeTaken",
                table: "SubmittedOption",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TimeTaken",
                table: "SubjectiveAnswer",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "TestStatus",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: "In Progress");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeTaken",
                table: "SubmittedOption");

            migrationBuilder.DropColumn(
                name: "TimeTaken",
                table: "SubjectiveAnswer");

            migrationBuilder.UpdateData(
                table: "TestStatus",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: "Started");
        }
    }
}
