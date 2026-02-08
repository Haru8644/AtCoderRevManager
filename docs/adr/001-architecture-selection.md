# 1. Architecture Selection: Cloud-Native Simulation with .NET Aspire

Date: 2026-02-06
Status: Accepted

## Context
We need to develop "AtCoderRevManager," a competitive programming review assistant. 
The project aims to demonstrate AZ-204 competencies and Microsoft Azure architectural patterns under a strict "Zero Cost" constraint for development, while maintaining readiness for enterprise-grade deployment.

## Decision
We will use **.NET Aspire** as the orchestration platform and **Clean Architecture** for the application structure.

## Consequences
* **Pros:**
    * Enables a "Cloud-Native" development experience locally (simulating Azure resources).
    * Simplifies the integration of Azure Cosmos DB Emulator and other containerized services.
    * Provides built-in observability (OpenTelemetry) via the Aspire Dashboard, fulfilling AZ-204 monitoring requirements.
    * Allows for easy deployment to Azure Container Apps via `azd` CLI in the future.
* **Cons:**
    * Requires Docker Desktop/Podman to be running during development.
* **Cost Analysis (Preliminary):**
    * Development: $0 (Local Emulators).
    * Production (Estimated): Utilizing Azure for Students credits ($100/year) or Consumption Plan for Serverless compute.

## Tech Stack
* **Orchestration:** .NET Aspire (using .NET 10 Preview/LTS features if available, falling back to .NET 9)
* **Database:** Azure Cosmos DB (NoSQL) via Emulator
* **Frontend:** Blazor Interactive Server + Fluent UI