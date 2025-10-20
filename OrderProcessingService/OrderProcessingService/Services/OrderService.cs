using Microsoft.Extensions.Options;
using OrderProcessingService.Config;
using OrderProcessingService.DAL.Entities;
using OrderProcessingService.DAL.Repos;
using OrderProcessingService.Dtos;
using OrderProcessingService.Mappers;
using OrderProcessingService.Messages;

namespace OrderProcessingService.Services;

public class OrderService(
    ILogger<OrderService> logger,
    IMapper<RequestOrderDto, ResponseOrderDto, OrderEntity> mapper,
    IMessagingService messagingService,
    IOptions<MessageQueueOptions> options,
    IRepository<OrderEntity> orderRepo) : IOrderService
{
    public async Task<ResponseOrderDto> AddAsync(RequestOrderDto order)
    {
        var orderEntity = mapper.Map(order);
        orderEntity.Id = Guid.NewGuid();
        orderEntity.Status = DAL.Enums.OrderStatus.Created;

        orderEntity = await orderRepo.AddAsync(orderEntity);

        logger.LogInformation("Order added");
        await messagingService.PublishAsync(
            options.Value.Queues[Constants.QueueNames.Orders],
            new ProcessOrderMessage(orderEntity.Id));

        return mapper.Map(orderEntity);
    }

    public async Task<ResponseOrderDto?> GetAsync(Guid id)
    {
        var entity = await orderRepo.GetAsync(id);
        var response = entity is null ? null : mapper.Map(entity);

        return response;
    }

    public async Task<IEnumerable<ResponseOrderDto>> GetByCustomer(Guid customerId)
    {
        var entities = await orderRepo.GetByCustomer(customerId);

        return entities.Select(e => mapper.Map(e));
    }

    public async Task ProcessAsync(Guid id)
    {
        await Task.Delay(10000);

        var order = await orderRepo.GetAsync(id);
        if (order == null)
            return;

        order.Status = DAL.Enums.OrderStatus.Processed;

        await orderRepo.UpdateAsync(order);
    }
}