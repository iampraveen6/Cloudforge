namespace CloudForge.Domain.Models;

public class Resource
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string ApplicationId { get; set; } = string.Empty;
    public string EnvironmentId { get; set; } = string.Empty;
    public ResourceType Type { get; set; }
    public ResourceStatus Status { get; set; } = ResourceStatus.Provisioning;
    public Dictionary<string, string> Properties { get; set; } = new();
    public string AzureResourceId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public enum ResourceType
{
    BlobStorage,
    KeyVault,
    AppConfiguration,
    ContainerApps,
    ContainerRegistry,
    CosmosDB,
    QueueStorage
}

public enum ResourceStatus
{
    Provisioning,
    Ready,
    Error,
    Deleting
}
