using AutoMapper;
using Revisao.Application.DTOs;
using Revisao.Application.Services.Generic;
using Revisao.Domain.Entities;
using Revisao.Domain.Interfaces;

namespace Revisao.Application.Services._OrderItem;

public class OrderItemService : GenericService<OrderItem, OrderItemDTO>, IOrderItemService
{
    private readonly IOrderItemRepository _departmentRepository;
    private readonly IMapper _mapper;
    public OrderItemService(IOrderItemRepository orderItemRepository, IMapper mapper) : base(orderItemRepository, mapper)
    {
        _departmentRepository = orderItemRepository;
        _mapper = mapper;
    }
}
