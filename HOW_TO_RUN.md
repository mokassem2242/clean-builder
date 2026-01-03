# How to Run the Clean Architecture Generator

## Quick Start

### Method 1: Using `dotnet run` (Recommended)

Navigate to the generator project directory and run:

```bash
cd CleanArchitectureGenerator
dotnet run
```

Or from the root directory:

```bash
dotnet run --project CleanArchitectureGenerator
```

### Method 2: Using the Compiled Executable

If you've already built the project, you can run the executable directly:

```bash
cd CleanArchitectureGenerator
.\bin\Debug\net9.0\CleanArchitectureGenerator.exe
```

Or from anywhere:

```bash
.\CleanArchitectureGenerator\bin\Debug\net9.0\CleanArchitectureGenerator.exe
```

### Method 3: Build and Run in One Step

```bash
cd CleanArchitectureGenerator
dotnet build && dotnet run
```

## Step-by-Step Example

1. **Open terminal/PowerShell** in the project root directory:
   ```bash
   W:\new mentality\clean builder>
   ```

2. **Navigate to the generator project**:
   ```bash
   cd CleanArchitectureGenerator
   ```

3. **Run the generator**:
   ```bash
   dotnet run
   ```

4. **Follow the interactive prompts**:
   ```
   ╔══════════════════════════════════════════════════════════════╗
   ║   Clean Architecture Solution Generator                    ║
   ╚══════════════════════════════════════════════════════════════╝

   Solution Name (e.g., Company.Product): Acme.ECommerce
   Base Namespace (default: Acme.ECommerce): [Press Enter]
   .NET Version (default: net9.0): [Press Enter]

   Select layers to include (Domain is mandatory):
     Include Domain layer? (mandatory) (Y/n, default: Y): [Press Enter]
     Include Application layer? (Y/n, default: N): Y
     Include Infrastructure layer? (Y/n, default: N): Y
     Include API layer? (Y/n, default: N): Y

   Optional Features:
     Include CQRS structure (Commands/Queries)? (Y/n, default: N): Y
     Include EF Core infrastructure setup? (Y/n, default: N): Y
     Include test projects (Unit/Integration)? (Y/n, default: N): Y
     Generate README with architecture guidelines? (Y/n, default: Y): [Press Enter]

   ╔══════════════════════════════════════════════════════════════╗
   ║                    Configuration Summary                     ║
   ╚══════════════════════════════════════════════════════════════╝
   Solution Name:     Acme.ECommerce
   Base Namespace:    Acme.ECommerce
   .NET Version:      net9.0
   Selected Layers:   Domain, Application, Infrastructure, API
   CQRS:              Yes
   EF Core:           Yes
   Test Projects:     Yes
   README:            Yes

   Proceed with generation? (Y/n, default: Y): [Press Enter]
   ```

5. **The solution will be generated** in the current directory (or specified directory)

6. **Navigate to the generated solution**:
   ```bash
   cd Acme.ECommerce
   ```

7. **Restore and build**:
   ```bash
   dotnet restore
   dotnet build
   ```

## Specifying Output Directory

You can optionally specify an output directory as an argument:

```bash
dotnet run -- "C:\Projects\MySolutions"
```

This will generate the solution in the specified directory instead of the current directory.

## Troubleshooting

### Error: "Could not find project or directory"
- Make sure you're in the correct directory
- Verify the `CleanArchitectureGenerator.csproj` file exists

### Error: ".NET SDK not found"
- Install .NET SDK 9.0 or later
- Verify installation: `dotnet --version`

### Error: "Permission denied"
- Ensure you have write permissions to the output directory
- On Windows, you may need to run as Administrator

## Requirements

- .NET SDK 9.0 or later
- Write permissions to the target directory

## What Happens When You Run It?

1. The generator prompts you for configuration
2. Validates your inputs
3. Shows a summary of your selections
4. Asks for confirmation
5. Generates the complete Clean Architecture solution with:
   - Solution file (.sln)
   - All selected projects
   - Folder structures
   - Project references
   - Configuration files
   - Optional features (if selected)

## Next Steps After Generation

Once the solution is generated:

1. Navigate to the solution directory
2. Restore NuGet packages: `dotnet restore`
3. Build the solution: `dotnet build`
4. Run tests (if included): `dotnet test`
5. Start coding! 🚀

