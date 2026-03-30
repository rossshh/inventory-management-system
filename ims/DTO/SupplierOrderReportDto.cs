namespace ims.DTO;

public class SupplierOrderReportDto
{
    public string Supplier { get; set; } = string.Empty;
    public int TotalQuantity { get; set; }
    public decimal TotalValue { get; set; }
}