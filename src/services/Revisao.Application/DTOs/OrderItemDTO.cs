using Revisao.Application.ApplicationValueObjects;
using Revisao.Domain.Entities;

namespace Revisao.Application.DTOs;

public class OrderItemDTO : BaseEntityDTO
{
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
