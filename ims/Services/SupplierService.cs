using ims.Models;
using ims.Repository.Interfaces;
using ims.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ims.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _supplierRepository;

    public SupplierService(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task AddAsync(Supplier supplier)
    {
        await _supplierRepository.AddAsync(supplier);
    }

    public async Task DeleteAsync(int id)
    {
        await _supplierRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Supplier>> GetAllAsync()
    {
        return await _supplierRepository.GetAllAsync();
    }

    public async Task<Supplier?> GetByIdAsync(int id)
    {
        return await _supplierRepository.GetByIdAsync(id);
    }

    public async Task UpdateAsync(Supplier supplier)
    {
        await _supplierRepository.UpdateAsync(supplier);
    }
}
