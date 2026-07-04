<div align="center">

# ☁️ CloudForge

### An Open-Source Internal Developer Platform (IDP) for Azure

*Empowering developers with self-service infrastructure while enabling platform teams to enforce governance, security, and operational excellence.*

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet)
![Azure](https://img.shields.io/badge/Azure-Cloud-0078D4?style=for-the-badge&logo=microsoftazure)
![React](https://img.shields.io/badge/React-Frontend-61DAFB?style=for-the-badge&logo=react)
![Docker](https://img.shields.io/badge/Docker-Containers-2496ED?style=for-the-badge&logo=docker)
![MIT License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)

---

**🚧 Active Development | Built for learning Platform Engineering, Cloud Architecture & Azure**

</div>

---

# 📖 Overview

Modern engineering teams shouldn't have to become Azure experts just to deploy an application.

CloudForge is an **Internal Developer Platform (IDP)** inspired by real-world platform engineering practices used by organizations running applications at scale.

It provides a **self-service developer experience** where engineers can provision cloud resources, manage secrets, configure applications, deploy workloads, and monitor services—all through a unified platform.

Rather than exposing developers directly to the complexity of Azure, CloudForge provides opinionated **Golden Paths** that improve developer productivity while enabling platform teams to maintain governance, security, and consistency.

---

# 🎯 Why CloudForge?

As applications scale, development teams often spend significant time managing cloud infrastructure instead of delivering business value.

CloudForge aims to solve this by providing:

- 🚀 Self-service infrastructure provisioning
- 🔐 Secure secrets management
- 📦 Standardized application deployments
- 📊 Built-in observability
- ⚙️ Centralized configuration
- 🏗️ Infrastructure standardization
- ☁️ Azure-native developer workflows

The goal isn't to replace Azure Portal—it's to provide a **developer-centric abstraction layer** that simplifies common operational tasks.

---

# ✨ Core Features

## 🚀 Application Management

- Create and bootstrap new applications
- Standardized project templates
- Environment onboarding
- Application lifecycle management

---

## ⚙️ Configuration Management

- Azure App Configuration integration
- Environment-specific settings
- Feature flags
- Centralized configuration

---

## 🔐 Secrets Management

- Azure Key Vault integration
- Secure secret generation
- Secret versioning
- Access policy management

---

## 📦 Deployment Platform

Deploy workloads to:

- Azure Container Apps
- Azure Kubernetes Service (AKS)

Features include:

- Deployment history
- Rollbacks
- Deployment status
- Environment promotion

---

## 📦 Container Management

- Azure Container Registry integration
- Image versioning
- Registry management
- Deployment artifacts

---

## 📊 Observability

- Azure Monitor integration
- Centralized logs
- Metrics
- Health dashboards
- Deployment insights

---

## 🏗️ Platform Services

- Blob Storage provisioning
- Managed Identity support
- Resource lifecycle management
- Self-service infrastructure requests

---

# 🏛️ Architecture

```
                         Developer
                              │
                              ▼
                   ┌─────────────────────┐
                   │  CloudForge Portal  │
                   └──────────┬──────────┘
                              │
                              ▼
                  ┌──────────────────────┐
                  │ Platform API (.NET)  │
                  └──────────┬───────────┘
                             │
                  Provision Request Queue
                             │
                             ▼
                 ┌────────────────────────┐
                 │ Provisioning Engine    │
                 └──────────┬─────────────┘
                            │
        ┌───────────┬────────────┬─────────────┬─────────────┐
        ▼           ▼            ▼             ▼
 Blob Storage   Key Vault   App Config   Container Registry
        │
        ▼
 Container Apps / AKS
        │
        ▼
 Azure Monitor
```

CloudForge follows an **event-driven architecture** where infrastructure provisioning is asynchronous, scalable, and resilient.

---

# 🧩 Technology Stack

| Layer | Technology |
|--------|------------|
| Frontend | React + TypeScript |
| Backend | .NET 8 |
| API | Azure Functions / Minimal APIs |
| Database | Azure Cosmos DB |
| Storage | Azure Blob Storage |
| Messaging | Azure Queue Storage |
| Configuration | Azure App Configuration |
| Secrets | Azure Key Vault |
| Containers | Azure Container Registry |
| Compute | Azure Container Apps / AKS |
| Monitoring | Azure Monitor |
| Local Azure | Floci (floci.io/az) |
| CI/CD | GitHub Actions *(Planned)* |

---

# 💻 Local Development

CloudForge uses **Floci** for local Azure emulation, enabling development without requiring an Azure subscription.

## Prerequisites

- **.NET 8.0 SDK** - [Download here](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Floci** - Local Azure emulator ([floci.io/az](https://floci.io/az))
- **Git** - For cloning the repository

## Quick Start

### 1. Clone the Repository

```bash
git clone https://github.com/yourusername/CloudForge.git
cd CloudForge
```

### 2. Start Floci (Local Azure Emulator)

Floci provides wire-compatible Azure services for local development:

```bash
# Install Floci (if not already installed)
npm install -g @floci/az

# Start Floci
floci start
```

Floci will start on `http://localhost:4566` and provide:
- Azure Cosmos DB
- Blob Storage & Queue Storage
- Azure Key Vault
- App Configuration
- Container Apps
- Container Registry
- And more Azure services

### 3. Configure Backend

The backend is pre-configured for Floci development. Configuration is in `backend/appsettings.json`:

```json
{
  "Floci": {
    "UseFloci": true,
    "Endpoint": "http://localhost:4566"
  },
  "CosmosDb": {
    "ConnectionString": "AccountEndpoint=https://localhost:8081;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==;",
    "DatabaseName": "CloudForgeDB"
  }
}
```

**Note:** When `UseFloci` is `true`, the backend uses an in-memory database and skips actual Azure operations, making it perfect for local development and testing.

### 4. Build the Backend

```bash
cd backend
dotnet restore
dotnet build
```

### 5. Run the Backend

```bash
dotnet run
```

The backend will start on `http://localhost:5093`

### 6. Access the API

- **Swagger UI:** `http://localhost:5093/swagger` - Interactive API documentation
- **Health Check:** `http://localhost:5093/health` - Verify backend is running
- **API Info:** `http://localhost:5093/` - Basic API information

## Testing the Backend

### Test 1: Health Check

```bash
curl http://localhost:5093/health
```

Expected response:
```json
{
  "status": "healthy",
  "timestamp": "2024-01-01T12:00:00Z"
}
```

### Test 2: Create a Resource

```bash
curl -X 'POST' 'http://localhost:5093/api/resources' \
  -H 'Content-Type: application/json' \
  -d '{
  "name": "my-storage",
  "applicationId": "app-123",
  "environmentId": "env-456",
  "resourceType": 0,
  "properties": {
    "containerName": "my-container"
  },
  "tags": {}
}'
```

Expected response (201 Created):
```json
{
  "id": "resource-id",
  "name": "my-storage",
  "applicationId": "app-123",
  "environmentId": "env-456",
  "type": 0,
  "status": 1,
  "properties": {
    "containerName": "my-container"
  },
  "azureResourceId": "blob-app-123-env-456",
  "createdAt": "2024-01-01T12:00:00Z",
  "updatedAt": "2024-01-01T12:00:00Z"
}
```

### Test 3: Get Resources by Application

```bash
curl http://localhost:5093/api/resources/application/app-123
```

### Test 4: Get Resource Status

```bash
curl http://localhost:5093/api/resources/{resource-id}/status
```

## Available API Endpoints

### Resources
- `POST /api/resources` - Create a new resource
- `GET /api/resources/{id}` - Get resource by ID
- `GET /api/resources/application/{applicationId}` - Get resources by application
- `DELETE /api/resources/{id}` - Delete a resource
- `GET /api/resources/{id}/status` - Get resource status

### Applications
- `POST /api/applications` - Create application
- `GET /api/applications/{id}` - Get application by ID
- `GET /api/applications` - Get all applications
- `DELETE /api/applications/{id}` - Delete application

### Teams
- `POST /api/teams` - Create team
- `GET /api/teams/{id}` - Get team by ID
- `GET /api/teams` - Get all teams
- `DELETE /api/teams/{id}` - Delete team

### Deployments
- `POST /api/deployments` - Create deployment
- `GET /api/deployments/{id}` - Get deployment by ID
- `GET /api/deployments/application/{applicationId}` - Get deployments by application
- `DELETE /api/deployments/{id}` - Delete deployment

## Resource Types

The backend supports the following resource types:

| Type | Value | Description |
|------|-------|-------------|
| BlobStorage | 0 | Azure Blob Storage account |
| KeyVault | 1 | Azure Key Vault for secrets |
| AppConfiguration | 2 | Azure App Configuration |
| ContainerApps | 3 | Azure Container Apps |
| ContainerRegistry | 4 | Azure Container Registry |

## Troubleshooting

### Backend won't start

1. Ensure Floci is running: `floci start`
2. Check if port 5093 is available
3. Verify .NET 8.0 SDK is installed: `dotnet --version`

### Build errors

1. Restore packages: `dotnet restore`
2. Clean build: `dotnet clean && dotnet build`
3. Check for missing dependencies in `.csproj`

### API returns errors

1. Verify Floci is running on port 4566
2. Check backend console for error messages
3. Ensure `UseFloci` is set to `true` in `appsettings.json`

### In-memory database data loss

This is expected behavior. The in-memory database resets when the backend restarts. For persistent data, you can:
- Connect to a real Cosmos DB account (set `UseFloci: false`)
- Add SQLite persistence (planned feature)

## Development Notes

- The backend uses **minimal APIs** for simplicity and performance
- All providers skip actual Azure operations when `UseFloci` is true
- The in-memory database is perfect for development and testing
- Swagger UI provides interactive API documentation
- The backend is ready for frontend integration

---

# �🛠️ Platform Engineering Principles

CloudForge is built around modern Platform Engineering concepts.

✅ Self-Service Infrastructure

✅ Golden Path Development

✅ Infrastructure Abstraction

✅ Platform as a Product

✅ Cloud Governance

✅ Security by Default

✅ Event-Driven Architecture

✅ Observability First

✅ Developer Experience (DevEx)

✅ Cloud-Native Design

---

# 📁 Project Structure

```
CloudForge/

├── backend/
├── frontend/
├── workers/
├── cli/
├── sdk/
├── infrastructure/
├── docs/
├── scripts/
├── tests/
└── .github/
```

---

# 🚀 Developer Workflow

```
Developer

     │

     ▼

Create Application

     │

Configure Settings

     │

Generate Secrets

     │

Provision Infrastructure

     │

Deploy Application

     │

Monitor Health

     │

Scale & Operate
```

CloudForge abstracts these operations behind a unified API, dashboard, and CLI.

---

# 🗺️ Roadmap

## Phase 1 — Foundation

- [x] Solution Architecture
- [x] Repository Setup
- [ ] Platform API
- [ ] Developer Portal

## Phase 2 — Platform Core

- [ ] Application Management
- [ ] Resource Provisioning Engine
- [ ] Blob Storage Provider
- [ ] Key Vault Provider
- [ ] App Configuration Provider

## Phase 3 — Deployments

- [ ] Azure Container Registry
- [ ] Azure Container Apps
- [ ] AKS Support
- [ ] Deployment History

## Phase 4 — Operations

- [ ] Azure Monitor Integration
- [ ] Audit Logs
- [ ] Metrics Dashboard
- [ ] Health Monitoring

## Phase 5 — Developer Experience

- [ ] CloudForge CLI
- [ ] RBAC
- [ ] Multi-tenancy
- [ ] Service Catalog

---

# 🎯 Learning Objectives

CloudForge is a portfolio project focused on mastering:

- Platform Engineering
- Azure Cloud Architecture
- Event-Driven Systems
- Infrastructure Automation
- Cloud-Native Development
- Developer Platforms
- Kubernetes
- Container Platforms
- Secure Configuration Management
- Observability
- Azure SDK
- Distributed Systems

---

# 🌟 Inspiration

CloudForge draws inspiration from industry-leading platform engineering tools:

- Backstage
- Azure Developer CLI (azd)
- Azure Portal
- Crossplane
- Dapr
- Kubernetes Operators
- Humanitec

---

# 🤝 Contributing

Contributions, feature suggestions, and discussions are welcome.

If you're interested in Platform Engineering, Azure, or Cloud Native development, feel free to open an issue or submit a pull request.

---

# 📜 License

This project is licensed under the MIT License.

---

<div align="center">

### ⭐ If you find CloudForge interesting, consider giving the repository a star!

**Building modern Platform Engineering concepts one commit at a time.**

</div>
