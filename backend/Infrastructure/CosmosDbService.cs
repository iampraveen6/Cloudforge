using Microsoft.Azure.Cosmos;
using CloudForge.Domain.Models;

namespace CloudForge.Infrastructure;

public interface ICosmosDbService
{
    Task<Application> CreateApplicationAsync(Application application);
    Task<Application?> GetApplicationAsync(string id);
    Task<List<Application>> GetApplicationsByTeamAsync(string teamId);
    Task<List<Application>> GetAllApplicationsAsync();
    Task DeleteApplicationAsync(string id);
    Task<Resource> CreateResourceAsync(Resource resource);
    Task<Resource?> GetResourceAsync(string id);
    Task<List<Resource>> GetResourcesByApplicationAsync(string applicationId);
    Task UpdateResourceAsync(Resource resource);
    Task<Deployment> CreateDeploymentAsync(Deployment deployment);
    Task<Deployment?> GetDeploymentAsync(string id);
    Task<List<Deployment>> GetDeploymentsByApplicationAsync(string applicationId);
    Task DeleteDeploymentAsync(string id);
    Task<Team> CreateTeamAsync(Team team);
    Task<Team?> GetTeamAsync(string id);
    Task<List<Team>> GetAllTeamsAsync();
    Task DeleteTeamAsync(string id);
}

public class CosmosDbService : ICosmosDbService
{
    private readonly CosmosClient _cosmosClient;
    private readonly Database _database;
    private readonly Container _applicationsContainer;
    private readonly Container _resourcesContainer;
    private readonly Container _deploymentsContainer;
    private readonly Container _teamsContainer;

    public CosmosDbService(string connectionString, string databaseName)
    {
        _cosmosClient = new CosmosClient(connectionString);
        _database = _cosmosClient.GetDatabase(databaseName);
        
        _applicationsContainer = _database.GetContainer("Applications");
        _resourcesContainer = _database.GetContainer("Resources");
        _deploymentsContainer = _database.GetContainer("Deployments");
        _teamsContainer = _database.GetContainer("Teams");
    }

    public async Task InitializeAsync()
    {
        await _database.CreateContainerIfNotExistsAsync("Applications", "/id");
        await _database.CreateContainerIfNotExistsAsync("Resources", "/id");
        await _database.CreateContainerIfNotExistsAsync("Deployments", "/id");
        await _database.CreateContainerIfNotExistsAsync("Teams", "/id");
    }

    // Application operations
    public async Task<Application> CreateApplicationAsync(Application application)
    {
        var response = await _applicationsContainer.CreateItemAsync(application);
        return response.Resource;
    }

    public async Task<Application?> GetApplicationAsync(string id)
    {
        try
        {
            var response = await _applicationsContainer.ReadItemAsync<Application>(id, new PartitionKey(id));
            return response.Resource;
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<Application>> GetApplicationsByTeamAsync(string teamId)
    {
        var query = new QueryDefinition("SELECT * FROM c WHERE c.TeamId = @teamId")
            .WithParameter("@teamId", teamId);
        
        var iterator = _applicationsContainer.GetItemQueryIterator<Application>(query);
        var results = new List<Application>();
        
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response);
        }
        
        return results;
    }

    // Resource operations
    public async Task<Resource> CreateResourceAsync(Resource resource)
    {
        var response = await _resourcesContainer.CreateItemAsync(resource);
        return response.Resource;
    }

    public async Task<Resource?> GetResourceAsync(string id)
    {
        try
        {
            var response = await _resourcesContainer.ReadItemAsync<Resource>(id, new PartitionKey(id));
            return response.Resource;
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<Resource>> GetResourcesByApplicationAsync(string applicationId)
    {
        var query = new QueryDefinition("SELECT * FROM c WHERE c.ApplicationId = @applicationId")
            .WithParameter("@applicationId", applicationId);
        
        var iterator = _resourcesContainer.GetItemQueryIterator<Resource>(query);
        var results = new List<Resource>();
        
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response);
        }
        
        return results;
    }

    public async Task UpdateResourceAsync(Resource resource)
    {
        await _resourcesContainer.UpsertItemAsync(resource);
    }

    // Deployment operations
    public async Task<Deployment> CreateDeploymentAsync(Deployment deployment)
    {
        var response = await _deploymentsContainer.CreateItemAsync(deployment);
        return response.Resource;
    }

    public async Task<Deployment?> GetDeploymentAsync(string id)
    {
        try
        {
            var response = await _deploymentsContainer.ReadItemAsync<Deployment>(id, new PartitionKey(id));
            return response.Resource;
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<Deployment>> GetDeploymentsByApplicationAsync(string applicationId)
    {
        var query = new QueryDefinition("SELECT * FROM c WHERE c.ApplicationId = @applicationId ORDER BY c.CreatedAt DESC")
            .WithParameter("@applicationId", applicationId);
        
        var iterator = _deploymentsContainer.GetItemQueryIterator<Deployment>(query);
        var results = new List<Deployment>();
        
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response);
        }
        
        return results;
    }

    // Team operations
    public async Task<Team> CreateTeamAsync(Team team)
    {
        var response = await _teamsContainer.CreateItemAsync(team);
        return response.Resource;
    }

    public async Task<Team?> GetTeamAsync(string id)
    {
        try
        {
            var response = await _teamsContainer.ReadItemAsync<Team>(id, new PartitionKey(id));
            return response.Resource;
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<Application>> GetAllApplicationsAsync()
    {
        var query = new QueryDefinition("SELECT * FROM c");
        var iterator = _applicationsContainer.GetItemQueryIterator<Application>(query);
        var results = new List<Application>();
        
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response);
        }
        
        return results;
    }

    public async Task DeleteApplicationAsync(string id)
    {
        await _applicationsContainer.DeleteItemAsync<Application>(id, new PartitionKey(id));
    }

    public async Task DeleteDeploymentAsync(string id)
    {
        await _deploymentsContainer.DeleteItemAsync<Deployment>(id, new PartitionKey(id));
    }

    public async Task<List<Team>> GetAllTeamsAsync()
    {
        var query = new QueryDefinition("SELECT * FROM c");
        var iterator = _teamsContainer.GetItemQueryIterator<Team>(query);
        var results = new List<Team>();
        
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response);
        }
        
        return results;
    }

    public async Task DeleteTeamAsync(string id)
    {
        await _teamsContainer.DeleteItemAsync<Team>(id, new PartitionKey(id));
    }
}
