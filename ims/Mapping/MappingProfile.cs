using AutoMapper;
using ims.DTO;
using ims.Models;

namespace ims.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Product Mappings
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<ProductCreateDto, Product>();
        CreateMap<ProductUpdateDto, Product>();

        // Supplier Mappings
        CreateMap<Supplier, SupplierDto>().ReverseMap();
        CreateMap<SupplierCreateDto, Supplier>();
        CreateMap<SupplierUpdateDto, Supplier>();

        // User Mappings
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<RegisterDto, User>();
        CreateMap<UserUpdateDto, User>();
        CreateMap<User, AuthResponseDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

        // Order Mappings
        CreateMap<Order, OrderDto>().ReverseMap();
        CreateMap<OrderCreateDto, Order>();
        CreateMap<OrderUpdateDto, Order>();

        // OrderItem Mappings
        CreateMap<OrderItem, OrderItemDto>().ReverseMap();
        CreateMap<OrderItemCreateDto, OrderItem>();
    }
}
