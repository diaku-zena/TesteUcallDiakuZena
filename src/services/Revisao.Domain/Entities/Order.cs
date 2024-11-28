using Revisao.Domain.Shared.Entities;
using Revisao.Domain.Shared.Enums;

namespace Revisao.Domain.Entities;


public class Order: BaseEntity
{
    public string CustomerId { get; set; }
    public decimal TotalValue { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public OrderStatusEnum Status { get; set; } = OrderStatusEnum.Pending;
}

