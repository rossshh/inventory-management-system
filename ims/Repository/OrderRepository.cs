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
        var sql = @"
            SELECT s.Name AS Supplier, SUM(o.Quantity) AS TotalQuantity, SUM(o.Quantity * p.Price) AS TotalValue
            FROM Orders o
            JOIN Products p ON o.ProductId = p.Id
            JOIN Suppliers s ON o.SupplierId = s.Id
            GROUP BY s.Name";

        return await _context.Database.SqlQueryRaw<SupplierOrderReportDto>(sql).ToListAsync();
    }
}
