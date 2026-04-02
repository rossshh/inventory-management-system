using System.ComponentModel.DataAnnotations;

namespace ims.Models;

/// <summary>
/// Represents a company that supplies products to the inventory.
/// </summary>
public class Supplier
{
    /// <summary>
    /// The unique identifier for the supplier.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The name of the supplier company.
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The primary contact person at the supplier company.
    /// </summary>
    [StringLength(100)]
    public string ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// The contact email address of the supplier.
    /// </summary>
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The contact phone number of the supplier.
    /// </summary>
    [Phone]
    [StringLength(20)]
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// The physical address of the supplier.
    /// </summary>
    [StringLength(250)]
    public string Address { get; set; } = string.Empty;
}
