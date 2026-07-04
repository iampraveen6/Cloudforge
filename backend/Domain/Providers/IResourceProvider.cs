using CloudForge.Domain.Models;

namespace CloudForge.Domain.Providers;

public interface IResourceProvider
{
    ResourceType ResourceType { get; }
    
    /// <summary>
    /// Provision a new Azure resource
    /// </summary>
    Task<string> ProvisionAsync(ResourceRequest request);
    
    /// <summary>
    /// Delete an existing Azure resource
    /// </summary>
    Task DeleteAsync(string resourceId);
    
    /// <summary>
    /// Get the current status of a resource
    /// </summary>
    Task<ResourceStatus> GetStatusAsync(string resourceId);
    
    /// <summary>
    /// Update resource properties
    /// </summary>
    Task UpdateAsync(string resourceId, Dictionary<string, string> properties);
}

public class ResourceRequest
{
    public string Name { get; set; } = string.Empty;
    public string ApplicationId { get; set; } = string.Empty;
    public string EnvironmentId { get; set; } = string.Empty;
    public ResourceType ResourceType { get; set; }
    public Dictionary<string, string> Properties { get; set; } = new();
    public Dictionary<string, string> Tags { get; set; } = new();
}
