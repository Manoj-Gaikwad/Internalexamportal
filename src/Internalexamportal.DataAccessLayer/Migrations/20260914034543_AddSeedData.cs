using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Internalexamportal.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ActivationType",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "CommonType" },
                    { 2, "AccessType" }
                });

            migrationBuilder.InsertData(
                table: "CandidateNumbering",
                columns: new[] { "Id", "Count" },
                values: new object[] { 1, 0 });

            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "Id", "Address", "Email", "IsDeleted", "Logo", "Name", "Phone" },
                values: new object[] { 1, "Life Repulic R9-sector B1307 marunji-kasarsai Road Marunji , Pune 411057", "manojdgaikwad4165@gmail.com", false, null, "Vidyaops Exam Portal", "9898989898" });

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
                table: "Permission",
                columns: new[] { "Id", "Description", "IsActive", "ParentId", "PermissionName" },
                values: new object[,]
                {
                    { 1, "", true, 0, "Dashboard" },
                    { 2, "", true, 0, "Question Manager" },
                    { 3, "", true, 0, "Subject Manager" },
                    { 4, "", true, 0, "Test Manager" },
                    { 5, "", true, 0, "Candidates Manager" },
                    { 6, "", true, 0, "Admin Manager" },
                    { 7, "", true, 1, "View" },
                    { 8, "", true, 1, "Add" },
                    { 9, "", true, 2, "View" },
                    { 10, "", true, 2, "Add/Edit" },
                    { 11, "", true, 2, "Delete" },
                    { 12, "", true, 2, "Import" },
                    { 13, "", true, 3, "View" },
                    { 14, "", true, 3, "Add/Edit" },
                    { 15, "", true, 3, "Delete" },
                    { 16, "", true, 4, "View" },
                    { 17, "", true, 4, "Add/Edit" },
                    { 18, "", true, 4, "Delete" },
                    { 19, "", true, 5, "View" },
                    { 20, "", true, 5, "Add/Edit" },
                    { 21, "", true, 5, "Delete" },
                    { 22, "", true, 5, "Import" },
                    { 23, "", true, 6, "View" },
                    { 24, "", true, 6, "Add/Edit" },
                    { 25, "", true, 6, "Delete" },
                    { 26, "", true, 6, "Role Permissions" }
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

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedDate", "IsActive", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "15bda09a-effe-4634-88dc-855d9642bfed", "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", new DateTime(2020, 10, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Candidate", "CANDIDATE" },
                    { "859e583d-691f-40b4-adb2-828703ee1dd7", "31a59558-0a25-48f4-91ba-6a2ffc0ad41c", new DateTime(2020, 10, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Admin", "ADMIN" },
                    { "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", "03f9632b-47fd-4ec4-a53f-39e14666d282", new DateTime(2020, 10, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "SuperAdmin", "SUPERADMIN" }
                });

            migrationBuilder.InsertData(
                table: "TestSettingType",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "Shuffling" },
                    { 2, "Display Result" },
                    { 3, "Multiple Attempt" },
                    { 4, "Window Minimise Warning" },
                    { 5, "Display Answer Sheet" }
                });

            migrationBuilder.InsertData(
                table: "TestStatus",
                columns: new[] { "Id", "Status" },
                values: new object[,]
                {
                    { 1, "In Progress" },
                    { 2, "Submitted" },
                    { 3, "Evaluated" }
                });

            migrationBuilder.InsertData(
                table: "TestType",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "Objective" },
                    { 2, "Subjective" },
                    { 3, "Objective & Subjective" }
                });

            migrationBuilder.InsertData(
                table: "Group",
                columns: new[] { "Id", "ClientId", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, 1, false, "General" },
                    { 2, 1, false, "Others" }
                });

            migrationBuilder.InsertData(
                table: "RolePermission",
                columns: new[] { "Id", "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 1, 1, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 2, 2, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 3, 3, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 4, 4, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 5, 5, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 6, 6, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 7, 7, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 8, 8, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 9, 9, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 10, 10, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 11, 11, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 12, 12, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 13, 13, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 14, 14, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 15, 15, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 16, 16, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 17, 17, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 18, 18, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 19, 19, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 20, 20, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 21, 21, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 22, 22, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 23, 23, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 24, 24, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
                    { 25, 25, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" },
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
                    { 45, 21, "859e583d-691f-40b4-adb2-828703ee1dd7" },
                    { 46, 22, "859e583d-691f-40b4-adb2-828703ee1dd7" },
                    { 47, 26, "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActivationType",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ActivationType",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CandidateNumbering",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CorrectOption",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CorrectOption",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CorrectOption",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CorrectOption",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CorrectOption",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CorrectOption",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CorrectOption",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "DifficultLevel",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "DifficultLevel",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "DifficultLevel",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Group",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Group",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "QuestionType",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "15bda09a-effe-4634-88dc-855d9642bfed");

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "TestSettingType",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TestSettingType",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TestSettingType",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TestSettingType",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TestSettingType",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TestStatus",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TestStatus",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TestStatus",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TestType",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TestType",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TestType",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "859e583d-691f-40b4-adb2-828703ee1dd7");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6");
        }
    }
}
