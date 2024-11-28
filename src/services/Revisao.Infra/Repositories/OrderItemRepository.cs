using Revisao.Domain.Entities;
using Revisao.Domain.Interfaces;
using Revisao.Infra.Context;

namespace Revisao.Infra.Repositories;

public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
{
    private readonly RevisaoContext _context;
    public OrderItemRepository(RevisaoContext context) : base(context)
    {
        _context = context;
    }
}
