using System.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using OrderProcessingService.DAL.Entities;
using OrderProcessingService.DAL.Repos;
using OrderProcessingService.Messaging.Config;
using OrderProcessingService.Messaging.Messages;
using OrderProcessingService.Messaging.Services;
using Prometheus;

namespace OrderProcessingService.Workers;

public class OrderProcessor(
    IMessagingService messagingService,
    IRepository<OrderEntity> orderRepo,
    IOptions<MessageQueueOptions> options) : BackgroundService
{
    private static readonly Counter MessagesProcessed = Metrics.CreateCounter("processed_order_amount", "Total ProcessOrderMessage messages handled");

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await base.StartAsync(cancellationToken);

        await messagingService.Subscribe<ProcessOrderMessage>(
            options.Value.Queues[Constants.QueueNames.Orders],
            (message) => ProcessOrderAsync(message.OrderId));
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

    public async Task ProcessOrderAsync(Guid id)
    {
        await Task.Delay(10000);

        var order = await orderRepo.GetAsync(id);
        if (order == null)
            return;

        if (order.TotalAmount > 500)
            order.TotalAmount *= 0.9;

        order.Status = DAL.Enums.OrderStatus.Processed;

        await orderRepo.UpdateAsync(order);

        MessagesProcessed.Inc();
    }
}