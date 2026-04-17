using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace residence.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentLines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentLines",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromMonth = table.Column<int>(type: "int", nullable: false),
                    FromYear = table.Column<int>(type: "int", nullable: false),
                    ToMonth = table.Column<int>(type: "int", nullable: false),
                    ToYear = table.Column<int>(type: "int", nullable: false),
                    Tarif = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ResidenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentLines_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalSchema: "dbo",
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentLines_PaymentId",
                schema: "dbo",
                table: "PaymentLines",
                column: "PaymentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentLines",
                schema: "dbo");
        }
    }
}
