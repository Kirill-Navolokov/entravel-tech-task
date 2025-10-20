using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderProcessingService.DAL;
using OrderProcessingService.Messaging;
using OrderProcessingService.Workers;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddDAL(builder.Configuration.GetConnectionString("OrdersDb")!);
builder.Services.AddMessaging(builder.Configuration);

builder.Services.AddHostedService<OrderProcessor>();

var host = builder.Build();
await host.RunAsync();