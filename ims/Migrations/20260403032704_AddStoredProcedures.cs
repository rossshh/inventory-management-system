using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ims.Migrations
{
    /// <inheritdoc />
    public partial class AddStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp1 = @"
CREATE PROCEDURE GetSupplierOrderReport
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        s.Name AS Supplier,
        ISNULL(SUM(oi.Quantity), 0) AS TotalQuantity,
        ISNULL(SUM(oi.Quantity * oi.UnitPrice), 0) AS TotalValue
    FROM Suppliers s
    LEFT JOIN Products p ON s.Id = p.SupplierId
    LEFT JOIN OrderItems oi ON p.Id = oi.ProductId
    GROUP BY s.Name;
END";
            migrationBuilder.Sql(sp1);

            var sp2 = @"
CREATE PROCEDURE GetLowStockProducts
    @Threshold INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.Id,
        p.Name,
        p.Description,
        p.Price,
        p.Quantity,
        p.SupplierId
    FROM Products p
    WHERE p.Quantity < @Threshold;
END";
            migrationBuilder.Sql(sp2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetSupplierOrderReport");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetLowStockProducts");
        }
    }
}
