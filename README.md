# Clean Architecture Task & Project Management REST API

A **production-ready** backend REST API for task & project management built with **ASP.NET Core 10** following **Clean Architecture** principles.

Inspired by tools like **Taiga / Trello** (backend side only).

[![.NET](https://img.shields.io/badge/.NET-10-blueviolet.svg)](https://dotnet.microsoft.com)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)

---

## Purpose of This Project

This repository demonstrates how to build a **clean, scalable, and maintainable** backend system that answers one important question for recruiters and teams:

> **“Is this developer ready to build production-grade backend systems?”**

Focus: **Clean Architecture**, proper layering, real business rules, robust error handling, and production-ready API design.

---

## Architecture Overview

Strict **Clean Architecture** implementation with clear separation of concerns:

| Layer           | Responsibility                                                                 | Dependencies      |
|-----------------|--------------------------------------------------------------------------------|-------------------|
| **Domain**      | Entities, enums, domain interfaces, business rules             | **None**          |
| **Application** | Use cases, DTOs, commands/queries, FluentValidation     | Domain              |
| **Infrastructure** | EF Core, PostgreSQL, repositories, | Application + Domain |
| **API**         | Controllers, middleware, exception handling, Swagger             | Application       |

**Important:** All meaningful **business logic** lives in the **Application** layer — **never** in controllers or infrastructure.

---

## Core Features

### Project Management
- Create new project
- Update project details
- Archive project
- List all projects (active + archived)
- Enforce **unique project name**
- Prevent modification of archived projects

### Task Management
- Create task in a project
- Update task title, description, priority, due date
- Assign task to a developer
- Change task status
- Get tasks filtered by project

### Important Business Rules (enforced in Application layer)
- Cannot create tasks in **archived** projects
- Strict **status transition** validation
- **Completed** tasks are immutable (cannot be modified)
- Project cannot be **deleted** if it contains active/open tasks

---

## Technical Stack

- **ASP.NET Core Web API** (.NET 10)
- **Clean Architecture**
- **Entity Framework Core** (Code-First)
- **PostgreSQL**
- **FluentValidation**
- **Swagger**
- Minimal external packages — only what's truly necessary

---

## Error Handling & Logging

- Global **ExceptionMiddleware**
- Catches **all** exceptions (expected & unexpected)

---
## Getting Started — Local Development

### 1. Clone the repository

```bash
git clone https://github.com/Taha-Khan-Personal-2202/dotnet-clean-architecture-api.git
```

### 2. Configure database connection

Update the connection string in `appsettings.json` (or `appsettings.Development.json`):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=TaskDb;Username=postgres;Password=yourpassword"
  }
}
```

Note: actual port may be different - check the console output after dotnet run
Important: Replace 'yourpassword' with your actual PostgreSQL password

### 3. Apply database migrations
Open Package Manager Console in Visual Studio and run these commands one by one:

```PowerShell
Add-Migration InitialCreate -Project src/Infrastructure -StartupProject src/API
```

```PowerShell
Update-Database -Project src/Infrastructure -StartupProject src/API
```

### 4. Apply database migrations
```Text
https://localhost:{port}/swagger
```




