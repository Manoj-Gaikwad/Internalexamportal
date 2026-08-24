using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Internalexamportal.DataAccessLayer.Migrations
{
    public partial class seederentry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CorrectOption",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Option = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorrectOption", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DifficultLevel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Level = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DifficultLevel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionType", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CorrectOption",
                columns: new[] { "Id", "Option" },
                values: new object[,]
                {
                    { 1, "A" },
                    { 2, "B" },
                    { 3, "C" },
                    { 4, "D" },
                    { 5, "true" },
                    { 6, "false" },
                    { 7, "subjective" }
                });

            migrationBuilder.InsertData(
                table: "DifficultLevel",
                columns: new[] { "Id", "Level" },
                values: new object[,]
                {
                    { 1, "Difficult" },
                    { 2, "Easy" },
                    { 3, "Normal" }
                });

            migrationBuilder.InsertData(
                table: "QuestionType",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "Multiple Choice Question" },
                    { 2, "True/False" },
                    { 3, "Subjective" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CorrectOption");

            migrationBuilder.DropTable(
                name: "DifficultLevel");

            migrationBuilder.DropTable(
                name: "QuestionType");
        }
    }
}
