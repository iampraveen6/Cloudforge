namespace CloudForge.Infrastructure;

public class Settings
{
    public const string SectionName = "Settings";

    public CosmosDbSettings CosmosDb { get; set; } = new();
    public AzureSettings Azure { get; set; } = new();
    public FlociSettings Floci { get; set; } = new();
}

public class CosmosDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = "CloudForge";
}

public class AzureSettings
{
    public string SubscriptionId { get; set; } = string.Empty;
    public string ResourceGroupName { get; set; } = string.Empty;
    public string Location { get; set; } = "eastus";
}

public class FlociSettings
{
    public string Endpoint { get; set; } = "http://localhost:4566";
    public bool UseFloci { get; set; } = true;
}
