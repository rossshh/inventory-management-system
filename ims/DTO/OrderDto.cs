using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ims.DTO;

public class OrderDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
}

public class OrderCreateDto
{
    [Required]
    public int UserId { get; set; }

    public ICollection<OrderItemCreateDto> OrderItems { get; set; } = new List<OrderItemCreateDto>();
}

public class OrderUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    public ICollection<OrderItemCreateDto> OrderItems { get; set; } = new List<OrderItemCreateDto>();
}

public class OrderItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class OrderItemCreateDto
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    [Range(1, 10000, ErrorMessage = "Quantity must be between 1 and 10000.")]
    public int Quantity { get; set; }

    [Required]
    [Range(0.01, 1000000.00, ErrorMessage = "UnitPrice must be greater than zero.")]
    public decimal UnitPrice { get; set; }
}

public class SupplierOrderReportDto
{
    [Column("Supplier")]
    public string SupplierName { get; set; } = string.Empty;
    public int TotalQuantity { get; set; }
    public decimal TotalValue { get; set; }
}
