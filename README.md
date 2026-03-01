# ⏱️🔁 AtCoderRevManager

![.NET](https://img.shields.io/badge/.NET-10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-12-239120?style=for-the-badge&logo=csharp&logoColor=white)
![Blazor](https://img.shields.io/badge/Blazor-Interactive%20Server-5C2D91?style=for-the-badge&logo=blazor&logoColor=white)
![Fluent UI](https://img.shields.io/badge/Fluent%20UI-Microsoft-0078D4?style=for-the-badge&logo=microsoft&logoColor=white)
![EF Core](https://img.shields.io/badge/EF%20Core-ORM-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Aspire](https://img.shields.io/badge/.NET%20Aspire-Orchestration-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Local%20(Docker/Aspire)-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)

> **A product-grade spaced repetition dashboard that turns “solve-and-forget” into a measurable review loop — built with Clean Architecture, MVVM, and a cyber-inspired, mobile-like UX.**

---

## 📖 Overview
AtCoderRevManager is a single-page dashboard designed to prevent the “solve once and never revisit” problem in competitive programming.
It schedules reviews using an Ebbinghaus-based spaced repetition algorithm and provides an operational UI that makes reviewing frictionless.

The project prioritizes enterprise-grade maintainability:
- Clean Architecture with strict separation (**Domain / Infrastructure / ApiService / Web**)
- MVVM on the Blazor side for testable UI logic
- Repository + Service abstraction for composability and long-term evolution
- .NET Aspire for a cloud-native local dev experience

---

## 🚀 The Challenge & Business Value
**The Problem:** In self-driven learning, output (solving) is often decoupled from retention (reviewing). Without a deterministic review loop, solved problems decay into “unreliable memory” and performance plateaus.

**The Solution:** AtCoderRevManager treats learning as a system:
- Reviews are scheduled by a domain-owned algorithm (not scattered across UI code)
- Due items are surfaced as actionable KPI signals
- The dashboard supports fast triage: search/filter → update progress → continue

**Impact (Expected / Observed):**
- Higher retention through scheduled review cadence
- Reduced context-switch cost (single dashboard for all review tasks)
- Consistent workflow that scales as the problem set grows

---

## ✨ Key Features
- **Spaced Repetition Scheduling:** Next review date is computed inside the domain model (encapsulated business logic).
- **“Due Today” KPI & Focus Mode:** Immediate prioritization of tasks that matter today.
- **Real-Time Search & Filtering:** In-memory LINQ for responsive UX.
- **MVVM-Driven Dialog Workflows:** Create/update operations are handled via dialog components with clear state boundaries.
- **Toast Notifications & Defensive Error Handling:** UX-first feedback with robust logging (`ILogger`, try/catch).
- **Product-Like Cyber/Mobile UI:** Gradient surfaces, glass cards, micro-interactions (sweep/shimmer/tap), tuned for readability.

---

## 🏗️ Architecture
This repository follows Clean Architecture boundaries and keeps UI concerns out of the core domain.

**Projects**
- `AtCoderRevManager.Domain`
  - Entities and business rules (review scheduling lives here)
  - Interfaces: `IReviewRepository`, `IReviewService`
- `AtCoderRevManager.Infrastructure`
  - EF Core `DbContext` and SQL repository implementation
- `AtCoderRevManager.ApiService`
  - ASP.NET Core Web API exposing CRUD endpoints
- `AtCoderRevManager.Web`
  - Blazor (Interactive Server) SPA + Fluent UI components
  - MVVM-style UI composition
- `AtCoderRevManager.AppHost`
  - .NET Aspire orchestration for local development
- `AtCoderRevManager.ServiceDefaults`
  - OpenTelemetry defaults and cross-cutting service config

---

## 🧩 Data & Infrastructure Notes
- **Local development:** SQL Server is provisioned via **.NET Aspire** (Docker container) to keep setup friction low.
- **Azure SQL:** An Azure SQL database can be used as a target environment; however, it is intentionally not kept running continuously in this public setup to avoid unnecessary cost. The application design remains provider-compatible for deployment scenarios.

---

## 🛠️ Getting Started (Local Development)

### Prerequisites
- .NET SDK 10.x
- Docker Desktop (Linux containers / WSL2)
  - Make sure Docker Desktop is running before executing `dotnet run`.
- Visual Studio 2026 / Visual Studio 2022+ / VS Code

### Run the solution with .NET Aspire
From the repository root:

```bash
cd src/AtCoderRevManager.AppHost
dotnet run
```
### What happens next (Aspire Dashboard)
Running the AppHost starts a local **.NET Aspire Dashboard** (a management UI for the distributed app).

In the terminal, you will see a URL like:
- `Now listening on: https://localhost:17077`
- `Login to the dashboard at https://localhost:17077/login?t=<one-time-token>`

> The `t=<one-time-token>` is a temporary login token for the **Aspire Dashboard** (not the AtCoderRevManager app itself).

### Open the actual app (Web UI)
1. Open the Aspire Dashboard login URL shown in the console.
2. In the dashboard, find `AtCoderRevManager.Web`.
3. Click its **Endpoint / Launch** link to open the AtCoderRevManager Web UI.

### Troubleshooting
- If `docker ps` fails, start Docker Desktop first.
- If you see `Failed to fetch data from the server`, check that `AtCoderRevManager.ApiService` is running in the Aspire Dashboard.
