using ims.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet("supplier-order")]
    [Authorize(Roles = "Manager,Admin")]
    public async Task<IActionResult> GetSupplierOrderReport()
    {
        var report = await _orderService.GetSupplierOrderReportAsync();
        return Ok(report);
    }
}