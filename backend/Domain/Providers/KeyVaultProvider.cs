using Azure.Security.KeyVault.Secrets;
using CloudForge.Domain.Models;

namespace CloudForge.Domain.Providers;

public class KeyVaultProvider : BaseResourceProvider
{
    public override ResourceType ResourceType => ResourceType.KeyVault;

    public KeyVaultProvider(string endpoint, bool useFloci = true) : base(endpoint, useFloci)
    {
    }

    private SecretClient? CreateClient(Uri vaultUri)
    {
        if (_useFloci)
        {
            return null;
        }
        else
        {
            return new SecretClient(vaultUri, _credential!);
        }
    }

    public override async Task<string> ProvisionAsync(ResourceRequest request)
    {
        var vaultName = GenerateResourceName("kv", request.ApplicationId, request.EnvironmentId);
        
        if (_useFloci)
        {
            return vaultName;
        }
        
        var vaultUri = new Uri($"{_endpoint}/{vaultName}");
        var client = CreateClient(vaultUri);
        
        if (client != null && request.Properties.TryGetValue("initialSecret", out var secretValue))
        {
            await client.SetSecretAsync("initial-secret", secretValue);
        }
        
        return vaultName;
    }

    public override async Task DeleteAsync(string resourceId)
    {
        if (_useFloci)
        {
            return;
        }
        
        var vaultUri = new Uri($"{_endpoint}/{resourceId}");
        var client = CreateClient(vaultUri);
        
        if (client != null)
        {
            await foreach (var secret in client.GetPropertiesOfSecretsAsync())
            {
                await client.StartDeleteSecretAsync(secret.Name);
            }
        }
    }

    public override async Task<ResourceStatus> GetStatusAsync(string resourceId)
    {
        if (_useFloci)
        {
            return ResourceStatus.Ready;
        }
        
        var vaultUri = new Uri($"{_endpoint}/{resourceId}");
        var client = CreateClient(vaultUri);
        
        if (client != null)
        {
            var iterator = client.GetPropertiesOfSecretsAsync().GetAsyncEnumerator();
            await iterator.MoveNextAsync();
            return ResourceStatus.Ready;
        }
        
        return ResourceStatus.Error;
    }

    public override async Task UpdateAsync(string resourceId, Dictionary<string, string> properties)
    {
        if (_useFloci)
        {
            return;
        }
        
        var vaultUri = new Uri($"{_endpoint}/{resourceId}");
        var client = CreateClient(vaultUri);
        
        if (client != null)
        {
            foreach (var property in properties)
            {
                if (property.Key.StartsWith("secret-"))
                {
                    var secretName = property.Key.Substring(7);
                    await client.SetSecretAsync(secretName, property.Value);
                }
            }
        }
    }
}
