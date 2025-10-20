using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderProcessingService.DAL.Entities;
using OrderProcessingService.DAL.Repos;

namespace OrderProcessingService.DAL;

public static class Extensions
{
    public static IServiceCollection AddDAL(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddPooledDbContextFactory<OrdersDbContext>(opt =>
        {
            opt
            .UseNpgsql(
                connectionString,
                o => o.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name))
            .UseSeeding((dbContext, _) => DbSeeder.Seed((OrdersDbContext)dbContext))
            .UseAsyncSeeding(async(dbContext, _, ct) => DbSeeder.Seed((OrdersDbContext)dbContext));
        });

        services.AddSingleton<IRepository<OrderEntity>, OrderRepository>();

        return services;
    }
}