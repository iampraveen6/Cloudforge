using Azure.Storage.Queues;
using System.Text.Json;

namespace CloudForge.Infrastructure;

public class QueueService
{
    private readonly QueueClient _queueClient;

    public QueueService(string connectionString, string queueName = "provisioning-requests")
    {
        _queueClient = new QueueClient(connectionString, queueName);
        _queueClient.CreateIfNotExists();
    }

    public async Task SendMessageAsync<T>(T message)
    {
        var json = JsonSerializer.Serialize(message);
        await _queueClient.SendMessageAsync(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json)));
    }

    public async Task<T?> ReceiveMessageAsync<T>()
    {
        var response = await _queueClient.ReceiveMessageAsync();
        if (response.Value == null)
        {
            return default;
        }

        var json = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(response.Value.MessageText));
        return JsonSerializer.Deserialize<T>(json);
    }

    public async Task DeleteMessageAsync(string messageId, string popReceipt)
    {
        await _queueClient.DeleteMessageAsync(messageId, popReceipt);
    }
}
