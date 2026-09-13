using Microsoft.EntityFrameworkCore.Migrations;

namespace Internalexamportal.DataAccessLayer.Migrations
{
    public partial class AddSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Client (Admin) - use a new ID to avoid conflict with existing data
            migrationBuilder.InsertData(
                "Client", "Id", new[] { 1000 },
                new[] { "Admin" });

            // Role (SuperAdmin)
            migrationBuilder.InsertData(
                "Role", "Id", new[] { 1001 },
                new[] { "SuperAdmin" });

            // Role (Regular)
            migrationBuilder.InsertData(
                "Role", "Id", new[] { 1002 },
                new[] { "Regular" });

            // Permission (Read)
            migrationBuilder.InsertData(
                "Permission", "Id", new[] { 1003 },
                new[] { "Read" });

            // Permission (Write)
            migrationBuilder.InsertData(
                "Permission", "Id", new[] { 1004 },
                new[] { "Write" });

            // RolePermission (SuperAdmin -> Read)
            migrationBuilder.InsertData(
                "RolePermission", "Id", new[] { 1001 },
                new[] { 1, 1 }); // PrincipalId=1, RoleId=1, PermissionId=1

            // RolePermission (SuperAdmin -> Write)
            migrationBuilder.InsertData(
                "RolePermission", "Id", new[] { 1001 },
                new[] { 1, 2 }); // PrincipalId=1, RoleId=1, PermissionId=2

            // TestStatus
            migrationBuilder.InsertData(
                "TestStatus", "Id", new[] { 1005 },
                new[] { "Pass" });

            // Question
            migrationBuilder.InsertData(
                "Question", "Id", new[] { 1006 },
                new[] { "Sample Question" });

            // TestInstruction
            migrationBuilder.InsertData(
                "TestInstruction", "Id", new[] { 1007 },
                new[] { "Sample Instruction" });

            // SubmittedTest
            migrationBuilder.InsertData(
                "SubmittedTest", "Id", new[] { 1008 },
                new[] { "Submitted Test" });
        }
    }
}