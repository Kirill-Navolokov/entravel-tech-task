using OrderProcessingService.Enums;

namespace OrderProcessingService.Entities;

public class OrderEntity
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public double TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public List<OrderItemEntity> Items { get; set; }
}