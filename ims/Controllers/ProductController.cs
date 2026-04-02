using ims.Models;
using ims.Services.Interfaces;
using ims.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ims.Controllers;

[Route("api/products")]
[ApiController]
[Authorize(Roles = "Admin,Manager")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Retrieves all products from the inventory (Manager/Admin only).
    /// </summary>
    /// <returns>A list of products.</returns>
    /// <response code="200">Returns the list of products.</response>
    /// <response code="401">If the caller is not authenticated.</response>
    /// <response code="403">If the caller does not have Manager or Admin roles.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    /// <summary>
    /// Retrieves a specific product by ID (Manager/Admin only).
    /// </summary>
    /// <param name="id">The ID of the product to retrieve.</param>
    /// <returns>The product details.</returns>
    /// <response code="200">Returns the requested product.</response>
    /// <response code="404">If the product is not found.</response>
    /// <response code="401">If the caller is not authenticated.</response>
    /// <response code="403">If the caller does not have Manager or Admin roles.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Product), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound(new ErrorResponse(404, "Product not found"));
        return Ok(product);
    }

    /// <summary>
    /// Creates a new product (Manager/Admin only).
    /// </summary>
    /// <param name="product">The details of the product to create.</param>
    /// <returns>The created product details.</returns>
    /// <response code="201">Returns the newly created product.</response>
    /// <response code="400">If the input data is invalid.</response>
    /// <response code="401">If the caller is not authenticated.</response>
    /// <response code="403">If the caller does not have Manager or Admin roles.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Product), 201)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    public async Task<IActionResult> Create([FromBody] Product product)
    {
        await _productService.AddAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    /// <summary>
    /// Updates an existing product's information (Manager/Admin only).
    /// </summary>
    /// <param name="id">The ID of the product to update.</param>
    /// <param name="product">The updated product information.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">If the update was successful.</response>
    /// <response code="400">If the ID in the URL does not match the ID in the body.</response>
    /// <response code="404">If the product is not found.</response>
    /// <response code="401">If the caller is not authenticated.</response>
    /// <response code="403">If the caller does not have Manager or Admin roles.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    public async Task<IActionResult> Update(int id, [FromBody] Product product)
    {
        if (product == null || id != product.Id) return BadRequest(new ErrorResponse(400, "ID mismatch"));
        await _productService.UpdateAsync(product);
        return NoContent();
    }

    /// <summary>
    /// Deletes a product (Manager/Admin only).
    /// </summary>
    /// <param name="id">The ID of the product to delete.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">If the deletion was successful.</response>
    /// <response code="401">If the caller is not authenticated.</response>
    /// <response code="403">If the caller does not have Manager or Admin roles.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteAsync(id);
        return NoContent();
    }
}
