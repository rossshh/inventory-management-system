using ims.DTO;
using ims.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ims.Repository.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order> GetByIdAsync(int id);
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(int id);
    Task<IEnumerable<SupplierOrderReportDto>> GetSupplierOrderReportAsync();
}
