# dotnet-clean-architecture-api

A **Clean Architecture–based Task & Project Management REST API** built with **ASP.NET Core (.NET 10)**.  
This project is **backend-only** and focuses on **architecture, business logic, and production-ready API design**.

Inspired by real internal tools like **Jira / Taiga / Trello (backend side only)**.

---

## Purpose of This Project

This repository exists to demonstrate:

- Clean Architecture principles
- Proper separation of concerns
- Real-world business logic (not just CRUD)
- Centralized exception handling
- Validation using FluentValidation
- Database-backed error logging
- Scalable and maintainable API design

> This project answers a single question for recruiters:  
> **“Is this developer production-ready for backend systems?”**

---

## Architecture Overview
The solution strictly follows **Clean Architecture**:


### Layer Responsibilities

| Layer | Responsibility |
|------|---------------|
| **Domain** | Entities, enums, interfaces (no external dependencies) |
| **Application** | Business logic, use cases, DTOs, validations |
| **Infrastructure** | EF Core, PostgreSQL, repositories |
| **API** | Controllers, middleware, HTTP & request pipeline |

**Business rules live in the Application layer**, not in controllers or database.

---

## Core Features

### Project Management
- Create project
- Update project
- Archive project
- Get all projects
- Enforce unique project name
- Prevent updates to archived projects

### Task Management
- Create task under a project
- Update task details
- Assign task to developer
- Update task status
- Fetch tasks by project

### Business Rules (Application Layer)
- Tasks cannot be created under archived projects
- Task status transitions are validated
- Completed tasks cannot be modified
- Project cannot be deleted if active tasks exist

### API Quality
- DTO-based API responses (no entity exposure)
- Proper HTTP status codes
- Centralized exception handling
- Clean validation error responses

---

## Tech Stack

- **ASP.NET Core Web API (.NET 10)**
- **Clean Architecture**
- **Entity Framework Core**
- **PostgreSQL**
- **FluentValidation**
- **Swagger / OpenAPI**

No unnecessary libraries are used.

---

## Error Handling & Logging

- Global `ExceptionMiddleware`
- Handles **all exception types**
- Logs errors directly into the database
- Captures:
  - Error message
  - Exception type
  - File name & line number
  - Stack trace
  - Request path & HTTP method
  - Timestamp

This allows building an **admin UI for error monitoring** later.

---

## How to Run Locally

### Clone the repository
```bash
  git clone https://github.com/Taha-Khan-Personal-2202/dotnet-clean-architecture-api.git

Configure database
Update appsettings.json:
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=TaskDb;Username=postgres;Password=yourpassword"
  }

Run migrations
Using Package Manager Console:
  Add-Migration InitialCreate
  Update-Database

Run the API
  dotnet run

Open Swagger UI
  https://localhost:{port}/swagger
