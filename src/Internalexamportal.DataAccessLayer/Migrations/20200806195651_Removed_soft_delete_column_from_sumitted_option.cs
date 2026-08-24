using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Internalexamportal.DataAccessLayer.Migrations
{
    public partial class Removed_soft_delete_column_from_sumitted_option : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmittedOption_User_CreatedById",
                table: "SubmittedOption");

            migrationBuilder.DropForeignKey(
                name: "FK_SubmittedOption_User_ModifiedById",
                table: "SubmittedOption");

            migrationBuilder.DropIndex(
                name: "IX_SubmittedOption_CreatedById",
                table: "SubmittedOption");

            migrationBuilder.DropIndex(
                name: "IX_SubmittedOption_ModifiedById",
                table: "SubmittedOption");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "SubmittedOption");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "SubmittedOption");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SubmittedOption");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "SubmittedOption");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "SubmittedOption");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "SubmittedOption",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "SubmittedOption",
                nullable: false,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SubmittedOption",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "SubmittedOption",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "SubmittedOption",
                nullable: false,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.CreateIndex(
                name: "IX_SubmittedOption_CreatedById",
                table: "SubmittedOption",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SubmittedOption_ModifiedById",
                table: "SubmittedOption",
                column: "ModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmittedOption_User_CreatedById",
                table: "SubmittedOption",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubmittedOption_User_ModifiedById",
                table: "SubmittedOption",
                column: "ModifiedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
