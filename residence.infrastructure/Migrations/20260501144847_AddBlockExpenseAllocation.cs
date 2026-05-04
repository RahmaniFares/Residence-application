using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace residence.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBlockExpenseAllocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BlockId",
                schema: "dbo",
                table: "Expenses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Blocks",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Coefficient = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: false),
                    ResidenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_BlockId",
                schema: "dbo",
                table: "Expenses",
                column: "BlockId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Blocks_BlockId",
                schema: "dbo",
                table: "Expenses",
                column: "BlockId",
                principalSchema: "dbo",
                principalTable: "Blocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Blocks_BlockId",
                schema: "dbo",
                table: "Expenses");

            migrationBuilder.DropTable(
                name: "Blocks",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_BlockId",
                schema: "dbo",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "BlockId",
                schema: "dbo",
                table: "Expenses");
        }
    }
}
