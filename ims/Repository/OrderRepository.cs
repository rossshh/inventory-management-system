using ims.Data;
using ims.DTO;
using ims.Models;
using ims.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ims.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _context.Orders.FindAsync(id);
        if (existing != null)
        {
            _context.Orders.Remove(existing);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _context.Orders.AsNoTracking().ToListAsync();
    }

    public async Task<Order> GetByIdAsync(int id)
    {
        return await _context.Orders.FindAsync(id);
    }

    public async Task UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<SupplierOrderReportDto>> GetSupplierOrderReportAsync()
    {
        // Calling the stored procedure mandated by the requirement document
        return await _context.Database.SqlQueryRaw<SupplierOrderReportDto>("EXEC GetSupplierOrderReport").ToListAsync();
    }
}
