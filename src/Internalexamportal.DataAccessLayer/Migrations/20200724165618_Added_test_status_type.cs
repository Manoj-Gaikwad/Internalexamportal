using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Internalexamportal.DataAccessLayer.Migrations
{
    public partial class Added_test_status_type : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestStatus", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TestStatus",
                columns: new[] { "Id", "Status" },
                values: new object[] { 1, "Started" });

            migrationBuilder.InsertData(
                table: "TestStatus",
                columns: new[] { "Id", "Status" },
                values: new object[] { 2, "Submitted" });

            migrationBuilder.InsertData(
                table: "TestStatus",
                columns: new[] { "Id", "Status" },
                values: new object[] { 3, "Evaluated" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestStatus");
        }
    }
}
