using CloudForge.Infrastructure;
using CloudForge.Domain.Providers;
using CloudForge.Domain.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure settings
builder.Services.Configure<Settings>(builder.Configuration.GetSection(Settings.SectionName));

// Add services to the container
var settings = builder.Configuration.GetSection(Settings.SectionName).Get<Settings>() ?? new Settings();

// Register Cosmos DB service (disabled for Floci compatibility)
if (!settings.Floci.UseFloci)
{
    builder.Services.AddSingleton<CosmosDbService>(sp => 
    {
        var service = new CosmosDbService(
            settings.CosmosDb.ConnectionString,
            settings.CosmosDb.DatabaseName
        );
        service.InitializeAsync().Wait();
        return service;
    });
}
else
{
    // Use in-memory service for Floci development
    builder.Services.AddSingleton<ICosmosDbService, InMemoryCosmosDbService>();
}

// Register Resource Providers
builder.Services.AddSingleton<BlobStorageProvider>(sp => 
    new BlobStorageProvider(settings.Floci.Endpoint, settings.Floci.UseFloci));
builder.Services.AddSingleton<KeyVaultProvider>(sp => 
    new KeyVaultProvider(settings.Floci.Endpoint, settings.Floci.UseFloci));

builder.Services.AddSingleton<AppConfigurationProvider>(sp => 
    new AppConfigurationProvider(settings.Floci.Endpoint, settings.Floci.UseFloci));

builder.Services.AddSingleton<ContainerAppsProvider>(sp => 
    new ContainerAppsProvider(settings.Floci.Endpoint, settings.Floci.UseFloci));

builder.Services.AddSingleton<ContainerRegistryProvider>(sp => 
    new ContainerRegistryProvider(settings.Floci.Endpoint, settings.Floci.UseFloci));

// Register all IResourceProvider implementations
builder.Services.AddSingleton<IEnumerable<IResourceProvider>>(sp => 
    new IResourceProvider[] 
    {
        sp.GetRequiredService<BlobStorageProvider>(),
        sp.GetRequiredService<KeyVaultProvider>(),
        sp.GetRequiredService<AppConfigurationProvider>(),
        sp.GetRequiredService<ContainerAppsProvider>(),
        sp.GetRequiredService<ContainerRegistryProvider>()
    });

// Register Provisioning Engine
builder.Services.AddSingleton<ProvisioningEngine>();

// Register Queue Service (disabled for Floci to avoid authentication issues)
if (!settings.Floci.UseFloci)
{
    builder.Services.AddSingleton<QueueService>(sp => 
        new QueueService(settings.CosmosDb.ConnectionString));
}

// Add API explorer and Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
    .WithName("HealthCheck")
    .WithOpenApi();

// API info endpoint
app.MapGet("/", () => Results.Ok(new { 
    name = "CloudForge API", 
    version = "1.0.0",
    description = "Internal Developer Platform for Azure"
}))
    .WithName("ApiInfo")
    .WithOpenApi();

