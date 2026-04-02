using ims.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ims.Services.Interfaces;

public interface ISupplierService
{
    Task<IEnumerable<SupplierDto>> GetAllAsync();
    Task<SupplierDto?> GetByIdAsync(int id);
    Task<SupplierDto> AddAsync(SupplierCreateDto supplierDto);
    Task UpdateAsync(int id, SupplierUpdateDto supplierDto);
    Task DeleteAsync(int id);
}
