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
| Backend | .NET 9 |
| API | Azure Functions / Minimal APIs |
| Database | Azure Cosmos DB |
| Storage | Azure Blob Storage |
| Messaging | Azure Queue Storage |
| Configuration | Azure App Configuration |
| Secrets | Azure Key Vault |
| Containers | Azure Container Registry |
| Compute | Azure Container Apps / AKS |
| Monitoring | Azure Monitor |
| Local Azure | Floci |
| CI/CD | GitHub Actions *(Planned)* |

---

# 🛠️ Platform Engineering Principles

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
