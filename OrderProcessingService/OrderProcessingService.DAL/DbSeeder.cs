using OrderProcessingService.DAL.Entities;

namespace OrderProcessingService.DAL;

internal static class DbSeeder
{
    internal static void Seed(OrdersDbContext dbContext)
    {
        if (!dbContext.Users.Any())
        {
            dbContext.Users.Add(new UserEntity
            {
                Id = Guid.Parse("b0f0d377-ee86-4bd3-a45c-4f05237407ce"),
                Username = "entravel"
            });
            dbContext.SaveChanges();
        }
    }
}