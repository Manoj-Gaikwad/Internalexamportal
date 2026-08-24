using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Internalexamportal.DataAccessLayer.Migrations
{
    public partial class Added_user_roll_number : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RollNumber",
                table: "User",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CandidateNumbering",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Count = table.Column<int>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateNumbering", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CandidateNumbering",
                columns: new[] { "Id", "Count", "RowVersion" },
                values: new object[] { 1, 0, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CandidateNumbering");

            migrationBuilder.DropColumn(
                name: "RollNumber",
                table: "User");
        }
    }
}
