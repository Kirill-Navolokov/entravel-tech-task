using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using OrderProcessingService.DAL;
using OrderProcessingService.DAL.Entities;
using OrderProcessingService.Dtos;
using OrderProcessingService.Mappers;
using OrderProcessingService.Messaging;
using OrderProcessingService.Middleware;
using OrderProcessingService.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("appsettings.json", false)
    .AddEnvironmentVariables();

builder.Services.AddSingleton<IOrderService, OrderService>()
    .AddSingleton<IMapper<RequestOrderDto, ResponseOrderDto, OrderEntity>, OrderMapper>();

builder.Services.AddDAL(builder.Configuration.GetConnectionString("OrdersDb")!);
builder.Services.AddMessaging(builder.Configuration);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

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