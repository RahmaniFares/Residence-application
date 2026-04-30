using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace residence.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeManagementSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donation_Houses_HouseId",
                table: "Donation");

            migrationBuilder.DropForeignKey(
                name: "FK_Donation_Residents_DonorId",
                table: "Donation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Donation",
                table: "Donation");

            migrationBuilder.RenameTable(
                name: "Donation",
                newName: "Donations",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_Donation_HouseId",
                schema: "dbo",
                table: "Donations",
                newName: "IX_Donations_HouseId");

            migrationBuilder.RenameIndex(
                name: "IX_Donation_DonorId",
                schema: "dbo",
                table: "Donations",
                newName: "IX_Donations_DonorId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DonationDate",
                schema: "dbo",
                table: "Donations",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "dbo",
                table: "Donations",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Donations",
                schema: "dbo",
                table: "Donations",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Employees",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResidenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.CheckConstraint("CK_Employees_HireDate", "[HireDate] <= GETUTCDATE()");
                    table.ForeignKey(
                        name: "FK_Employees_Residences_ResidenceId",
                        column: x => x.ResidenceId,
                        principalSchema: "dbo",
                        principalTable: "Residences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeSalaries",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Reason = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResidenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSalaries", x => x.Id);
                    table.CheckConstraint("CK_EmployeeSalaries_Amount_Positive", "[Amount] > 0");
                    table.CheckConstraint("CK_EmployeeSalaries_DateRange", "[EndDate] IS NULL OR [EndDate] >= [EffectiveDate]");
                    table.ForeignKey(
                        name: "FK_EmployeeSalaries_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "dbo",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Donations_DonationDate",
                schema: "dbo",
                table: "Donations",
                column: "DonationDate");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_HouseId_DonationDate",
                schema: "dbo",
                table: "Donations",
                columns: new[] { "HouseId", "DonationDate" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Donations_Amount_Positive",
                schema: "dbo",
                table: "Donations",
                sql: "[Amount] > 0");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                schema: "dbo",
                table: "Employees",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ResidenceId",
                schema: "dbo",
                table: "Employees",
                column: "ResidenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ResidenceId_Position",
                schema: "dbo",
                table: "Employees",
                columns: new[] { "ResidenceId", "Position" });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ResidenceId_Status",
                schema: "dbo",
                table: "Employees",
                columns: new[] { "ResidenceId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaries_EffectiveDate",
                schema: "dbo",
                table: "EmployeeSalaries",
                column: "EffectiveDate");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaries_EmployeeId",
                schema: "dbo",
                table: "EmployeeSalaries",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaries_EmployeeId_EffectiveDate",
                schema: "dbo",
                table: "EmployeeSalaries",
                columns: new[] { "EmployeeId", "EffectiveDate" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaries_EmployeeId_IsCurrent",
                schema: "dbo",
                table: "EmployeeSalaries",
                columns: new[] { "EmployeeId", "IsCurrent" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaries_IsCurrent",
                schema: "dbo",
                table: "EmployeeSalaries",
                column: "IsCurrent");

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_Houses_HouseId",
                schema: "dbo",
                table: "Donations",
                column: "HouseId",
                principalSchema: "dbo",
                principalTable: "Houses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_Residents_DonorId",
                schema: "dbo",
                table: "Donations",
                column: "DonorId",
                principalSchema: "dbo",
                principalTable: "Residents",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Houses_HouseId",
                schema: "dbo",
                table: "Donations");

            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Residents_DonorId",
                schema: "dbo",
                table: "Donations");

            migrationBuilder.DropTable(
                name: "EmployeeSalaries",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Employees",
                schema: "dbo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Donations",
                schema: "dbo",
                table: "Donations");

            migrationBuilder.DropIndex(
                name: "IX_Donations_DonationDate",
                schema: "dbo",
                table: "Donations");

            migrationBuilder.DropIndex(
                name: "IX_Donations_HouseId_DonationDate",
                schema: "dbo",
                table: "Donations");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Donations_Amount_Positive",
                schema: "dbo",
                table: "Donations");

            migrationBuilder.RenameTable(
                name: "Donations",
                schema: "dbo",
                newName: "Donation");

            migrationBuilder.RenameIndex(
                name: "IX_Donations_HouseId",
                table: "Donation",
                newName: "IX_Donation_HouseId");

            migrationBuilder.RenameIndex(
                name: "IX_Donations_DonorId",
                table: "Donation",
                newName: "IX_Donation_DonorId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DonationDate",
                table: "Donation",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Donation",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Donation",
                table: "Donation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Donation_Houses_HouseId",
                table: "Donation",
                column: "HouseId",
                principalSchema: "dbo",
                principalTable: "Houses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Donation_Residents_DonorId",
                table: "Donation",
                column: "DonorId",
                principalSchema: "dbo",
                principalTable: "Residents",
                principalColumn: "Id");
        }
    }
}
