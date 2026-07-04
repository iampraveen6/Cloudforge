using CloudForge.Domain.Models;

namespace CloudForge.Infrastructure;

public class InMemoryCosmosDbService : ICosmosDbService
{
    private readonly Dictionary<string, Application> _applications = new();
    private readonly Dictionary<string, Resource> _resources = new();
    private readonly Dictionary<string, Deployment> _deployments = new();
    private readonly Dictionary<string, Team> _teams = new();

    public Task<Application> CreateApplicationAsync(Application application)
    {
        _applications[application.Id] = application;
        return Task.FromResult(application);
    }

    public Task<Application?> GetApplicationAsync(string id)
    {
        _applications.TryGetValue(id, out var application);
        return Task.FromResult(application);
    }

    public Task<List<Application>> GetApplicationsByTeamAsync(string teamId)
    {
        var results = _applications.Values.Where(a => a.TeamId == teamId).ToList();
        return Task.FromResult(results);
    }

    public Task<List<Application>> GetAllApplicationsAsync()
    {
        var results = _applications.Values.ToList();
        return Task.FromResult(results);
    }

    public Task DeleteApplicationAsync(string id)
    {
        _applications.Remove(id);
        return Task.CompletedTask;
    }

    public Task<Resource> CreateResourceAsync(Resource resource)
    {
        _resources[resource.Id] = resource;
        return Task.FromResult(resource);
    }

    public Task<Resource?> GetResourceAsync(string id)
    {
        _resources.TryGetValue(id, out var resource);
        return Task.FromResult(resource);
    }

    public Task<List<Resource>> GetResourcesByApplicationAsync(string applicationId)
    {
        var results = _resources.Values.Where(r => r.ApplicationId == applicationId).ToList();
        return Task.FromResult(results);
    }

    public Task UpdateResourceAsync(Resource resource)
    {
        _resources[resource.Id] = resource;
        return Task.CompletedTask;
    }

    public Task<Deployment> CreateDeploymentAsync(Deployment deployment)
    {
        _deployments[deployment.Id] = deployment;
        return Task.FromResult(deployment);
    }

    public Task<Deployment?> GetDeploymentAsync(string id)
    {
        _deployments.TryGetValue(id, out var deployment);
        return Task.FromResult(deployment);
    }

    public Task<List<Deployment>> GetDeploymentsByApplicationAsync(string applicationId)
    {
        var results = _deployments.Values
            .Where(d => d.ApplicationId == applicationId)
            .OrderByDescending(d => d.CreatedAt)
            .ToList();
        return Task.FromResult(results);
    }

    public Task DeleteDeploymentAsync(string id)
    {
        _deployments.Remove(id);
        return Task.CompletedTask;
    }

    public Task<Team> CreateTeamAsync(Team team)
    {
        _teams[team.Id] = team;
        return Task.FromResult(team);
    }

    public Task<Team?> GetTeamAsync(string id)
    {
        _teams.TryGetValue(id, out var team);
        return Task.FromResult(team);
    }

    public Task<List<Team>> GetAllTeamsAsync()
    {
        var results = _teams.Values.ToList();
        return Task.FromResult(results);
    }

    public Task DeleteTeamAsync(string id)
    {
        _teams.Remove(id);
        return Task.CompletedTask;
    }
}
