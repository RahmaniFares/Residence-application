using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace residence.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class inithousesdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DECLARE @ResidenceId UNIQUEIDENTIFIER = '11111111-1111-1111-1111-111111111111';

            INSERT INTO Houses 
                (Id, Block, Unit, Floor, Status, CurrentResidentId, ResidenceId, 
                 CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, IsDeleted)
            SELECT 
                NEWID() AS Id,
                b.Block,
                u.Unit,
                f.Floor,
                1 AS Status,
                NULL AS CurrentResidentId,
                @ResidenceId,
                GETDATE() AS CreatedAt,
                NULL AS CreatedBy,
                NULL AS UpdatedAt,
                NULL AS UpdatedBy,
                0 AS IsDeleted
            FROM 
                (VALUES ('A'), ('B'), ('C'), ('D'), ('E')) AS b(Block)
            CROSS JOIN 
                (VALUES (0), (1), (2), (3), (4)) AS f([Floor])
            CROSS JOIN 
                (VALUES (1), (2), (3), (4)) AS u(Unit)
            ORDER BY b.Block, f.Floor, u.Unit; 
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
