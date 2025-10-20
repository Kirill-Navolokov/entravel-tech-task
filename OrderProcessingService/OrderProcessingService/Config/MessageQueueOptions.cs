namespace OrderProcessingService.Config;

public class MessageQueueOptions
{
    public required Dictionary<string, string> Queues { get; set; }
}