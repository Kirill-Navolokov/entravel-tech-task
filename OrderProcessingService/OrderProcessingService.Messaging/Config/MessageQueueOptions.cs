namespace OrderProcessingService.Messaging.Config;

public class MessageQueueOptions
{
    public required Dictionary<string, string> Queues { get; set; }
}