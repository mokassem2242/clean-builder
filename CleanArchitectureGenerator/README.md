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
dotnet build
```

## 🎯 Running

### Quick Start

```bash
dotnet run
```

### From Root Directory

```bash
dotnet run --project CleanArchitectureGenerator
```

### Using Compiled Executable

```bash
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

## 📁 Generated Solution Structure

The generator creates a solution with the following structure:

```
SolutionName/
├── src/                          # Source code projects
│   ├── Company.Product.Domain/
│   ├── Company.Product.Application/
│   ├── Company.Product.Infrastructure/
│   └── Company.Product.API/
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

- ✅ Application → Domain
- ✅ Infrastructure → Application
- ✅ API → Application
- ✅ Domain → Nothing (enforced)

### Project Structure

The generator itself follows a clean structure:

- **Models/**: Configuration and data models
- **Services/**: Core business logic
  - `CLIInterface`: Interactive user interface
  - `SolutionGenerator`: Solution and project generation
  - `LayerConfigurationService`: Layer definitions
  - `InputValidator`: Input validation
  - `DependencyValidator`: Dependency rule validation

## 🎨 API Types

- **Web API**: Traditional ASP.NET Core Web API with Controllers
- **Minimal API**: Lightweight ASP.NET Core Minimal API
- **gRPC**: gRPC service with Grpc.AspNetCore

## ✅ Validation

The generator includes comprehensive validation:

- ✅ Solution name format validation
- ✅ Namespace format validation
- ✅ .NET version format validation
- ✅ Dependency rule enforcement
- ✅ Duplicate project prevention
- ✅ Cross-platform path handling

## 🚦 Next Steps After Generation

1. Navigate to the solution: `cd SolutionName`
2. Restore packages: `dotnet restore`
3. Build the solution: `dotnet build`
4. Run tests: `dotnet test`
5. Start coding! 🎉

## 📚 Resources

- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Domain-Driven Design](https://martinfowler.com/bliki/DomainDrivenDesign.html)
- [.NET Documentation](https://docs.microsoft.com/dotnet/)

## 📄 License

This tool is provided as-is for generating Clean Architecture solutions.

