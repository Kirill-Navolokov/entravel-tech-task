using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using OrderProcessingService.Config;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrderProcessingService.Services;

public class RabbitMessagingService : IMessagingService
{
    private readonly ConnectionFactory _rabbitConnectionFactory;

    private IConnection? _rabbitConnection;
    private IChannel? _rabbitChannel;

    public RabbitMessagingService(IOptions<RabbitMqOptions> options)
    {
        _rabbitConnectionFactory = new ConnectionFactory
        {
            Uri = new Uri(options.Value.HostName)
        };
    }

    public async Task PublishAsync<T>(string queue, T message)
    {
        var channel = await GetChannelAsync();
        await channel.QueueDeclareAsync(queue: queue, durable: false, exclusive: false, autoDelete: false);

        var messageJson = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(messageJson);
        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queue, body: body);
    }

    public async Task Subscribe<T>(string queue, Func<T, Task> handler)
    {
        var channel = await GetChannelAsync();
        await channel.QueueDeclareAsync(queue: queue, durable: false, exclusive: false, autoDelete: false);
        if (await channel.ConsumerCountAsync(queue) > 0)
            return;

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = Encoding.UTF8.GetString(ea.Body.ToArray());
            var message = JsonSerializer.Deserialize<T>(body)!;
            await handler(message);
        };

        await channel.BasicConsumeAsync(queue, autoAck: true, consumer: consumer);
    }

    private async Task<IChannel> GetChannelAsync()
    {
        if (_rabbitChannel == null)
        {
            _rabbitConnection ??= await _rabbitConnectionFactory.CreateConnectionAsync();
            _rabbitChannel = await _rabbitConnection.CreateChannelAsync();
        }
        
        return _rabbitChannel;
    }
}