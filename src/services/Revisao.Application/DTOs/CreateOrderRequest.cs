using Revisao.Application.ApplicationValueObjects;
using Revisao.Domain.Entities;

namespace Revisao.Application.DTOs;

public class CreateOrderRequest : BaseEntityDTO
{
    public string CustomerId { get; set; } = string.Empty;
    public List<OrderItemDTO>? Items { get; set; }
    public decimal TotalValue { get; set; }
}
