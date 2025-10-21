using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Prometheus;

namespace OrderProcessingService.Workers;

public class MetricsHostedService(IConfiguration configuration) : IHostedService
{
    private KestrelMetricServer? _server;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var port = int.TryParse(configuration.GetValue<string>("Metrics__Port"), out var p) ? p : 9100;

        _server = new KestrelMetricServer(port: port);
        _server.Start();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _server?.Stop();
    }
}