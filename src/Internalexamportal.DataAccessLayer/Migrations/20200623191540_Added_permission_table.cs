using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Internalexamportal.DataAccessLayer.Migrations
{
    public partial class Added_permission_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ParentId = table.Column<int>(nullable: false),
                    PermissionName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Description", "IsActive", "ParentId", "PermissionName" },
                values: new object[,]
                {
                    { 1, "", true, 0, "Dashboard" },
                    { 23, "", true, 6, "View" },
                    { 22, "", true, 5, "Import" },
                    { 21, "", true, 5, "Delete" },
                    { 20, "", true, 5, "Add/Edit" },
                    { 19, "", true, 5, "View" },
                    { 18, "", true, 4, "Delete" },
                    { 17, "", true, 4, "Add/Edit" },
                    { 16, "", true, 4, "View" },
                    { 15, "", true, 3, "Delete" },
                    { 14, "", true, 3, "Add/Edit" },
                    { 24, "", true, 6, "Add/Edit" },
                    { 13, "", true, 3, "View" },
                    { 11, "", true, 2, "Delete" },
                    { 10, "", true, 2, "Add/Edit" },
                    { 9, "", true, 2, "View" },
                    { 8, "", true, 1, "Add" },
                    { 7, "", true, 1, "View" },
                    { 6, "", true, 0, "Admin Manager" },
                    { 5, "", true, 0, "Candidates Manager" },
                    { 4, "", true, 0, "Test Manager" },
                    { 3, "", true, 0, "Subject Manager" },
                    { 2, "", true, 0, "Question Manager" },
                    { 12, "", true, 2, "Import" },
                    { 25, "", true, 6, "Delete" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Permission");
        }
    }
}
