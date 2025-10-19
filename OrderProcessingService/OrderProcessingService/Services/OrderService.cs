using OrderProcessingService.Dtos;
using OrderProcessingService.Entities;
using OrderProcessingService.Mappers;

namespace OrderProcessingService.Services;

public class OrderService(
    ILogger<OrderService> logger,
    IMapper<RequestOrderDto, ResponseOrderDto, OrderEntity> mapper) : IOrderService
{
    private readonly List<OrderEntity> _orders = new List<OrderEntity>();

    public Task<ResponseOrderDto> AddAsync(RequestOrderDto order)
    {
        var orderEntity = mapper.Map(order);
        orderEntity.Id = Guid.NewGuid();
        orderEntity.Status = Enums.OrderStatus.Created;

        _orders.Add(orderEntity);

        logger.LogInformation("Order added");

        return Task.FromResult(mapper.Map(orderEntity));
    }

    public Task<ResponseOrderDto?> GetAsync(Guid id)
    {
        var entity = _orders.FirstOrDefault(o => o.Id == id);
        var response = entity is null ? null : mapper.Map(entity);

        return Task.FromResult(response);
    }
}