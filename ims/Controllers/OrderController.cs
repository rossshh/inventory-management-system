using ims.Models;
using ims.Services.Interfaces;
using ims.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ims.Controllers;

[Route("api/orders")]
[ApiController]
[Authorize(Roles = "Admin,Manager,Staff")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Retrieves all orders (Staff/Manager/Admin).
    /// </summary>
    /// <returns>A list of orders.</returns>
    /// <response code="200">Returns the list of orders.</response>
    /// <response code="401">If the caller is not authenticated.</response>
    /// <response code="403">If the caller does not have the required role.</response>
    [HttpGet]
    [Authorize(Roles = "Staff,Manager,Admin")]
    [ProducesResponseType(typeof(IEnumerable<Order>), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _orderService.GetAllAsync();
        return Ok(orders);
    }

    /// <summary>
    /// Retrieves a specific order by ID (Staff/Manager/Admin).
    /// </summary>
    /// <param name="id">The ID of the order to retrieve.</param>
    /// <returns>The order details.</returns>
    /// <response code="200">Returns the requested order.</response>
    /// <response code="404">If the order is not found.</response>
    /// <response code="401">If the caller is not authenticated.</response>
    /// <response code="403">If the caller does not have the required role.</response>
    [HttpGet("{id}")]
    [Authorize(Roles = "Staff,Manager,Admin")]
    [ProducesResponseType(typeof(Order), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _orderService.GetByIdAsync(id);
        if (order == null) return NotFound(new ErrorResponse(404, "Order not found"));
        return Ok(order);
    }

    /// <summary>
    /// Creates a new order (Staff only).
    /// </summary>
    /// <param name="order">The details of the order to create.</param>
    /// <returns>The created order details.</returns>
    /// <response code="201">Returns the newly created order.</response>
    /// <response code="401">If the caller is not authenticated.</response>
    /// <response code="403">If the caller is not Staff.</response>
    [HttpPost]
    [Authorize(Roles = "Staff")]
    [ProducesResponseType(typeof(Order), 201)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    public async Task<IActionResult> Create([FromBody] Order order)
    {
        await _orderService.AddAsync(order);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    /// <summary>
    /// Updates an existing order (Manager/Admin only).
    /// </summary>
    /// <param name="id">The ID of the order to update.</param>
    /// <param name="order">The updated order information.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">If the update was successful.</response>
    /// <response code="400">If the ID in the URL does not match the ID in the body.</response>
    /// <response code="404">If the order is not found.</response>
    /// <response code="401">If the caller is not authenticated.</response>
    /// <response code="403">If the caller does not have Manager or Admin roles.</response>
    [HttpPut("{id}")]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    public async Task<IActionResult> Update(int id, [FromBody] Order order)
    {
        if (order == null || id != order.Id) return BadRequest(new ErrorResponse(400, "ID mismatch"));
        await _orderService.UpdateAsync(order);
        return NoContent();
    }

    /// <summary>
    /// Deletes an order (Admin only).
    /// </summary>
    /// <param name="id">The ID of the order to delete.</param>
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
        await _orderService.DeleteAsync(id);
        return NoContent();
    }
}
