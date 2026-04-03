using AutoMapper;
using ims.DTO;
using ims.Models;
using ims.Repository.Interfaces;
using ims.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ims.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ProductDto> AddAsync(ProductCreateDto productDto)
    {
        var product = _mapper.Map<Product>(productDto);
        await _productRepository.AddAsync(product);
        return _mapper.Map<ProductDto>(product);
    }

    public async Task DeleteAsync(int id)
    {
        await _productRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return _mapper.Map<ProductDto?>(product);
    }

    public async Task UpdateAsync(int id, ProductUpdateDto productDto)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return;

        _mapper.Map(productDto, product);
        await _productRepository.UpdateAsync(product);
    }

    public async Task<IEnumerable<ProductDto>> GetLowStockProductsAsync(int threshold)
    {
        var products = await _productRepository.GetLowStockProductsAsync(threshold);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }
}
