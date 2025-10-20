namespace OrderProcessingService.Messaging.Services;

public interface IMessagingService
{
    Task PublishAsync<T>(string queue, T message);

    Task Subscribe<T>(string queue, Func<T, Task> handler);
}