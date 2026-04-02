using ims.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ims.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync();
    Task<ProductDto?> GetByIdAsync(int id);
    Task<ProductDto> AddAsync(ProductCreateDto productDto);
    Task UpdateAsync(int id, ProductUpdateDto productDto);
    Task DeleteAsync(int id);
}
