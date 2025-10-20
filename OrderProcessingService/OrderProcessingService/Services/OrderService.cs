using Microsoft.Extensions.Options;
using OrderProcessingService.DAL.Entities;
using OrderProcessingService.DAL.Repos;
using OrderProcessingService.Dtos;
using OrderProcessingService.Mappers;
using OrderProcessingService.Messaging.Config;
using OrderProcessingService.Messaging.Messages;
using OrderProcessingService.Messaging.Services;

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
}