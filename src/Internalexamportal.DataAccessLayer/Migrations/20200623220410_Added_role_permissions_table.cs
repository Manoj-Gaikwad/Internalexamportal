using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Internalexamportal.DataAccessLayer.Migrations
{
    public partial class Added_role_permissions_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RolePermission",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: true),
                    PermissionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", "03f9632b-47fd-4ec4-a53f-39e14666d282", "SuperAdmin", "SUPERADMIN" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "859e583d-691f-40b4-adb2-828703ee1dd7", "31a59558-0a25-48f4-91ba-6a2ffc0ad41c", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "15bda09a-effe-4634-88dc-855d9642bfed", "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", "Candidate", "CANDIDATE" });

            migrationBuilder.InsertData(
                table: "RolePermission",
                columns: new[] { "Id", "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 1, 1, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 26, 1, "859e583d-691f-40b4-adb2-828703ee1dd7" },
                    { 27, 2, "859e583d-691f-40b4-adb2-828703ee1dd7" },
                    { 28, 3, "859e583d-691f-40b4-adb2-828703ee1dd7" },
                    { 29, 4, "859e583d-691f-40b4-adb2-828703ee1dd7" },
                    { 30, 5, "859e583d-691f-40b4-adb2-828703ee1dd7" },
                    { 31, 7, "859e583d-691f-40b4-adb2-828703ee1dd7" },
                    { 32, 8, "859e583d-691f-40b4-adb2-828703ee1dd7" },
                    { 33, 9, "859e583d-691f-40b4-adb2-828703ee1dd7" },
                    { 34, 10, "859e583d-691f-40b4-adb2-828703ee1dd7" },
                    { 35, 11, "859e583d-691f-40b4-adb2-828703ee1dd7" },
                    { 36, 12, "859e583d-691f-40b4-adb2-828703ee1dd7" },
                    { 37, 13, "859e583d-691f-40b4-adb2-828703ee1dd7" },
                    { 38, 14, "859e583d-691f-40b4-adb2-828703ee1dd7" },
                    { 39, 15, "859e583d-691f-40b4-adb2-828703ee1dd7" },
                    { 40, 16, "859e583d-691f-40b4-adb2-828703ee1dd7" },
                    { 41, 17, "859e583d-691f-40b4-adb2-828703ee1dd7" },
                    { 42, 18, "859e583d-691f-40b4-adb2-828703ee1dd7" },
                    { 43, 19, "859e583d-691f-40b4-adb2-828703ee1dd7" },
                    { 44, 20, "859e583d-691f-40b4-adb2-828703ee1dd7" },
                    { 25, 25, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 24, 24, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 23, 23, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 22, 22, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 2, 2, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 3, 3, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 4, 4, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 5, 5, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 6, 6, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 7, 7, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 8, 8, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 9, 9, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 10, 10, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 45, 21, "859e583d-691f-40b4-adb2-828703ee1dd7" },
                    { 11, 11, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 13, 13, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 14, 14, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 15, 15, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 16, 16, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 17, 17, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 18, 18, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 19, 19, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 20, 20, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 21, 21, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 12, 12, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 46, 22, "859e583d-691f-40b4-adb2-828703ee1dd7" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                table: "RolePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RoleId",
                table: "RolePermission",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolePermission");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "15bda09a-effe-4634-88dc-855d9642bfed", "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" });

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "859e583d-691f-40b4-adb2-828703ee1dd7", "31a59558-0a25-48f4-91ba-6a2ffc0ad41c" });

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", "03f9632b-47fd-4ec4-a53f-39e14666d282" });
        }
    }
}
