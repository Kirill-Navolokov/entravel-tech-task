using System.Text.Json.Serialization;
using OrderProcessingService.Dtos;
using OrderProcessingService.Entities;
using OrderProcessingService.Mappers;
using OrderProcessingService.Middleware;
using OrderProcessingService.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddSingleton<IOrderService, OrderService>()
    .AddSingleton<IMapper<RequestOrderDto, ResponseOrderDto, OrderEntity>, OrderMapper>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();


app.UseHttpsRedirection();
app.MapControllers();

app.Run();