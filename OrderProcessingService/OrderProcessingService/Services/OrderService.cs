using Microsoft.Extensions.Options;
using OrderProcessingService.Config;
using OrderProcessingService.Dtos;
using OrderProcessingService.Entities;
using OrderProcessingService.Mappers;
using OrderProcessingService.Messages;

namespace OrderProcessingService.Services;

public class OrderService(
    ILogger<OrderService> logger,
    IMapper<RequestOrderDto, ResponseOrderDto, OrderEntity> mapper,
    IMessagingService messagingService,
    IOptions<MessageQueueOptions> options) : IOrderService
{
    private readonly List<OrderEntity> _orders = new List<OrderEntity>();

    public async Task<ResponseOrderDto> AddAsync(RequestOrderDto order)
    {
        var orderEntity = mapper.Map(order);
        orderEntity.Id = Guid.NewGuid();
        orderEntity.Status = Enums.OrderStatus.Created;

        _orders.Add(orderEntity);

        logger.LogInformation("Order added");
        await messagingService.PublishAsync(
            options.Value.Queues[Constants.QueueNames.Orders],
            new ProcessOrderMessage(orderEntity.Id));

        return mapper.Map(orderEntity);
    }

    public Task<ResponseOrderDto?> GetAsync(Guid id)
    {
        var entity = _orders.FirstOrDefault(o => o.Id == id);
        var response = entity is null ? null : mapper.Map(entity);

        return Task.FromResult(response);
    }

    public async Task ProcessAsync(Guid id)
    {
        await Task.Delay(10000);

        var order = _orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
            return;

        order.Status = Enums.OrderStatus.Processed;
    }
}