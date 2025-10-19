using OrderProcessingService.Enums;

namespace OrderProcessingService.Dtos;

public record ResponseOrderDto(
    Guid Id,
    Guid CustomerId,
    OrderStatus Status,
    double TotalAmount,
    IEnumerable<OrderItemDto> Items);