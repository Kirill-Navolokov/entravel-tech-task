using OrderProcessingService.Dtos;

namespace OrderProcessingService.Services;

public interface IOrderService
{
    Task<ResponseOrderDto> AddAsync(RequestOrderDto order);

    Task<ResponseOrderDto?> GetAsync(Guid id);

    Task<IEnumerable<ResponseOrderDto>> GetByCustomer(Guid customerId);

    Task ProcessAsync(Guid id);
}