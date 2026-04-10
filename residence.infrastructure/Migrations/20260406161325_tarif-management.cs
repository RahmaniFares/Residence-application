using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace residence.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class tarifmanagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tarifs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResidenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "USD"),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarifs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tarifs_Residences_ResidenceId",
                        column: x => x.ResidenceId,
                        principalSchema: "dbo",
                        principalTable: "Residences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TarifHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TarifId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResidenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreviousAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    NewAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PreviousDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NewDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ChangeReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TarifHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TarifHistories_Residences_ResidenceId",
                        column: x => x.ResidenceId,
                        principalSchema: "dbo",
                        principalTable: "Residences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TarifHistories_Tarifs_TarifId",
                        column: x => x.TarifId,
                        principalTable: "Tarifs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TarifHistory_ChangedAt",
                table: "TarifHistories",
                column: "ChangedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TarifHistory_ResidenceId_ChangedAt",
                table: "TarifHistories",
                columns: new[] { "ResidenceId", "ChangedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_TarifHistory_TarifId",
                table: "TarifHistories",
                column: "TarifId");

            migrationBuilder.CreateIndex(
                name: "IX_Tarif_EffectiveDate",
                table: "Tarifs",
                column: "EffectiveDate");

            migrationBuilder.CreateIndex(
                name: "IX_Tarif_ResidenceId_IsActive",
                table: "Tarifs",
                columns: new[] { "ResidenceId", "IsActive" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TarifHistories");

            migrationBuilder.DropTable(
                name: "Tarifs");
        }
    }
}
