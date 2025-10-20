using Microsoft.EntityFrameworkCore;
using OrderProcessingService.DAL.Entities;

namespace OrderProcessingService.DAL.Repos;

public class OrderRepository(IDbContextFactory<OrdersDbContext> dbContextFactory) : IRepository<OrderEntity>
{
    public async Task<OrderEntity> AddAsync(OrderEntity entity)
    {
        using var dbContext = GetContext();
        var addedEntry = await dbContext.Orders.AddAsync(entity);
        await dbContext.SaveChangesAsync();

        return addedEntry.Entity;
    }

    public async Task<OrderEntity?> GetAsync(Guid id)
    {
        using var dbContext = GetContext();
        var order = await dbContext.Orders.AsNoTracking()
            .Include(o => o.Items)
            .SingleOrDefaultAsync(o => o.Id == id);

        return order;
    }

    public async Task UpdateAsync(OrderEntity entity)
    {
        using var dbContext = GetContext();
        dbContext.Orders.Update(entity);

        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<OrderEntity>> GetByCustomer(Guid customerId)
    {
        using var dbContext = GetContext();
        var items = dbContext.Orders.Where(o => o.CustomerId == customerId)
            .Include(o => o.Items)
            .ToList();

        return items;
    }

    private OrdersDbContext GetContext()
    {
        return dbContextFactory.CreateDbContext();
    }
}