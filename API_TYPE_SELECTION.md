# API Type Selection Feature

## ✅ Implementation Complete

The generator now supports selecting different API types when the API layer is included.

## Supported API Types

1. **Web API** (Default)
   - Traditional ASP.NET Core Web API with Controllers
   - Uses `Microsoft.NET.Sdk.Web` SDK
   - Includes Controllers, Filters, Middleware folders
   - Generated Program.cs with controller-based setup

2. **Minimal API**
   - ASP.NET Core Minimal API
   - Uses `Microsoft.NET.Sdk.Web` SDK
   - Includes Controllers, Filters, Middleware folders (for consistency)
   - Generated Program.cs with endpoint-based setup

3. **gRPC**
   - gRPC Service
   - Uses `Microsoft.NET.Sdk.Web` SDK
   - Includes Grpc.AspNetCore NuGet package
   - Uses Services folder instead of Controllers
   - Generated Program.cs with gRPC setup

## Changes Made

### 1. Models (`SolutionConfiguration.cs`)
- Added `ApiType` enum with WebAPI, MinimalAPI, gRPC options
- Added `SelectedApiType` property to `SolutionConfiguration`

### 2. CLI Interface (`CLIInterface.cs`)
- Added `PromptApiType()` method to prompt user for API type selection
- Shows menu with 3 options (1-3)
- Defaults to WebAPI if no selection is made
- Updated `DisplaySummary()` to show selected API type

### 3. Solution Generator (`SolutionGenerator.cs`)
- Updated `CreateProjectAsync()` to handle different API types:
  - Different SDKs and NuGet packages based on type
  - gRPC includes Grpc.AspNetCore package
- Added `CreateApiProgramFileAsync()` method:
  - Generates appropriate Program.cs for each API type
  - WebAPI: Controller-based setup with Swagger
  - MinimalAPI: Endpoint-based setup with Swagger
  - gRPC: gRPC service setup
- Updated `CreateFolderStructureAsync()`:
  - gRPC uses "Services" folder instead of "Controllers"

## User Experience

When the user selects the API layer, they will see:

```
Select API Type:
  1. Web API (Traditional ASP.NET Core with Controllers) - Default
  2. Minimal API (ASP.NET Core Minimal API)
  3. gRPC (gRPC Service)
Enter choice (1-3, default: 1):
```

The selection is then shown in the configuration summary:

```
╔══════════════════════════════════════════════════════════════╗
║                    Configuration Summary                     ║
╚══════════════════════════════════════════════════════════════╝
Solution Name:     Company.Product
Base Namespace:    Company.Product
.NET Version:      net9.0
Selected Layers:   Domain, Application, Infrastructure, API
API Type:          WebAPI
...
```

## Generated Code Examples

### Web API Program.cs
```csharp
using Company.Product.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

### Minimal API Program.cs
```csharp
using Company.Product.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello from Minimal API!");

app.Run();
```

### gRPC Program.cs
```csharp
using Company.Product.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var app = builder.Build();

// TODO: Add your gRPC services here
// app.MapGrpcService<YourService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client...");

app.Run();
```

## Folder Structure by API Type

### Web API & Minimal API
```
API/
├── Controllers/
├── Filters/
├── Middleware/
├── Contracts/
└── Extensions/
```

### gRPC
```
API/
├── Services/      (instead of Controllers)
├── Filters/
├── Middleware/
├── Contracts/
└── Extensions/
```

## Benefits

1. **Flexibility**: Choose the right API style for your project
2. **Best Practices**: Each type generates code following best practices
3. **Ready to Use**: Generated code is ready to run with appropriate setup
4. **Extensible**: Easy to add more API types in the future

## Future Enhancements

Potential additional API types:
- GraphQL
- SignalR
- WebSocket
- Blazor Server/WebAssembly

