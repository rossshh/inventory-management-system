using ims.Data;
using ims.Models;
using ims.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ims.Repository;

public class SupplierRepository : ISupplierRepository
{
    private readonly AppDbContext _context;

    public SupplierRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Supplier supplier)
    {
        await _context.Set<Supplier>().AddAsync(supplier);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var supplier = await _context.Set<Supplier>().FindAsync(id);
        if (supplier != null)
        {
            _context.Set<Supplier>().Remove(supplier);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Supplier>> GetAllAsync()
    {
        return await _context.Set<Supplier>().AsNoTracking().ToListAsync();
    }

    public async Task<Supplier?> GetByIdAsync(int id)
    {
        return await _context.Set<Supplier>().FindAsync(id);
    }

    public async Task UpdateAsync(Supplier supplier)
    {
        _context.Set<Supplier>().Update(supplier);
        await _context.SaveChangesAsync();
    }
}
