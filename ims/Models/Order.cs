using System;
using System.ComponentModel.DataAnnotations;

namespace ims.Models;

/// <summary>
/// Represents a customer or inventory order.
/// </summary>
public class Order
{
    /// <summary>
    /// The unique identifier for the order.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The ID of the user who created or managed the order.
    /// </summary>
    [Required]
    public int UserId { get; set; }

    /// <summary>
    /// The date and time when the order was placed.
    /// </summary>
    [Required]
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The collection of individual line items within this order.
    /// </summary>
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
