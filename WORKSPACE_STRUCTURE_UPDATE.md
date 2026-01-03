# Workspace Structure Update - src/ and tests/ Folders

## ✅ Changes Implemented

The generator now organizes projects into a standard workspace structure:

```
SolutionName/
├── src/                          # All layer projects
│   ├── Company.Product.Domain/
│   ├── Company.Product.Application/
│   ├── Company.Product.Infrastructure/
│   └── Company.Product.API/
├── tests/                        # All test projects
│   ├── Company.Product.UnitTests/
│   └── Company.Product.IntegrationTests/
├── SolutionName.sln
├── Directory.Build.props
├── .editorconfig
└── README.md
```

## Changes Made

### 1. Source Projects in `src/` Folder
- All layer projects (Domain, Application, Infrastructure, API) are now created in the `src/` folder
- Updated `CreateProjectAsync()` to accept `srcPath` parameter
- Projects are organized as: `src/Company.Product.Layer/`

### 2. Test Projects in `tests/` Folder
- All test projects are now created in the `tests/` folder
- Updated `CreateTestProjectsAsync()` to accept `testsPath` parameter
- Test projects are organized as: `tests/Company.Product.UnitTests/` and `tests/Company.Product.IntegrationTests/`

### 3. Project References
- Project references are automatically calculated with correct relative paths
- References from test projects to source projects use paths like: `../../src/Company.Product.Domain/Company.Product.Domain.csproj`
- References between source projects use paths like: `../Company.Product.Domain/Company.Product.Domain.csproj`

### 4. Solution File
- Solution file correctly includes all projects with proper relative paths
- Projects in `src/` and `tests/` are both included in the solution

### 5. README Update
- README now shows the new folder structure
- Includes visual representation of the workspace organization

## Benefits

1. **Standard Structure**: Follows common .NET workspace conventions
2. **Clear Separation**: Source code and tests are clearly separated
3. **Better Organization**: Easier to navigate and understand the solution structure
4. **CI/CD Friendly**: Standard structure works well with build pipelines

## Testing

To test the new structure:

1. Run the generator:
   ```bash
   dotnet run
   ```

2. Generate a solution with all layers and tests

3. Verify the structure:
   ```bash
   cd GeneratedSolutionName
   tree /F  # Windows
   # or
   tree      # Linux/macOS
   ```

4. Build the solution:
   ```bash
   dotnet build
   ```

## Note

If you have a previously generated solution in the generator directory (like `masar.test`), you may need to move or delete it to avoid build conflicts when building the generator itself.

