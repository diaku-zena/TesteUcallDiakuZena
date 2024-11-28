using Revisao.Domain.Entities;
using Revisao.Domain.Interfaces;
using Revisao.Domain.Shared.Enums;
using Revisao.Infra.Context;
using Microsoft.EntityFrameworkCore;


namespace Revisao.Infra.Repositories;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    private readonly RevisaoContext _context;
    public OrderRepository(RevisaoContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Guid> AddOrderAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order.Id;
    }

    public async Task<Order> GetOrderByIdAsync(Guid orderId)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.Items)
            .ToListAsync();
    }

    public async Task UpdateOrderStatusAsync(Guid orderId, OrderStatusEnum status)
    {
        var order = await GetOrderByIdAsync(orderId);
        if (order != null)
        {
            order.Status = status;
            await _context.SaveChangesAsync();
        }
    }
}