// Resource provisioning endpoints
app.MapPost("/api/resources", async (ResourceRequest request, ProvisioningEngine engine, ICosmosDbService cosmosDb) =>
{
    try
    {
        var resource = await engine.ProvisionResourceAsync(request);
        return Results.Created($"/api/resources/{resource.Id}", resource);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
    .WithName("CreateResource")
    .WithOpenApi();

app.MapGet("/api/resources/{id}", async (string id, ICosmosDbService cosmosDb) =>
{
    var resource = await cosmosDb.GetResourceAsync(id);
    return resource is not null ? Results.Ok(resource) : Results.NotFound();
})
    .WithName("GetResource")
    .WithOpenApi();

app.MapGet("/api/resources/application/{applicationId}", async (string applicationId, ICosmosDbService cosmosDb) =>
{
    var resources = await cosmosDb.GetResourcesByApplicationAsync(applicationId);
    return Results.Ok(resources);
})
    .WithName("GetResourcesByApplication")
    .WithOpenApi();

app.MapDelete("/api/resources/{id}", async (string id, ProvisioningEngine engine) =>
{
    try
    {
        await engine.DeleteResourceAsync(id);
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
    .WithName("DeleteResource")
    .WithOpenApi();

app.MapGet("/api/resources/{id}/status", async (string id, ProvisioningEngine engine) =>
{
    try
    {
        var status = await engine.GetResourceStatusAsync(id);
        return Results.Ok(new { resourceId = id, status = status.ToString() });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
    .WithName("GetResourceStatus")
    .WithOpenApi();

// Application endpoints
app.MapPost("/api/applications", async (Application application, ICosmosDbService cosmosDb) =>
{
    try
    {
        var created = await cosmosDb.CreateApplicationAsync(application);
        return Results.Created($"/api/applications/{created.Id}", created);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
    .WithName("CreateApplication")
    .WithOpenApi();

app.MapGet("/api/applications/{id}", async (string id, ICosmosDbService cosmosDb) =>
{
    var application = await cosmosDb.GetApplicationAsync(id);
    return application is not null ? Results.Ok(application) : Results.NotFound();
})
    .WithName("GetApplication")
    .WithOpenApi();

app.MapGet("/api/applications", async (ICosmosDbService cosmosDb) =>
{
    var applications = await cosmosDb.GetAllApplicationsAsync();
    return Results.Ok(applications);
})
    .WithName("GetAllApplications")
    .WithOpenApi();

app.MapDelete("/api/applications/{id}", async (string id, ICosmosDbService cosmosDb) =>
{
    try
    {
        await cosmosDb.DeleteApplicationAsync(id);
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
    .WithName("DeleteApplication")
    .WithOpenApi();

// Team endpoints
app.MapPost("/api/teams", async (Team team, ICosmosDbService cosmosDb) =>
{
    try
    {
        var created = await cosmosDb.CreateTeamAsync(team);
        return Results.Created($"/api/teams/{created.Id}", created);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
    .WithName("CreateTeam")
    .WithOpenApi();

app.MapGet("/api/teams/{id}", async (string id, ICosmosDbService cosmosDb) =>
{
    var team = await cosmosDb.GetTeamAsync(id);
    return team is not null ? Results.Ok(team) : Results.NotFound();
})
    .WithName("GetTeam")
    .WithOpenApi();

app.MapGet("/api/teams", async (ICosmosDbService cosmosDb) =>
{
    var teams = await cosmosDb.GetAllTeamsAsync();
    return Results.Ok(teams);
})
    .WithName("GetAllTeams")
    .WithOpenApi();

app.MapDelete("/api/teams/{id}", async (string id, ICosmosDbService cosmosDb) =>
{
    try
    {
        await cosmosDb.DeleteTeamAsync(id);
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
    .WithName("DeleteTeam")
    .WithOpenApi();

// Deployment endpoints
app.MapPost("/api/deployments", async (Deployment deployment, ICosmosDbService cosmosDb) =>
{
    try
    {
        var created = await cosmosDb.CreateDeploymentAsync(deployment);
        return Results.Created($"/api/deployments/{created.Id}", created);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
    .WithName("CreateDeployment")
    .WithOpenApi();

app.MapGet("/api/deployments/{id}", async (string id, ICosmosDbService cosmosDb) =>
{
    var deployment = await cosmosDb.GetDeploymentAsync(id);
    return deployment is not null ? Results.Ok(deployment) : Results.NotFound();
})
    .WithName("GetDeployment")
    .WithOpenApi();

app.MapGet("/api/deployments/application/{applicationId}", async (string applicationId, ICosmosDbService cosmosDb) =>
{
    var deployments = await cosmosDb.GetDeploymentsByApplicationAsync(applicationId);
    return Results.Ok(deployments);
})
    .WithName("GetDeploymentsByApplication")
    .WithOpenApi();

app.MapDelete("/api/deployments/{id}", async (string id, ICosmosDbService cosmosDb) =>
{
    try
    {
        await cosmosDb.DeleteDeploymentAsync(id);
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
    .WithName("DeleteDeployment")
    .WithOpenApi();

app.Run();
