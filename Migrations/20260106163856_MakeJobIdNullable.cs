using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortal.Migrations
{
    /// <inheritdoc />
    public partial class MakeJobIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobId",
                table: "ApplyForms",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Bookmarks",
                columns: table => new
                {
                    BookmarkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookmarks", x => x.BookmarkId);
                    table.ForeignKey(
                        name: "FK_Bookmarks_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookmarks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplyForms_JobId",
                table: "ApplyForms",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookmarks_JobId",
                table: "Bookmarks",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookmarks_UserId",
                table: "Bookmarks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyForms_Jobs_JobId",
                table: "ApplyForms",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "JobId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplyForms_Jobs_JobId",
                table: "ApplyForms");

            migrationBuilder.DropTable(
                name: "Bookmarks");

            migrationBuilder.DropIndex(
                name: "IX_ApplyForms_JobId",
                table: "ApplyForms");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "ApplyForms");
        }
    }
}
