using AutoMapper;
using ims.DTO;
using ims.Models;
using ims.Repository.Interfaces;
using ims.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ims.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly IMapper _mapper;

    public SupplierService(ISupplierRepository supplierRepository, IMapper mapper)
    {
        _supplierRepository = supplierRepository;
        _mapper = mapper;
    }
    public async Task<SupplierDto> AddAsync(SupplierCreateDto supplierDto)
    {
        var supplier = _mapper.Map<Supplier>(supplierDto);
        await _supplierRepository.AddAsync(supplier);
        return _mapper.Map<SupplierDto>(supplier);
    }

    public async Task DeleteAsync(int id)
    {
        await _supplierRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<SupplierDto>> GetAllAsync()
    {
        var suppliers = await _supplierRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<SupplierDto>>(suppliers);
    }

    public async Task<SupplierDto?> GetByIdAsync(int id)
    {
        var supplier = await _supplierRepository.GetByIdAsync(id);
        return _mapper.Map<SupplierDto?>(supplier);
    }

    public async Task UpdateAsync(int id, SupplierUpdateDto supplierDto)
    {
        var supplier = await _supplierRepository.GetByIdAsync(id);
        if (supplier == null) return;

        _mapper.Map(supplierDto, supplier);
        await _supplierRepository.UpdateAsync(supplier);
    }
}
