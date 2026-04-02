using AutoMapper;
using ims.DTO;
using ims.Models;
using ims.Repository.Interfaces;
using ims.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ims.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public OrderService(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<OrderDto> AddAsync(OrderCreateDto orderDto)
    {
        var order = _mapper.Map<Order>(orderDto);
        order.OrderDate = DateTime.UtcNow;

        await _orderRepository.AddAsync(order);

        return _mapper.Map<OrderDto>(order);
    }

    public async Task DeleteAsync(int id)
    {
        await _orderRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<OrderDto>> GetAllAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task<OrderDto?> GetByIdAsync(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        return _mapper.Map<OrderDto?>(order);
    }

    public async Task UpdateAsync(int id, OrderUpdateDto orderDto)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null) return;

        _mapper.Map(orderDto, order);
        await _orderRepository.UpdateAsync(order);
    }

    public async Task<IEnumerable<SupplierOrderReportDto>> GetSupplierOrderReportAsync()
    {
        return await _orderRepository.GetSupplierOrderReportAsync();
    }
}
