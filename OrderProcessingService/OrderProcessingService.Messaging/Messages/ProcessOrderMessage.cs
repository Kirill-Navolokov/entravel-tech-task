namespace OrderProcessingService.Messaging.Messages;

public record ProcessOrderMessage(Guid OrderId);