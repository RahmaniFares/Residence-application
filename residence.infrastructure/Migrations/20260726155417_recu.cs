using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace residence.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class recu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceiptNumber",
                schema: "dbo",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiptNumber",
                schema: "dbo",
                table: "Payments");
        }
    }
}
