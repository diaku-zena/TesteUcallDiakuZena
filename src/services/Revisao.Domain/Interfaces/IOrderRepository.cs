using Revisao.Domain.Entities;
using Revisao.Domain.Shared.Enums;

namespace Revisao.Domain.Interfaces;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<Guid> AddOrderAsync(Order order);
    Task<Order> GetOrderByIdAsync(Guid orderId);
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task UpdateOrderStatusAsync(Guid orderId, OrderStatusEnum status);
}
