using ims.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ims.Services.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<OrderDto>> GetAllAsync();
    Task<OrderDto?> GetByIdAsync(int id);
    Task<OrderDto> AddAsync(OrderCreateDto orderDto);
    Task UpdateAsync(int id, OrderUpdateDto orderDto);
    Task DeleteAsync(int id);
    Task<IEnumerable<SupplierOrderReportDto>> GetSupplierOrderReportAsync();
}
