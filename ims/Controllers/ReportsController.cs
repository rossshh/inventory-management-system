using ims.Services.Interfaces;
using ims.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ims.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IOrderService _orderService;

    public ReportsController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Generates a report of supplier orders (Manager/Admin only).
    /// </summary>
    /// <returns>A list of orders with supplier information.</returns>
    /// <response code="200">Returns the generated report.</response>
    /// <response code="401">If the caller is not authenticated.</response>
    /// <response code="403">If the caller does not have Manager or Admin roles.</response>
    [HttpGet("supplier-order")]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(typeof(IEnumerable<object>), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    public async Task<IActionResult> GetSupplierOrderReport()
    {
        var report = await _orderService.GetSupplierOrderReportAsync();
        return Ok(report);
    }
}