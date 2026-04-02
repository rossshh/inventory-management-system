using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ims.Models;

/// <summary>
/// Represents a specific product line item within an order.
/// </summary>
public class OrderItem
{
    /// <summary>
    /// The unique identifier for the order item.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The ID of the product being ordered.
    /// </summary>
    [Required]
    public int ProductId { get; set; }

    /// <summary>
    /// The number of units ordered.
    /// </summary>
    [Required]
    [Range(1, 10000, ErrorMessage = "Quantity must be between 1 and 10000.")]
    public int Quantity { get; set; }

    /// <summary>
    /// The price per unit at the time of the order.
    /// </summary>
    [Required]
    [Range(0.01, 1000000.00, ErrorMessage = "UnitPrice must be greater than zero.")]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// The ID of the parent order.
    /// </summary>
    [Required]
    public int OrderId { get; set; }

    /// <summary>
    /// Navigation property to the parent order.
    /// </summary>
    [JsonIgnore]
    public Order? Order { get; set; }
}
