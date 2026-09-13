using Microsoft.EntityFrameworkCore.Migrations;

namespace Internalexamportal.DataAccessLayer.Migrations
{
    public partial class AddSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Client (Admin) - use a new ID to avoid conflict with existing data
            migrationBuilder.InsertData(
                "Client", "Id", "integer", new int[] { 1000 },
                new[] { "Admin" });

            // Role (SuperAdmin)
            migrationBuilder.InsertData(
                "Role", "Id", "integer", new int[] { 1001 },
                new[] { "SuperAdmin" });

            // Role (Regular)
            migrationBuilder.InsertData(
                "Role", "Id", "integer", new int[] { 1002 },
                new[] { "Regular" });

            // Permission (Read)
            migrationBuilder.InsertData(
                "Permission", "Id", "string", new string[] { "Read" });

            // Permission (Write)
            migrationBuilder.InsertData(
                "Permission", "Id", "string", new string[] { "Write" });

            // RolePermission (SuperAdmin -> Read)
            migrationBuilder.InsertData(
                "RolePermission", "Id", "integer", new int[] { 1001 },
                new[] { 1, 1 }); // PrincipalId=1, RoleId=1, PermissionId=1

            // RolePermission (SuperAdmin -> Write)
            migrationBuilder.InsertData(
                "RolePermission", "Id", "integer", new int[] { 1001 },
                new[] { 1, 2 }); // PrincipalId=1, RoleId=1, PermissionId=2

            // TestStatus
            migrationBuilder.InsertData(
                "TestStatus", "Id", "string", new string[] { "Pass" });

            // Question
            migrationBuilder.InsertData(
                "Question", "Id", "string", new string[] { "Sample Question" });

            // TestInstruction
            migrationBuilder.InsertData(
                "TestInstruction", "Id", "string", new string[] { "Sample Instruction" });

            // SubmittedTest
            migrationBuilder.InsertData(
                "SubmittedTest", "Id", "string", new string[] { "Submitted Test" });
        }
    }
}