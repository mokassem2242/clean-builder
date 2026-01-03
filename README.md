# Clean Architecture Solution Generator

A powerful .NET console application that generates ready-to-use Clean Architecture solutions with Domain-Driven Design (DDD) alignment. Eliminate repetitive project setup and enforce architectural boundaries by default.

## 🚀 Features

- ✅ **Interactive CLI Interface** - User-friendly prompts with sensible defaults
- ✅ **Clean Architecture Layers** - Generates Domain, Application, Infrastructure, and API layers
- ✅ **Multiple API Types** - Support for Web API, Minimal API, and gRPC
- ✅ **Automatic Dependency Management** - Enforces Clean Architecture dependency rules
- ✅ **Organized Structure** - Projects organized in `src/` and `tests/` folders with Visual Studio solution folders
- ✅ **Predefined Folder Structures** - DDD-aligned folder hierarchies per layer
- ✅ **Optional Features**:
  - CQRS structure (Commands/Queries)
  - EF Core infrastructure setup
  - Test projects (Unit/Integration)
  - Comprehensive README generation
- ✅ **Configuration Artifacts** - `.editorconfig`, `Directory.Build.props`, `.gitignore`
- ✅ **Cross-Platform** - Works on Windows, macOS, and Linux

## 📋 Requirements

- .NET SDK 9.0 or later
- Write permissions to the target directory

## 🏗️ Building

```bash
cd CleanArchitectureGenerator
dotnet build
```

## 🎯 Running

### Quick Start

```bash
cd CleanArchitectureGenerator
dotnet run
```

### From Root Directory

```bash
dotnet run --project CleanArchitectureGenerator
```

### Using Compiled Executable

```bash
cd CleanArchitectureGenerator
.\bin\Debug\net9.0\CleanArchitectureGenerator.exe
```

## 📖 Usage

### Basic Usage

1. **Run the generator**:
   ```bash
   dotnet run
   ```

2. **Follow the interactive prompts**:
   - Enter solution name (e.g., `Company.Product`)
   - Enter base namespace (defaults to solution name)
   - Select .NET version (defaults to `net9.0`)
   - Select layers to include (Domain is mandatory)
   - Choose API type if API layer is selected
   - Select optional features

3. **The solution is generated** in the current directory

### Example Workflow

```bash
$ dotnet run

╔══════════════════════════════════════════════════════════════╗
║   Clean Architecture Solution Generator                    ║
╚══════════════════════════════════════════════════════════════╝

Solution Name (e.g., Company.Product): Acme.ECommerce
Base Namespace (default: Acme.ECommerce): [Enter]
.NET Version (default: net9.0): [Enter]

Select layers to include (Domain is mandatory):
  Include Domain layer? (mandatory) (Y/n, default: Y): [Enter]
  Include Application layer? (Y/n, default: N): Y
  Include Infrastructure layer? (Y/n, default: N): Y
  Include API layer? (Y/n, default: N): Y

Select API Type:
  1. Web API (Traditional ASP.NET Core with Controllers) - Default
  2. Minimal API (ASP.NET Core Minimal API)
  3. gRPC (gRPC Service)
Enter choice (1-3, default: 1): [Enter]

Optional Features:
  Include CQRS structure (Commands/Queries)? (Y/n, default: N): Y
  Include EF Core infrastructure setup? (Y/n, default: N): Y
  Include test projects (Unit/Integration)? (Y/n, default: N): Y

╔══════════════════════════════════════════════════════════════╗
║                    Configuration Summary                     ║
╚══════════════════════════════════════════════════════════════╝
Solution Name:     Acme.ECommerce
Base Namespace:    Acme.ECommerce
.NET Version:      net9.0
Selected Layers:   Domain, Application, Infrastructure, API
API Type:          WebAPI
CQRS:              Yes
EF Core:           Yes
Test Projects:     Yes

Proceed with generation? (Y/n, default: Y): [Enter]

🚀 Generating solution: Acme.ECommerce
...
✅ Solution generated successfully!
```

## 📁 Generated Solution Structure

The generator creates a solution with the following structure:

```
SolutionName/
├── src/                          # Source code projects
│   ├── Company.Product.Domain/
│   │   ├── Entities/
│   │   ├── ValueObjects/
│   │   ├── Aggregates/
│   │   ├── Specifications/
│   │   ├── DomainServices/
│   │   ├── DomainEvents/
│   │   ├── Exceptions/
│   │   └── Common/
│   ├── Company.Product.Application/
│   │   ├── UseCases/
│   │   │   ├── Commands/        # If CQRS enabled
│   │   │   └── Queries/         # If CQRS enabled
│   │   ├── Interfaces/
│   │   ├── DTOs/
│   │   ├── Validators/
│   │   ├── Mappings/
│   │   └── Common/
│   ├── Company.Product.Infrastructure/
│   │   ├── Persistence/
│   │   │   ├── DbContext/
│   │   │   ├── Configurations/
│   │   │   └── Migrations/
│   │   ├── Repositories/
│   │   ├── Services/
│   │   └── Messaging/
│   └── Company.Product.API/
│       ├── Controllers/         # Or Services for gRPC
│       ├── Filters/
│       ├── Middleware/
│       ├── Contracts/
│       └── Extensions/
├── tests/                        # Test projects
│   ├── Company.Product.UnitTests/
│   └── Company.Product.IntegrationTests/
├── SolutionName.sln
├── Directory.Build.props
├── .editorconfig
├── .gitignore
└── README.md
```

