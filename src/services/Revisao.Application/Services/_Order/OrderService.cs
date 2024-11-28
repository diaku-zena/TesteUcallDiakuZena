using AutoMapper;
using Revisao.Application.DTOs;
using Revisao.Application.Services.Generic;
using Revisao.Domain.Entities;
using Revisao.Domain.Shared.Enums;
using Revisao.Domain.Interfaces;

namespace Revisao.Application.Services._Order;

public class OrderService : GenericService<Order, CreateOrderRequest>, IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMessageQueue _messageQueue;

    public OrderService(IOrderRepository repository, IMessageQueue messageQueue, IMapper mapper) : base(repository, mapper)
    {
        _repository = repository;
        _messageQueue = messageQueue;
        _mapper = mapper;
    }

    public async Task<Guid> RegisterOrderAsync(CreateOrderRequest request)
    {
        var order = _mapper.Map<Order>(request);
        var orderId = await _repository.AddOrderAsync(order);

        // Enviar para a fila de processamento
        await _messageQueue.SendMessageAsync("orders-queue", new { OrderId = orderId });

        return orderId;
    }

    public async Task<OrderResponse> GetOrderStatusAsync(Guid orderId)
    {
        var order = await _repository.GetOrderByIdAsync(orderId);
        if (order == null)
            throw new Exception("Pedido não encontrado");

        return _mapper.Map<OrderResponse>(order);
    }

    public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync()
    {
        var orders = await _repository.GetAllOrdersAsync();
        return _mapper.Map<IEnumerable<OrderResponse>>(orders);
    }

    public async Task ProcessOrderAsync(Guid orderId)
    {
        // Simula o processamento do pedido
        var order = await _repository.GetOrderByIdAsync(orderId);
        if (order == null)
            throw new Exception("Pedido não encontrado");

        order.Status = OrderStatusEnum.Processing;
        await _repository.UpdateOrderStatusAsync(orderId, OrderStatusEnum.Processing);

        // Simula um delay de processamento
        await Task.Delay(5000);

        // Atualiza para concluído
        await _repository.UpdateOrderStatusAsync(orderId, OrderStatusEnum.Completed);
    }
}
