using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Internalexamportal.DataAccessLayer.Migrations
{
    public partial class Added_New_Test_Settings_Lookup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "15bda09a-effe-4634-88dc-855d9642bfed",
                columns: new[] { "ConcurrencyStamp", "CreatedDate" },
                values: new object[] { "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", new DateTime(2020, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "859e583d-691f-40b4-adb2-828703ee1dd7",
                columns: new[] { "ConcurrencyStamp", "CreatedDate" },
                values: new object[] { "31a59558-0a25-48f4-91ba-6a2ffc0ad41c", new DateTime(2020, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6",
                columns: new[] { "ConcurrencyStamp", "CreatedDate" },
                values: new object[] { "03f9632b-47fd-4ec4-a53f-39e14666d282", new DateTime(2020, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "TestSettingType",
                keyColumn: "Id",
                keyValue: 2,
                column: "Type",
                value: "Display Result");

            migrationBuilder.UpdateData(
                table: "TestSettingType",
                keyColumn: "Id",
                keyValue: 4,
                column: "Type",
                value: "Window Minimise Warning");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "15bda09a-effe-4634-88dc-855d9642bfed",
                columns: new[] { "ConcurrencyStamp", "CreatedDate" },
                values: new object[] { "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", new DateTime(2020, 7, 1, 17, 15, 38, 967, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "859e583d-691f-40b4-adb2-828703ee1dd7",
                columns: new[] { "ConcurrencyStamp", "CreatedDate" },
                values: new object[] { "31a59558-0a25-48f4-91ba-6a2ffc0ad41c", new DateTime(2020, 7, 1, 17, 15, 38, 967, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6",
                columns: new[] { "ConcurrencyStamp", "CreatedDate" },
                values: new object[] { "03f9632b-47fd-4ec4-a53f-39e14666d282", new DateTime(2020, 7, 1, 17, 15, 38, 966, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "TestSettingType",
                keyColumn: "Id",
                keyValue: 2,
                column: "Type",
                value: "Grouping");

            migrationBuilder.UpdateData(
                table: "TestSettingType",
                keyColumn: "Id",
                keyValue: 4,
                column: "Type",
                value: "Full screen");
        }
    }
}
