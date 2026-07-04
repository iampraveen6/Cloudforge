# ADR-0000: CloudForge Architecture Vision

**Status:** Accepted

**Date:** 2026-07-04

**Author:** Praveen

---

# Overview

CloudForge is an **Internal Developer Platform (IDP)** that enables development teams to provision infrastructure, deploy applications, manage configurations, and operate workloads through a unified self-service platform.

Unlike traditional cloud management portals, CloudForge focuses on **Developer Experience (DevEx)** by providing opinionated workflows ("Golden Paths") that abstract cloud complexity while enforcing governance, security, and operational standards.

CloudForge is built as a **Platform as a Product**, where developers are treated as customers and the platform provides a consistent, automated, and secure experience.

---

# Vision

Developers should be able to create and deploy applications without becoming experts in Azure.

Instead of:

```
Developer
      ↓
Azure Portal
      ↓
Storage
Key Vault
Container Apps
Networking
Identity
```

CloudForge provides:

```
Developer
      ↓
CloudForge
      ↓
Standardized Platform APIs
      ↓
Azure Resources
```

---

# Objectives

CloudForge is designed to demonstrate modern Platform Engineering concepts including:

- Self-Service Infrastructure
- Platform as a Product
- Golden Path Development
- Cloud Governance
- Infrastructure Standardization
- Secure by Default
- Event-Driven Architecture
- Resource Lifecycle Management
- Developer Experience
- Cloud Native Design

---

# Core Principles

## 1. Everything is API First

Every platform capability must be exposed through REST APIs.

The UI and CLI should consume the same APIs.

```
Web Portal

CLI

Automation

↓

Platform API
```

---

## 2. Everything is Asynchronous

Provisioning cloud resources should never block user requests.

```
Request

↓

Validation

↓

Queue

↓

Worker

↓

Provision Resource
```

Benefits:

- Retry support
- Fault tolerance
- Scalability
- Better user experience

---

## 3. Resource Providers

CloudForge should never call Azure SDKs directly from controllers.

Instead:

```
REST API

↓

Provisioning Engine

↓

Resource Provider

↓

Azure SDK

↓

Azure Service
```

Every Azure service becomes a provider.

Examples:

- Blob Storage Provider
- Key Vault Provider
- App Configuration Provider
- Container Apps Provider
- Container Registry Provider

This makes CloudForge extensible and easy to test.

---

## 4. Platform Metadata

CloudForge stores platform metadata separately from Azure resources.

Azure stores infrastructure.

Cosmos DB stores:

- Applications
- Deployments
- Teams
- Resources
- Audit Logs
- Environments

This allows CloudForge to build higher-level workflows.

---

## 5. Event Driven Platform

Every significant action produces an event.

Example:

```
Application Created

↓

Storage Provisioned

↓

Deployment Started

↓

Deployment Completed

↓

Health Check Passed
```

Events can later drive:

- Notifications
- Audit Logs
- Dashboards
- Metrics
- Automation

---

# High-Level Architecture

```
                 +----------------------+
                 | Developer Portal     |
                 | (React + TypeScript) |
                 +----------+-----------+
                            |
                            |
                 REST / HTTPS
                            |
                 +----------v-----------+
                 | CloudForge API       |
                 | (.NET 9 Minimal API) |
                 +----------+-----------+
                            |
          +-----------------+-----------------+
          |                                   |
          |                                   |
+---------v----------+             +----------v---------+
| Provision Queue    |             | Cosmos DB          |
+---------+----------+             +--------------------+
          |
          |
+---------v-----------+
| Provision Worker    |
+---------+-----------+
          |
          |
+---------v------------------------------+
| Resource Provider Framework            |
+---------+--------------+---------------+
          |              |
          |              |
 Blob Provider     KeyVault Provider
 AppConfig Provider Container Provider
 Queue Provider     Monitor Provider
          |
          |
+---------v-------------------------------+
| Azure Services (via Floci)              |
+-----------------------------------------+
```

---

# Component Responsibilities

## Developer Portal

Responsible for:

- User interface
- Dashboards
- Application management
- Resource visualization

No provisioning logic should exist here.

---

## Platform API

Responsible for:

- Authentication
- Validation
- Authorization
- Queueing requests
- Returning operation status

Should never directly provision Azure resources.

---

## Provisioning Engine

Responsible for:

- Reading queue messages
- Selecting appropriate provider
- Executing provisioning
- Updating metadata
- Publishing events

---

## Resource Providers

Responsible for one Azure service only.

Each provider implements a common interface.

```csharp
public interface IResourceProvider
{
    Task ProvisionAsync(ResourceRequest request);

    Task DeleteAsync(string resourceId);

    Task<ResourceStatus> GetStatusAsync(string resourceId);
}
```

---

## Cosmos DB

Stores CloudForge state.

Examples:

Applications

Deployments

Audit Logs

Resources

Teams

Environments

---

# Repository Structure

```
CloudForge/

backend/

frontend/

workers/

providers/

sdk/

cli/

docs/

tests/

scripts/

docker/

.github/
```

---

# Domain Model

```
Organization

↓

Team

↓

Application

↓

Environment

↓

Resources

↓

Deployment

↓

Monitoring
```

---

# Request Lifecycle

```
Developer

↓

API

↓

Validation

↓

Queue

↓

Worker

↓

Provider

↓

Azure

↓

Metadata Update

↓

Event

↓

Dashboard
```

---

# Technology Decisions

| Area | Technology |
|--------|------------|
| Backend | .NET 9 |
| Frontend | React |
| Storage | Cosmos DB |
| Queue | Azure Queue Storage |
| Secrets | Azure Key Vault |
| Config | Azure App Configuration |
| Storage | Blob Storage |
| Monitoring | Azure Monitor |
| Containers | Azure Container Apps |
| Kubernetes | AKS |
| Local Azure | Floci |

---

# Non-Goals

CloudForge is **not** intended to replace:

- Azure Portal
- Azure CLI
- Azure Resource Manager

Instead, it provides opinionated workflows for internal development teams.

---

# Success Criteria

CloudForge should demonstrate:

✅ Platform Engineering

✅ Azure Architecture

✅ Event-Driven Systems

✅ Resource Providers

✅ Infrastructure Automation

✅ Cloud Native Design

✅ Secure Secret Management

✅ Observability

✅ Developer Experience

✅ Extensible Architecture

---

# Future Extensions

- Kubernetes Operators
- Terraform Integration
- GitHub Actions
- Azure DevOps
- Multi-tenancy
- Service Catalog
- AI-assisted Platform Operations

---

# Guiding Philosophy

> "Platform teams should build paved roads, not traffic rules."

CloudForge aims to provide a paved road where developers can safely and efficiently build, deploy, and operate applications without needing to understand every detail of the underlying cloud infrastructure.
