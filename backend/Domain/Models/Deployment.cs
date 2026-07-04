namespace CloudForge.Domain.Models;

public class Deployment
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ApplicationId { get; set; } = string.Empty;
    public string EnvironmentId { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public DeploymentStatus Status { get; set; } = DeploymentStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public string? ErrorMessage { get; set; }
    public Dictionary<string, string> Metadata { get; set; } = new();
}

public enum DeploymentStatus
{
    Pending,
    Deploying,
    Success,
    Failed,
    RollingBack
}
