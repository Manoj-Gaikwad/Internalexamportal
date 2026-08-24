using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Internalexamportal.DataAccessLayer.Migrations
{
    public partial class UpdateApplicatioErrorLogTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExceptionType",
                table: "ApplicationError",
                newName: "User");

            migrationBuilder.RenameColumn(
                name: "ExceptionDateUTC",
                table: "ApplicationError",
                newName: "TimeStamp");

            migrationBuilder.AddColumn<string>(
                name: "Exception",
                table: "ApplicationError",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Exception",
                table: "ApplicationError");

            migrationBuilder.RenameColumn(
                name: "User",
                table: "ApplicationError",
                newName: "ExceptionType");

            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "ApplicationError",
                newName: "ExceptionDateUTC");
        }
    }
}
