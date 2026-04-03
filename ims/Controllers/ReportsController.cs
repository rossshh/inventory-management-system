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
    private readonly IProductService _productService;

    public ReportsController(IOrderService orderService, IProductService productService)
    {
        _orderService = orderService;
        _productService = productService;
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

    /// <summary>
    /// Generates a report of low stock products (Manager/Admin only).
    /// </summary>
    /// <param name="threshold">The quantity threshold to consider a product as low stock.</param>
    /// <returns>A list of low stock products.</returns>
    /// <response code="200">Returns the low stock products.</response>
    /// <response code="401">If the caller is not authenticated.</response>
    /// <response code="403">If the caller does not have Manager or Admin roles.</response>
    [HttpGet("low-stock")]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(typeof(IEnumerable<ims.DTO.ProductDto>), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    public async Task<IActionResult> GetLowStockProducts([FromQuery] int threshold = 10)
    {
        var products = await _productService.GetLowStockProductsAsync(threshold);
        return Ok(products);
    }
}