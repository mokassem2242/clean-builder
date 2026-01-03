# Clean Architecture Solution Generator

A .NET console application that generates ready-to-use Clean Architecture solutions with Domain-Driven Design (DDD) alignment.

## Features

- ✅ Interactive CLI interface
- ✅ Generates Clean Architecture layers (Domain, Application, Infrastructure, API)
- ✅ Automatic project reference wiring with dependency rule enforcement
- ✅ Predefined folder structures per layer
- ✅ Optional features: CQRS, EF Core setup, Test projects, README
- ✅ Configuration artifacts: `.editorconfig`, `Directory.Build.props`, `.gitignore`
- ✅ Cross-platform support (Windows, macOS, Linux)

## Requirements

- .NET SDK 9.0 or later
- Write permissions to the target directory

## Building

```bash
dotnet build
```

## Running

```bash
dotnet run
```

Or after building:

```bash
dotnet run --project CleanArchitectureGenerator
```

## Usage

1. Run the generator:
   ```bash
   dotnet run
   ```

2. Follow the interactive prompts:
   - Enter solution name (e.g., `Company.Product`)
   - Enter base namespace (defaults to solution name)
   - Select .NET version (defaults to `net9.0`)
   - Select layers to include (Domain is mandatory)
   - Choose optional features

3. The solution will be generated in the current directory (or specified directory)

## Generated Structure

The generator creates a solution with the following structure:

```
SolutionName/
├── SolutionName.sln
├── Company.Product.Domain/
│   ├── Entities/
│   ├── ValueObjects/
│   ├── Aggregates/
│   ├── Specifications/
│   ├── DomainServices/
│   ├── DomainEvents/
│   ├── Exceptions/
│   └── Common/
├── Company.Product.Application/
│   ├── UseCases/
│   ├── Interfaces/
│   ├── DTOs/
│   ├── Validators/
│   ├── Mappings/
│   └── Common/
├── Company.Product.Infrastructure/
│   ├── Persistence/
│   │   ├── DbContext/
│   │   ├── Configurations/
│   │   └── Migrations/
│   ├── Repositories/
│   ├── Services/
│   └── Messaging/
└── Company.Product.API/
    ├── Controllers/
    ├── Filters/
    ├── Middleware/
    ├── Contracts/
    └── Extensions/
```

## Architecture

The generator itself follows a clean structure:

- **Models/**: Configuration and data models
- **Services/**: Core business logic
  - `CLIInterface`: Interactive user interface
  - `SolutionGenerator`: Solution and project generation
  - `LayerConfigurationService`: Layer definitions
  - `InputValidator`: Input validation

## License

This tool is provided as-is for generating Clean Architecture solutions.

