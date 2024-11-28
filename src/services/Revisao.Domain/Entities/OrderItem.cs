using Revisao.Domain.Shared.Entities;

namespace Revisao.Domain.Entities;

public class OrderItem : BaseEntity
{
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public Guid OrderId { get; set; }
}
