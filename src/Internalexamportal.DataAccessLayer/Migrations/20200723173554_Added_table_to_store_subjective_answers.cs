using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Internalexamportal.DataAccessLayer.Migrations
{
    public partial class Added_table_to_store_subjective_answers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubjectiveAnswer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SubmittedTestId = table.Column<int>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false),
                    Answer = table.Column<string>(nullable: true),
                    ObtainedMarks = table.Column<int>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModifiedById = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectiveAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectiveAnswer_User_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectiveAnswer_User_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectiveAnswer_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectiveAnswer_SubmittedTest_SubmittedTestId",
                        column: x => x.SubmittedTestId,
                        principalTable: "SubmittedTest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectiveAnswer_CreatedById",
                table: "SubjectiveAnswer",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectiveAnswer_ModifiedById",
                table: "SubjectiveAnswer",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectiveAnswer_QuestionId",
                table: "SubjectiveAnswer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectiveAnswer_SubmittedTestId",
                table: "SubjectiveAnswer",
                column: "SubmittedTestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubjectiveAnswer");
        }
    }
}
