using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Internalexamportal.DataAccessLayer.Migrations
{
    public partial class Added_test_type_and_percentage_in_test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Test_QuestionType_QuestionTypeId",
                table: "Test");

            migrationBuilder.RenameColumn(
                name: "QuestionTypeId",
                table: "Test",
                newName: "TestTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Test_QuestionTypeId",
                table: "Test",
                newName: "IX_Test_TestTypeId");

            migrationBuilder.AddColumn<float>(
                name: "Percentage",
                table: "Test",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TestType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestType", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TestType",
                columns: new[] { "Id", "Type" },
                values: new object[] { 1, "Objective" });

            migrationBuilder.InsertData(
                table: "TestType",
                columns: new[] { "Id", "Type" },
                values: new object[] { 2, "Subjective" });

            migrationBuilder.InsertData(
                table: "TestType",
                columns: new[] { "Id", "Type" },
                values: new object[] { 3, "Objective & Subjective" });

            migrationBuilder.AddForeignKey(
                name: "FK_Test_TestType_TestTypeId",
                table: "Test",
                column: "TestTypeId",
                principalTable: "TestType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Test_TestType_TestTypeId",
                table: "Test");

            migrationBuilder.DropTable(
                name: "TestType");

            migrationBuilder.DropColumn(
                name: "Percentage",
                table: "Test");

            migrationBuilder.RenameColumn(
                name: "TestTypeId",
                table: "Test",
                newName: "QuestionTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Test_TestTypeId",
                table: "Test",
                newName: "IX_Test_QuestionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Test_QuestionType_QuestionTypeId",
                table: "Test",
                column: "QuestionTypeId",
                principalTable: "QuestionType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
