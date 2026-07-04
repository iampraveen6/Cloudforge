using CloudForge.Domain.Models;
using CloudForge.Domain.Providers;

namespace CloudForge.Infrastructure;

public class ProvisioningEngine
{
    private readonly Dictionary<ResourceType, IResourceProvider> _providers;
    private readonly ICosmosDbService _cosmosDbService;

    public ProvisioningEngine(
        IEnumerable<IResourceProvider> providers,
        ICosmosDbService cosmosDbService)
    {
        _providers = providers.ToDictionary(p => p.ResourceType);
        _cosmosDbService = cosmosDbService;
    }

    public async Task<Resource> ProvisionResourceAsync(ResourceRequest request)
    {
        // Create resource record in Cosmos DB
        var resource = new Resource
        {
            Name = request.Name,
            ApplicationId = request.ApplicationId,
            EnvironmentId = request.EnvironmentId,
            Type = request.ResourceType,
            Properties = request.Properties,
            Status = ResourceStatus.Provisioning
        };

        resource = await _cosmosDbService.CreateResourceAsync(resource);

        try
        {
            // Get appropriate provider
            if (!_providers.ContainsKey(request.ResourceType))
            {
                throw new InvalidOperationException($"No provider found for resource type: {request.ResourceType}");
            }

            var provider = _providers[request.ResourceType];
            
            // Provision the resource
            var azureResourceId = await provider.ProvisionAsync(request);
            
            // Update resource with Azure resource ID
            resource.AzureResourceId = azureResourceId;
            resource.Status = ResourceStatus.Ready;
            resource.UpdatedAt = DateTime.UtcNow;
            
            await _cosmosDbService.UpdateResourceAsync(resource);
            
            return resource;
        }
        catch (Exception ex)
        {
            // Update resource status to error
            resource.Status = ResourceStatus.Error;
            resource.Properties["error"] = ex.Message;
            resource.UpdatedAt = DateTime.UtcNow;
            
            await _cosmosDbService.UpdateResourceAsync(resource);
            
            throw;
        }
    }

    public async Task DeleteResourceAsync(string resourceId)
    {
        var resource = await _cosmosDbService.GetResourceAsync(resourceId);
        if (resource == null)
        {
            throw new InvalidOperationException($"Resource not found: {resourceId}");
        }

        if (!_providers.ContainsKey(resource.Type))
        {
            throw new InvalidOperationException($"No provider found for resource type: {resource.Type}");
        }

        var provider = _providers[resource.Type];
        await provider.DeleteAsync(resource.AzureResourceId);
        
        // Update resource status
        resource.Status = ResourceStatus.Deleting;
        resource.UpdatedAt = DateTime.UtcNow;
        await _cosmosDbService.UpdateResourceAsync(resource);
    }

    public async Task<ResourceStatus> GetResourceStatusAsync(string resourceId)
    {
        var resource = await _cosmosDbService.GetResourceAsync(resourceId);
        if (resource == null)
        {
            throw new InvalidOperationException($"Resource not found: {resourceId}");
        }

        if (!_providers.ContainsKey(resource.Type))
        {
            return resource.Status;
        }

        var provider = _providers[resource.Type];
        var status = await provider.GetStatusAsync(resource.AzureResourceId);
        
        // Update status if changed
        if (status != resource.Status)
        {
            resource.Status = status;
            resource.UpdatedAt = DateTime.UtcNow;
            await _cosmosDbService.UpdateResourceAsync(resource);
        }

        return status;
    }
}
