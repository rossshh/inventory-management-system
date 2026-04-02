using ims.Models;
using ims.Services.Interfaces;
using ims.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ims.Controllers;

[Route("api/suppliers")]
[ApiController]
[Authorize(Roles = "Admin,Manager")]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SuppliersController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    /// <summary>
    /// Retrieves all suppliers (Manager/Admin only).
    /// </summary>
    /// <returns>A list of suppliers.</returns>
    /// <response code="200">Returns the list of suppliers.</response>
    /// <response code="401">If the caller is not authenticated.</response>
    /// <response code="403">If the caller does not have Manager or Admin roles.</response>
    [HttpGet]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(typeof(IEnumerable<Supplier>), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    public async Task<IActionResult> GetAll()
    {
        var suppliers = await _supplierService.GetAllAsync();
        return Ok(suppliers);
    }

    /// <summary>
    /// Retrieves a specific supplier by ID (Manager/Admin only).
    /// </summary>
    /// <param name="id">The ID of the supplier to retrieve.</param>
    /// <returns>The supplier details.</returns>
    /// <response code="200">Returns the requested supplier.</response>
    /// <response code="404">If the supplier is not found.</response>
    /// <response code="401">If the caller is not authenticated.</response>
    /// <response code="403">If the caller does not have Manager or Admin roles.</response>
    [HttpGet("{id}")]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(typeof(Supplier), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    public async Task<IActionResult> GetById(int id)
    {
        var supplier = await _supplierService.GetByIdAsync(id);
        if (supplier == null) return NotFound(new ErrorResponse(404, "Supplier not found"));
        return Ok(supplier);
    }

    /// <summary>
    /// Creates a new supplier (Manager/Admin only).
    /// </summary>
    /// <param name="supplier">The details of the supplier to create.</param>
    /// <returns>The created supplier details.</returns>
    /// <response code="201">Returns the newly created supplier.</response>
    /// <response code="401">If the caller is not authenticated.</response>
    /// <response code="403">If the caller does not have Manager or Admin roles.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Supplier), 201)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    public async Task<IActionResult> Create([FromBody] Supplier supplier)
    {
        await _supplierService.AddAsync(supplier);
        return CreatedAtAction(nameof(GetById), new { id = supplier.Id }, supplier);
    }

    /// <summary>
    /// Updates an existing supplier's information (Manager/Admin only).
    /// </summary>
    /// <param name="id">The ID of the supplier to update.</param>
    /// <param name="supplier">The updated supplier information.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">If the update was successful.</response>
    /// <response code="400">If the ID in the URL does not match the ID in the body.</response>
    /// <response code="404">If the supplier is not found.</response>
    /// <response code="401">If the caller is not authenticated.</response>
    /// <response code="403">If the caller does not have Manager or Admin roles.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    public async Task<IActionResult> Update(int id, [FromBody] Supplier supplier)
    {
        if (supplier == null || id != supplier.Id) return BadRequest(new ErrorResponse(400, "ID mismatch"));
        await _supplierService.UpdateAsync(supplier);
        return NoContent();
    }

    /// <summary>
    /// Deletes a supplier (Admin only).
    /// </summary>
    /// <param name="id">The ID of the supplier to delete.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">If the deletion was successful.</response>
    /// <response code="401">If the caller is not authenticated.</response>
    /// <response code="403">If the caller is not an Admin.</response>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    public async Task<IActionResult> Delete(int id)
    {
        await _supplierService.DeleteAsync(id);
        return NoContent();
    }
}
