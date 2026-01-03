# Folder Structure Enforcement

## ✅ Structure Rules

The generator enforces a strict folder structure:

### `src/` Folder
- **Contains**: All source code projects (Domain, Application, Infrastructure, API)
- **Does NOT contain**: Any test projects or test files
- **Purpose**: Production code only

### `tests/` Folder
- **Contains**: All test projects (UnitTests, IntegrationTests)
- **Contains**: All test files and test-related code
- **Purpose**: Testing code only

## Implementation

### Source Projects (src/)
```csharp
// Create src folder for all source code projects (NO test projects here)
var srcPath = Path.Combine(solutionPath, "src");
Directory.CreateDirectory(srcPath);

// Generate source projects for each selected layer in src folder only
foreach (var layer in config.SelectedLayers)
{
    var projectPath = await CreateProjectAsync(srcPath, config, layer, definition);
    // Project is created in src/ folder
}
```

### Test Projects (tests/)
```csharp
// Create tests folder for ALL test-related projects and files
// IMPORTANT: ALL test projects and test files MUST be in tests/ folder, NOT in src/
var testsPath = Path.Combine(solutionPath, "tests");
Directory.CreateDirectory(testsPath);
await CreateTestProjectsAsync(testsPath, solutionFile, config, projectPaths);
```

## Generated Structure

```
SolutionName/
├── src/                          # Source code ONLY
│   ├── Company.Product.Domain/
│   ├── Company.Product.Application/
│   ├── Company.Product.Infrastructure/
│   └── Company.Product.API/
├── tests/                        # Test code ONLY
│   ├── Company.Product.UnitTests/
│   │   ├── UnitTest.cs          # Test file
│   │   └── Company.Product.UnitTests.csproj
│   └── Company.Product.IntegrationTests/
│       ├── IntegrationTest.cs   # Test file
│       └── Company.Product.IntegrationTests.csproj
├── SolutionName.sln
├── Directory.Build.props
├── .editorconfig
└── README.md
```

## Validation

The code includes explicit comments and validation to ensure:
1. ✅ Source projects are only created in `src/`
2. ✅ Test projects are only created in `tests/`
3. ✅ Test files are only created inside test projects (which are in `tests/`)
4. ✅ Clear separation between source and test code

## Benefits

1. **Clear Organization**: Easy to distinguish between source and test code
2. **CI/CD Friendly**: Standard structure works well with build pipelines
3. **Best Practices**: Follows .NET solution structure conventions
4. **Maintainability**: Clear separation makes the codebase easier to navigate

