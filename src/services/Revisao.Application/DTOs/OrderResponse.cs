namespace Revisao.Application.DTOs;

public class OrderResponse
{
    public Guid Id { get; set; }
    public string CustomerId { get; set; }
    public decimal TotalValue { get; set; }
    public List<OrderItemDTO>? Items { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

