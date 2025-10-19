namespace OrderProcessingService.Dtos;

public record OrderItemDto(Guid ProductId, double PricePerItem, double Quantity);