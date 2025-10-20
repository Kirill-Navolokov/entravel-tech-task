using System.Text.Json.Serialization;
using OrderProcessingService.Config;
using OrderProcessingService.Dtos;
using OrderProcessingService.Entities;
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

builder.Services.AddHostedService<OrderProcessor>();

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