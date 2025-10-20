namespace OrderProcessingService.DAL.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public List<OrderEntity> Orders { get; set; }
}