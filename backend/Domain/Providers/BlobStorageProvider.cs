using Azure.Storage.Blobs;
using CloudForge.Domain.Models;

namespace CloudForge.Domain.Providers;

public class BlobStorageProvider : BaseResourceProvider
{
    public override ResourceType ResourceType => ResourceType.BlobStorage;

    public BlobStorageProvider(string endpoint, bool useFloci = true) : base(endpoint, useFloci)
    {
    }

    public override async Task<string> ProvisionAsync(ResourceRequest request)
    {
        var accountName = GenerateResourceName("blob", request.ApplicationId, request.EnvironmentId);
        
        // For Floci, skip actual Blob Storage operations
        if (_useFloci)
        {
            return accountName;
        }
        
        var connectionString = $"DefaultEndpointsProtocol=http;AccountName={accountName};AccountKey=key;BlobEndpoint={_endpoint}/{accountName};";
        var blobServiceClient = new BlobServiceClient(connectionString);
        
        // Create container
        var containerName = request.Properties.GetValueOrDefault("containerName", "default");
        await blobServiceClient.CreateBlobContainerAsync(containerName);
        
        return accountName;
    }

    public override async Task DeleteAsync(string resourceId)
    {
        // For Floci, skip actual Blob Storage operations
        if (_useFloci)
        {
            return;
        }
        
        var connectionString = $"DefaultEndpointsProtocol=http;AccountName={resourceId};AccountKey=key;BlobEndpoint={_endpoint}/{resourceId};";
        var blobServiceClient = new BlobServiceClient(connectionString);
        
        await foreach (var container in blobServiceClient.GetBlobContainersAsync())
        {
            await blobServiceClient.DeleteBlobContainerAsync(container.Name);
        }
    }

    public override Task<ResourceStatus> GetStatusAsync(string resourceId)
    {
        // For Floci, we assume it's ready if we can connect
        return Task.FromResult(ResourceStatus.Ready);
    }

    public override Task UpdateAsync(string resourceId, Dictionary<string, string> properties)
    {
        // Blob storage properties are typically immutable
        return Task.CompletedTask;
    }
}
