using Azure.Identity;
using CloudForge.Domain.Models;

namespace CloudForge.Domain.Providers;

public abstract class BaseResourceProvider : IResourceProvider
{
    protected readonly string _endpoint;
    protected readonly DefaultAzureCredential? _credential;
    protected readonly bool _useFloci;

    protected BaseResourceProvider(string endpoint, bool useFloci = true)
    {
        _endpoint = endpoint;
        _useFloci = useFloci;
        // Only create credential if not using Floci
        if (!useFloci)
        {
            _credential = new DefaultAzureCredential();
        }
    }

    public abstract ResourceType ResourceType { get; }
    
    public abstract Task<string> ProvisionAsync(ResourceRequest request);
    
    public abstract Task DeleteAsync(string resourceId);
    
    public abstract Task<ResourceStatus> GetStatusAsync(string resourceId);
    
    public abstract Task UpdateAsync(string resourceId, Dictionary<string, string> properties);

    protected string GenerateResourceName(string prefix, string applicationId, string environmentId)
    {
        var appIdPart = applicationId.Length > 8 ? applicationId.Substring(0, 8) : applicationId;
        var envIdPart = environmentId.Length > 8 ? environmentId.Substring(0, 8) : environmentId;
        return $"{prefix}-{appIdPart}-{envIdPart}";
    }
}
