namespace CloudForge.Domain.Models;

public class Environment
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string ApplicationId { get; set; } = string.Empty;
    public EnvironmentType Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<Resource> Resources { get; set; } = new();
    public List<Deployment> Deployments { get; set; } = new();
}

public enum EnvironmentType
{
    Development,
    Staging,
    Production
}
