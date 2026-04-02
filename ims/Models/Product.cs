using System;
using System.ComponentModel.DataAnnotations;

namespace ims.Models;

/// <summary>
/// Represents a product in the inventory.
/// </summary>
public class Product
{
    /// <summary>
    /// The unique identifier for the product.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The name of the product.
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// A detailed description of the product.
    /// </summary>
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The unit price of the product.
    /// </summary>
    [Required]
    [Range(0.01, 1000000.00, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }

    /// <summary>
    /// The quantity of the product currently in stock.
    /// </summary>
    [Range(0, 1000000, ErrorMessage = "Quantity cannot be negative.")]
    public int Quantity { get; set; }
    
    /// <summary>
    /// The unique identifier of the supplier for this product.
    /// </summary>
    [Required]
    public int SupplierId { get; set; }
}
