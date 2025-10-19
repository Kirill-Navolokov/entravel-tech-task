namespace OrderProcessingService.Mappers;

public interface IMapper<RequestDto, ResponseDto, Entity>
{
    Entity Map(RequestDto requestDto);

    ResponseDto Map(Entity entity);
}