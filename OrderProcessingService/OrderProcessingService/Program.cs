using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using OrderProcessingService.Config;
using OrderProcessingService.DAL;
using OrderProcessingService.DAL.Entities;
using OrderProcessingService.Dtos;
using OrderProcessingService.Mappers;
using OrderProcessingService.Middleware;
using OrderProcessingService.Services;
using OrderProcessingService.Workers;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables()
    .AddJsonFile("appsettings.json", false);

builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMQ"))
    .Configure<MessageQueueOptions>(builder.Configuration.GetSection("MessageQueues"));

builder.Services.AddSingleton<IOrderService, OrderService>()
    .AddSingleton<IMessagingService, RabbitMessagingService>()
    .AddSingleton<IMapper<RequestOrderDto, ResponseOrderDto, OrderEntity>, OrderMapper>();

builder.Services.AddDAL(builder.Configuration.GetConnectionString("OrdersDb")!);

builder.Services.AddHostedService<OrderProcessor>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<OrdersDbContext>>();
    using var context = factory.CreateDbContext();
    await context.Database.MigrateAsync();
}


app.UseHttpsRedirection();
app.MapControllers();

app.Run();