using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace residence.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class user_resident_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Residents_Users_UserId",
                schema: "dbo",
                table: "Residents");

            migrationBuilder.DropIndex(
                name: "IX_Residents_UserId",
                schema: "dbo",
                table: "Residents");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "dbo",
                table: "Residents");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "dbo",
                table: "Residents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Residents_UserId",
                schema: "dbo",
                table: "Residents",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Residents_Users_UserId",
                schema: "dbo",
                table: "Residents",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
