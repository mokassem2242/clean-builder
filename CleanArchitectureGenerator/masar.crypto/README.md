# masar.crypto

## Overview

This solution follows **Clean Architecture** principles with **Domain-Driven Design (DDD)** alignment. It provides a structured, maintainable foundation for building scalable .NET applications.

## Solution Structure

```
masar.crypto/
├── src/                          # Source code projects
│   ├── masar.crypto.Domain/
│   ├── masar.crypto.Application/
│   ├── masar.crypto.Infrastructure/
│   ├── masar.crypto.API/
├── tests/                      # Test projects
│   ├── masar.crypto.UnitTests/
│   └── masar.crypto.IntegrationTests/
├── masar.crypto.sln
├── Directory.Build.props
├── .editorconfig
├── .gitignore
└── README.md
```

## Projects

### Source Projects (src/)

#### masar.crypto.Domain

Core business logic, entities, and domain rules

**Location**: `src/masar.crypto.Domain/`

#### masar.crypto.Application

Use cases, interfaces, and application services

**Location**: `src/masar.crypto.Application/`

#### masar.crypto.Infrastructure

Data access, external services, and infrastructure implementations

**Location**: `src/masar.crypto.Infrastructure/`

#### masar.crypto.API

Web API controllers, middleware, and HTTP concerns

**Location**: `src/masar.crypto.API/`


#### API Type

This solution uses **WebAPI** for the API layer.



#### CQRS Structure

This solution includes CQRS (Command Query Responsibility Segregation) structure:
- **Commands**: Located in `Application/UseCases/Commands/`
- **Queries**: Located in `Application/UseCases/Queries/`



#### Entity Framework Core

EF Core infrastructure is configured in the Infrastructure layer:
- **DbContext**: `Infrastructure/Persistence/DbContext/ApplicationDbContext.cs`
- **Configurations**: `Infrastructure/Persistence/Configurations/`
- **Migrations**: `Infrastructure/Persistence/Migrations/`



### Test Projects (tests/)

#### masar.crypto.UnitTests

Unit tests for all layers. Tests individual components in isolation.

**Location**: `tests/masar.crypto.UnitTests/`

#### masar.crypto.IntegrationTests

Integration tests that verify the interaction between layers and external dependencies.

**Location**: `tests/masar.crypto.IntegrationTests/`


## Architecture

### Clean Architecture Principles

This solution enforces Clean Architecture dependency rules:

```
┌─────────────┐
│     API     │
└──────┬──────┘
       │
       ↓
┌─────────────┐
│ Application │
└──────┬──────┘
       │
       ↓
┌─────────────┐
│   Domain    │
└──────┬──────┘
       │
       ↓
┌─────────────┐
│SharedKernel │ ← Innermost layer (no dependencies)
└─────────────┘
       ↑
       │
┌─────────────┐
│Infrastructure│
└──────────────┘
```

### Dependency Rules

- ✅ **Application → Domain**: Application layer depends on Domain

- ✅ **Infrastructure → Application**: Infrastructure implements Application interfaces
- ✅ **API → Application**: API layer depends on Application
- ✅ **Domain → Nothing/SharedKernel**: Domain has no external dependencies (except SharedKernel if selected)


**Important**: Dependencies always point **inward** toward the Domain layer.

### Layer Responsibilities

#### Domain Layer
- **Purpose**: Core business logic and domain rules
- **Contains**: Entities, Value Objects, Aggregates, Domain Services, Domain Events, Specifications
- **Rules**: 
  - No framework dependencies
  - No external references
  - Pure business logic only

#### SharedKernel Layer (Optional)
- **Purpose**: Shared domain concepts and common abstractions (DDD pattern)
- **Contains**: Shared Entities, Value Objects, Enums, Constants, Common Interfaces
- **Rules**:
  - No framework dependencies
  - No external references
  - Shared between multiple bounded contexts
  - Pure domain concepts only

#### Application Layer
- **Purpose**: Application use cases and business workflows
- **Contains**: Use Cases, Interfaces, DTOs, Validators, Mappings
- **Rules**:
  - Depends on Domain (and SharedKernel if selected)
  - Defines interfaces for Infrastructure to implement
  - Contains application-specific business logic

#### Infrastructure Layer
- **Purpose**: External concerns and technical implementations
- **Contains**: Persistence (DbContext, Configurations, Migrations), Repositories, Services, Messaging
- **Rules**:
  - Implements interfaces from Application layer
  - Handles data access, external APIs, file I/O
  - Can depend on Application and Domain

#### API Layer
- **Purpose**: Entry point for external clients
- **Contains**: Controllers, Filters, Middleware, Contracts, Extensions
- **Rules**:
  - Depends only on Application layer
  - No business logic
  - Handles HTTP concerns only

## Getting Started

### Prerequisites

- .NET SDK 9.0 or later
- Your preferred IDE (Visual Studio, Rider, VS Code)

### Setup

1. **Restore NuGet packages**:
   ```bash
   dotnet restore
   ```

2. **Build the solution**:
   ```bash
   dotnet build
   ```

3. **Run tests** (if included):
   ```bash
   dotnet test
   ```

4. **Run the application** (if API layer is included):
   ```bash
   cd src/masar.crypto.API
   dotnet run
   ```

## Development Guidelines

### Adding New Features

1. **Domain Layer**: Start by defining entities and domain rules
2. **Application Layer**: Create use cases and define interfaces
3. **Infrastructure Layer**: Implement the interfaces
4. **API Layer**: Expose endpoints that call use cases

### Code Organization

- **Entities**: Domain entities representing business concepts
- **Value Objects**: Immutable objects defined by their values
- **Aggregates**: Cluster of entities treated as a single unit
- **Use Cases**: Application-specific business operations
- **Repositories**: Data access abstraction (defined in Application, implemented in Infrastructure)
- **DTOs**: Data Transfer Objects for API communication

### Testing Strategy

- **Unit Tests**: Test individual components in isolation
- **Integration Tests**: Test layer interactions and external dependencies
- Run tests frequently: `dotnet test`


## Configuration Files

- **`.editorconfig`**: Code style and formatting rules
- **`Directory.Build.props`**: Common MSBuild properties for all projects
- **`.gitignore`**: Git ignore patterns for .NET projects

## Best Practices

1. ✅ **Keep Domain Pure**: Never add framework dependencies to Domain
2. ✅ **Dependency Direction**: Always point dependencies inward
3. ✅ **Interface Segregation**: Define interfaces in Application, implement in Infrastructure
4. ✅ **Single Responsibility**: Each layer has a clear, single purpose
5. ✅ **Test Coverage**: Write tests for business logic and critical paths

## Resources

- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Domain-Driven Design](https://martinfowler.com/bliki/DomainDrivenDesign.html)
- [.NET Documentation](https://docs.microsoft.com/dotnet/)

## License

[Specify your license here]

---

**Generated by**: Clean Architecture Solution Generator
**Generated on**: 2026-01-03
