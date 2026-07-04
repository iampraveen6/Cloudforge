namespace CloudForge.Domain.Models;

public class Application
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string TeamId { get; set; } = string.Empty;
    public string RepositoryUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Created;
    public List<Environment> Environments { get; set; } = new();
    public List<Resource> Resources { get; set; } = new();
}

public enum ApplicationStatus
{
    Created,
    Configuring,
    Ready,
    Error
}
