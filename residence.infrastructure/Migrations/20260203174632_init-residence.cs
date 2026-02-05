using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace residence.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initresidence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Residences",
                columns: new[] { "Id", "Address", "City", "CreatedAt", "Description", "Name", "State", "UpdatedAt", "ZipCode" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), "123 Main St", "Anytown", new DateTime(2026, 2, 3, 17, 46, 31, 384, DateTimeKind.Utc).AddTicks(1182), "This is the default residence.", "Residence Mariem", "State", null, "12345" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "ResidenceSettings",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "InitialBudget", "ResidenceId", "ResidenceName", "ResidencePlace", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111112"), new DateTime(2026, 2, 3, 17, 46, 31, 384, DateTimeKind.Utc).AddTicks(3721), null, 1000m, new Guid("11111111-1111-1111-1111-111111111111"), "Default Residence", "Default Place", null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "ResidenceSettings",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111112"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Residences",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));
        }
    }
}
