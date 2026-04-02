using System.ComponentModel.DataAnnotations;

namespace ims.DTO;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int SupplierId { get; set; }
}

public class ProductCreateDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0.01, 1000000.00, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }

    [Range(0, 1000000, ErrorMessage = "Quantity cannot be negative.")]
    public int Quantity { get; set; }

    [Required]
    public int SupplierId { get; set; }
}

public class ProductUpdateDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0.01, 1000000.00, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }

    [Range(0, 1000000, ErrorMessage = "Quantity cannot be negative.")]
    public int Quantity { get; set; }

    [Required]
    public int SupplierId { get; set; }
}
