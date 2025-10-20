using Microsoft.Extensions.Options;
using OrderProcessingService.Config;
using OrderProcessingService.Messages;
using OrderProcessingService.Services;

namespace OrderProcessingService.Workers;

public class OrderProcessor(
    IMessagingService messagingService,
    IOrderService orderService,
    IOptions<MessageQueueOptions> options) : BackgroundService
{
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await base.StartAsync(cancellationToken);

        await messagingService.Subscribe<ProcessOrderMessage>(
            options.Value.Queues[Constants.QueueNames.Orders],
            (message) => orderService.ProcessAsync(message.OrderId));
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //Nothing to do here. We just need to subscrube for new messages from Message broker
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        //Unsubscribe from orders processing here if needed

        await base.StopAsync(cancellationToken);
    }
}