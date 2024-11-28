using Revisao.Application.DTOs;
using Revisao.Application.Services.Generic;

namespace Revisao.Application.Services._Order;

public interface IOrderService : IGenericService<CreateOrderRequest>
{
    Task<Guid> RegisterOrderAsync(CreateOrderRequest request);
    Task<OrderResponse> GetOrderStatusAsync(Guid orderId);
    Task<IEnumerable<OrderResponse>> GetAllOrdersAsync();
    Task ProcessOrderAsync(Guid orderId);
}
