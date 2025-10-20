using OrderProcessingService.DAL.Entities;
using OrderProcessingService.Dtos;

namespace OrderProcessingService.Mappers;

public class OrderMapper : IMapper<RequestOrderDto, ResponseOrderDto, OrderEntity>
{
    public OrderEntity Map(RequestOrderDto requestDto)
    {
        var items = new List<OrderItemEntity>();
        foreach (var item in requestDto.Items)
            items.Add(new OrderItemEntity
            {
                ProductId = item.ProductId,
                PricePerItem = item.PricePerItem,
                Quantity = item.Quantity,
            });

        var entity = new OrderEntity
        {
            CustomerId = requestDto.CustomerId,
            TotalAmount = requestDto.TotalAmount,
            Items = items
        };

        return entity;
    }

    public ResponseOrderDto Map(OrderEntity entity)
    {
        var items = new List<OrderItemDto>();
        foreach (var item in entity.Items)
            items.Add(new OrderItemDto(item.ProductId, item.PricePerItem, item.Quantity));

        var response = new ResponseOrderDto(
            entity.Id,
            entity.CustomerId,
            entity.Status,
            entity.TotalAmount,
            items);

        return response;
    }
}