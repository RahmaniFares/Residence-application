using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace residence.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class adjust_user_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Residents_ResidentId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Residents_ResidentId1",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ResidentId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ResidentId1",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ResidentId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ResidentId1",
                schema: "dbo",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ResidentId",
                schema: "dbo",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ResidentId1",
                schema: "dbo",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ResidentId",
                schema: "dbo",
                table: "Users",
                column: "ResidentId",
                unique: true,
                filter: "[ResidentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ResidentId1",
                schema: "dbo",
                table: "Users",
                column: "ResidentId1",
                unique: true,
                filter: "[ResidentId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Residents_ResidentId",
                schema: "dbo",
                table: "Users",
                column: "ResidentId",
                principalSchema: "dbo",
                principalTable: "Residents",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Residents_ResidentId1",
                schema: "dbo",
                table: "Users",
                column: "ResidentId1",
                principalSchema: "dbo",
                principalTable: "Residents",
                principalColumn: "Id");
        }
    }
}
