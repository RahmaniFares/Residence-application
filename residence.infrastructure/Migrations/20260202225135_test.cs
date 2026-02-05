using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace residence.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostComments_Residents_AuthorId",
                schema: "dbo",
                table: "PostComments");

            migrationBuilder.DropIndex(
                name: "IX_PostComments_AuthorId",
                schema: "dbo",
                table: "PostComments");

            migrationBuilder.CreateIndex(
                name: "IX_PostComments_AuthorId",
                schema: "dbo",
                table: "PostComments",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostComments_Residents_AuthorId",
                schema: "dbo",
                table: "PostComments",
                column: "AuthorId",
                principalSchema: "dbo",
                principalTable: "Residents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostComments_Residents_AuthorId",
                schema: "dbo",
                table: "PostComments");

            migrationBuilder.DropIndex(
                name: "IX_PostComments_AuthorId",
                schema: "dbo",
                table: "PostComments");

            migrationBuilder.CreateIndex(
                name: "IX_PostComments_AuthorId",
                schema: "dbo",
                table: "PostComments",
                column: "AuthorId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PostComments_Residents_AuthorId",
                schema: "dbo",
                table: "PostComments",
                column: "AuthorId",
                principalSchema: "dbo",
                principalTable: "Residents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
