namespace OrderProcessingService.DAL.Entities;

public class OrderItemEntity
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid OrderId { get; set; }
    public double PricePerItem { get; set; }
    public double Quantity { get; set; }
    public OrderEntity Order { get; set; }
}