## 🏛️ Architecture

### Dependency Rules Enforced

The generator automatically enforces Clean Architecture dependency rules:

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
│   Domain    │ ← No dependencies
└─────────────┘
       ↑
       │
┌─────────────┐
│Infrastructure│
└──────────────┘
```

- ✅ Application → Domain
- ✅ Infrastructure → Application
- ✅ API → Application
- ✅ Domain → Nothing (enforced)

### Layer Descriptions

- **Domain**: Core business logic, entities, value objects, domain services. No framework dependencies.
- **Application**: Use cases, interfaces, DTOs, application services. Depends only on Domain.
- **Infrastructure**: Data access, external services, infrastructure implementations. Implements Application interfaces.
- **API**: Web API controllers, middleware, HTTP concerns. Depends only on Application.

## 🎨 API Types

### Web API (Default)
Traditional ASP.NET Core Web API with Controllers, Swagger, and standard middleware pipeline.

### Minimal API
Lightweight ASP.NET Core Minimal API with endpoint-based routing and Swagger support.

### gRPC
gRPC service with Grpc.AspNetCore package and service-based architecture.

## 🛠️ Project Structure

The generator itself follows a clean structure:

```
CleanArchitectureGenerator/
├── Models/
│   ├── SolutionConfiguration.cs    # Configuration model
│   └── LayerDefinition.cs          # Layer definition model
├── Services/
│   ├── CLIInterface.cs             # Interactive CLI prompts
│   ├── SolutionGenerator.cs        # Core generation logic
│   ├── LayerConfigurationService.cs # Layer definitions
│   ├── InputValidator.cs           # Input validation
│   └── DependencyValidator.cs      # Dependency rule validation
├── Program.cs                       # Application entry point
└── README.md                        # This file
```

## 🔧 Configuration

### Solution Configuration

The generator prompts for:
- **Solution Name**: Name of the solution (e.g., `Company.Product`)
- **Base Namespace**: Base namespace for all projects (defaults to solution name)
- **.NET Version**: Target framework version (defaults to `net9.0`)
- **Layers**: Which layers to include (Domain is mandatory)
- **API Type**: Type of API to generate (if API layer is selected)
- **Optional Features**: CQRS, EF Core, Tests

### Generated Configuration Files

- **`.editorconfig`**: Code style and formatting rules
- **`Directory.Build.props`**: Common MSBuild properties
- **`.gitignore`**: Git ignore patterns for .NET
- **`README.md`**: Comprehensive solution documentation

## ✅ Validation

The generator includes comprehensive validation:

- ✅ Solution name format validation
- ✅ Namespace format validation
- ✅ .NET version format validation
- ✅ Dependency rule enforcement
- ✅ Duplicate project prevention
- ✅ Cross-platform path handling

## 🚦 Next Steps After Generation

1. **Navigate to the solution**:
   ```bash
   cd SolutionName
   ```

2. **Restore packages**:
   ```bash
   dotnet restore
   ```

3. **Build the solution**:
   ```bash
   dotnet build
   ```

4. **Run tests** (if included):
   ```bash
   dotnet test
   ```

5. **Start coding!** 🎉

## 📝 Examples

### Generate Minimal Solution (Domain Only)

```bash
dotnet run
# Select only Domain layer
```

### Generate Full Solution with All Features

```bash
dotnet run
# Select all layers, all optional features
```

### Generate API-Only Solution

```bash
dotnet run
# Select Domain, Application, and API layers
# Choose Minimal API or gRPC
```

## 🐛 Troubleshooting

### Error: "Could not find project or directory"
- Ensure you're in the correct directory
- Verify the `CleanArchitectureGenerator.csproj` file exists

### Error: ".NET SDK not found"
- Install .NET SDK 9.0 or later
- Verify installation: `dotnet --version`

### Error: "Permission denied"
- Ensure you have write permissions to the output directory
- On Windows, you may need to run as Administrator

### Error: "Project already exists in solution"
- This has been fixed in the latest version
- Regenerate the solution if you see this error

## 🔮 Future Enhancements

Potential future features (not currently implemented):
- Convert to `dotnet new` template
- Package as .NET global tool
- Support modular monolith configuration
- Add OpenAPI, logging, and health checks options
- Support for additional API types (GraphQL, SignalR)

## 📚 Resources

- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Domain-Driven Design](https://martinfowler.com/bliki/DomainDrivenDesign.html)
- [.NET Documentation](https://docs.microsoft.com/dotnet/)
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core/)

## 📄 License

This tool is provided as-is for generating Clean Architecture solutions.

## 🤝 Contributing

Contributions are welcome! Please ensure your code follows the existing patterns and includes appropriate validation.

## 📧 Support

For issues, questions, or suggestions, please refer to the project documentation or create an issue in the repository.

---

**Version**: 1.0.0  
**Last Updated**: 2024  
**Author**: Clean Architecture Solution Generator

