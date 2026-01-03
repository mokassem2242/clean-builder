# Masar.crypto

## Architecture

This solution follows **Clean Architecture** principles with **Domain-Driven Design (DDD)** alignment.

### Solution Structure

```
Masar.crypto/
├── src/                          # Source code projects
│   ├── Masar.crypto.Domain/
│   ├── Masar.crypto.Application/
│   ├── Masar.crypto.Infrastructure/
│   ├── Masar.crypto.API/
├── tests/                      # Test projects
│   ├── Masar.crypto.UnitTests/
│   └── Masar.crypto.IntegrationTests/
├── Masar.crypto.sln
├── Directory.Build.props
├── .editorconfig
└── README.md
```

### Project Structure

- **Masar.crypto.Domain** (src/): Core business logic, entities, and domain rules
- **Masar.crypto.Application** (src/): Use cases, interfaces, and application services
- **Masar.crypto.Infrastructure** (src/): Data access, external services, and infrastructure implementations
- **Masar.crypto.API** (src/): Web API controllers, middleware, and HTTP concerns

### Test Projects

- **Masar.crypto.UnitTests** (tests/): Unit tests for all layers
- **Masar.crypto.IntegrationTests** (tests/): Integration tests

### Dependency Rules

- Application → Domain
- Infrastructure → Application
- API → Application
- Domain MUST NOT reference any other project

### Getting Started

1. Restore packages:
   ```bash
   dotnet restore
   ```

2. Build the solution:
   ```bash
   dotnet build
   ```

3. Run tests (if applicable):
   ```bash
   dotnet test
   ```

### Architecture Guidelines

- **Domain Layer**: Contains business logic, entities, value objects, and domain services. No framework dependencies.
- **Application Layer**: Contains use cases, interfaces, DTOs, and application services.
- **Infrastructure Layer**: Implements interfaces from Application layer (repositories, external services).
- **API Layer**: Contains controllers, middleware, and API-specific concerns. No business logic.

### Development

Follow Clean Architecture principles:
- Dependencies point inward (toward Domain)
- Domain is framework-agnostic
- Business logic resides in Domain and Application layers
