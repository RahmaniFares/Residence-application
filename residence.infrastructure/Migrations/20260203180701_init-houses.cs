using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace residence.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class inithouses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ResidenceSettings",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111112"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 3, 18, 7, 0, 889, DateTimeKind.Utc).AddTicks(9506));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Residences",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 3, 18, 7, 0, 889, DateTimeKind.Utc).AddTicks(7102));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ResidenceSettings",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111112"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 3, 17, 46, 31, 384, DateTimeKind.Utc).AddTicks(3721));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Residences",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 3, 17, 46, 31, 384, DateTimeKind.Utc).AddTicks(1182));
        }
    }
}
