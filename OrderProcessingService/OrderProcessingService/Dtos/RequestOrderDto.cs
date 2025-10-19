namespace OrderProcessingService.Dtos;

public record RequestOrderDto(Guid CustomerId, double TotalAmount, IEnumerable<OrderItemDto> Items);