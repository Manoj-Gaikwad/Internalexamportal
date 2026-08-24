using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Internalexamportal.DataAccessLayer.Migrations
{
    public partial class Changed_Role_entity_from_IdentityRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Role",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Role",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "15bda09a-effe-4634-88dc-855d9642bfed",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "IsActive" },
                values: new object[] { "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", new DateTime(2020, 7, 1, 17, 15, 38, 967, DateTimeKind.Local), true });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "859e583d-691f-40b4-adb2-828703ee1dd7",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "IsActive" },
                values: new object[] { "31a59558-0a25-48f4-91ba-6a2ffc0ad41c", new DateTime(2020, 7, 1, 17, 15, 38, 967, DateTimeKind.Local), true });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "IsActive" },
                values: new object[] { "03f9632b-47fd-4ec4-a53f-39e14666d282", new DateTime(2020, 7, 1, 17, 15, 38, 966, DateTimeKind.Local), true });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Role");
        }
    }
}
