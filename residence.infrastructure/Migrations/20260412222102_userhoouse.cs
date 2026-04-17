using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace residence.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class userhoouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserHouses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ResidenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHouses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserHouses_Houses_HouseId",
                        column: x => x.HouseId,
                        principalSchema: "dbo",
                        principalTable: "Houses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserHouses_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserHouse_HouseId",
                table: "UserHouses",
                column: "HouseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHouse_UserId",
                table: "UserHouses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHouse_UserId_HouseId",
                table: "UserHouses",
                columns: new[] { "UserId", "HouseId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserHouses");
        }
    }
}
