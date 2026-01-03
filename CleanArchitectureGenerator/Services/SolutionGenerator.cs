using System.Diagnostics;
using CleanArchitectureGenerator.Models;

namespace CleanArchitectureGenerator.Services;

public class SolutionGenerator
{
    private readonly string _outputDirectory;

    public SolutionGenerator(string outputDirectory)
    {
        _outputDirectory = outputDirectory;
    }

    public async Task<bool> GenerateSolutionAsync(SolutionConfiguration config)
    {
        try
        {
            Console.WriteLine($"\n🚀 Generating solution: {config.SolutionName}");
            Console.WriteLine($"📁 Output directory: {_outputDirectory}\n");

            // Create solution directory
            var solutionPath = Path.Combine(_outputDirectory, config.SolutionName);
            Directory.CreateDirectory(solutionPath);

            // Create solution file
            var solutionFile = Path.Combine(solutionPath, $"{config.SolutionName}.sln");
            await CreateSolutionFileAsync(solutionFile, config);

            // Create src folder for all source code projects (NO test projects here)
            var srcPath = Path.Combine(solutionPath, "src");
            Directory.CreateDirectory(srcPath);

            // Generate source projects for each selected layer in src folder only
            // IMPORTANT: Only source code projects go in src/ - test projects go in tests/
            var layerDefinitions = LayerConfigurationService.GetLayerDefinitions();
            var projectPaths = new Dictionary<LayerType, string>();

            foreach (var layer in config.SelectedLayers)
            {
                // Ensure we're only creating source projects, not test projects
                var definition = layerDefinitions[layer];
                var projectPath = await CreateProjectAsync(srcPath, config, layer, definition);
                projectPaths[layer] = projectPath;

                // Create folder structure
                await CreateFolderStructureAsync(projectPath, definition, config);

                // Remove default boilerplate files (but preserve Program.cs for API projects)
                await RemoveBoilerplateFilesAsync(projectPath, layer);
            }

            // Add project references
            await AddProjectReferencesAsync(solutionPath, config, projectPaths, layerDefinitions);

            // Add projects to solution with solution folders
            await AddProjectsToSolutionAsync(solutionFile, projectPaths, config);

            // Generate optional features
            if (config.IncludeCQRS)
            {
                await CreateCQRSStructureAsync(projectPaths, config);
            }

            if (config.IncludeEFCore && config.SelectedLayers.Contains(LayerType.Infrastructure))
            {
                await CreateEFCorePlaceholdersAsync(projectPaths[LayerType.Infrastructure], config);
            }

            if (config.IncludeTests)
            {
                // Create tests folder for ALL test-related projects and files
                // IMPORTANT: ALL test projects and test files MUST be in tests/ folder, NOT in src/
                var testsPath = Path.Combine(solutionPath, "tests");
                Directory.CreateDirectory(testsPath);
                await CreateTestProjectsAsync(testsPath, solutionFile, config, projectPaths);
            }

            // Generate configuration artifacts
            await GenerateConfigurationArtifactsAsync(solutionPath, config);

            Console.WriteLine($"\n✅ Solution generated successfully!");
            Console.WriteLine($"📂 Location: {solutionPath}\n");

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ Error generating solution: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return false;
        }
    }

    private async Task CreateSolutionFileAsync(string solutionFile, SolutionConfiguration config)
    {
        var content = $@"Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 17
VisualStudioVersion = 17.0.31903.59
MinimumVisualStudioVersion = 10.0.40219.1
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
EndGlobal
";
        await File.WriteAllTextAsync(solutionFile, content);
        Console.WriteLine($"✓ Created solution file: {Path.GetFileName(solutionFile)}");
    }

    private async Task<string> CreateProjectAsync(string srcPath, SolutionConfiguration config, LayerType layer, LayerDefinition definition)
    {
        // This method only creates source code projects in src/ folder
        // Test projects are created separately in CreateTestProjectsAsync() in tests/ folder
        var projectName = $"{config.BaseNamespace}.{layer}";
        var projectPath = Path.Combine(srcPath, projectName);
        Directory.CreateDirectory(projectPath);

        var projectFile = Path.Combine(projectPath, $"{projectName}.csproj");

        string projectContent;
        string sdk = "Microsoft.NET.Sdk";

        // Handle API projects with different types
        if (layer == LayerType.API)
        {
            sdk = "Microsoft.NET.Sdk.Web";
            
            switch (config.SelectedApiType)
            {
                case ApiType.WebAPI:
                    projectContent = $@"<Project Sdk=""{sdk}"">

  <PropertyGroup>
    <TargetFramework>{config.TargetFramework}</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <RootNamespace>{config.BaseNamespace}.{layer}</RootNamespace>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""Swashbuckle.AspNetCore"" Version=""6.9.0"" />
  </ItemGroup>

</Project>
";
                    break;

                case ApiType.MinimalAPI:
                    projectContent = $@"<Project Sdk=""{sdk}"">

  <PropertyGroup>
    <TargetFramework>{config.TargetFramework}</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <RootNamespace>{config.BaseNamespace}.{layer}</RootNamespace>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""Swashbuckle.AspNetCore"" Version=""6.9.0"" />
  </ItemGroup>

</Project>
";
                    break;

                case ApiType.gRPC:
                    projectContent = $@"<Project Sdk=""{sdk}"">

  <PropertyGroup>
    <TargetFramework>{config.TargetFramework}</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <RootNamespace>{config.BaseNamespace}.{layer}</RootNamespace>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""Grpc.AspNetCore"" Version=""2.62.0"" />
  </ItemGroup>

</Project>
";
                    break;

                default:
                    projectContent = $@"<Project Sdk=""{sdk}"">

  <PropertyGroup>
    <TargetFramework>{config.TargetFramework}</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <RootNamespace>{config.BaseNamespace}.{layer}</RootNamespace>
  </PropertyGroup>

</Project>
";
                    break;
            }
        }
        else
        {
            // Non-API projects (Domain, Application, Infrastructure)
            projectContent = $@"<Project Sdk=""{sdk}"">

  <PropertyGroup>
    <TargetFramework>{config.TargetFramework}</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <RootNamespace>{config.BaseNamespace}.{layer}</RootNamespace>
  </PropertyGroup>

</Project>
";
        }

        await File.WriteAllTextAsync(projectFile, projectContent);
        
        var apiTypeInfo = layer == LayerType.API ? $" ({config.SelectedApiType})" : "";
        Console.WriteLine($"✓ Created project: {projectName} ({definition.ProjectType}{apiTypeInfo})");

        // Create Program.cs for API projects based on type
        if (layer == LayerType.API)
        {
            await CreateApiProgramFileAsync(projectPath, config);
        }

        return projectPath;
    }

    private async Task CreateApiProgramFileAsync(string apiPath, SolutionConfiguration config)
    {
        var programFile = Path.Combine(apiPath, "Program.cs");
        string programContent;

        switch (config.SelectedApiType)
        {
            case ApiType.WebAPI:
                programContent = $@"var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{{
    app.UseSwagger();
    app.UseSwaggerUI();
}}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
";
                break;

            case ApiType.MinimalAPI:
                programContent = $@"var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{{
    app.UseSwagger();
    app.UseSwaggerUI();
}}

app.UseHttpsRedirection();

// Map minimal API endpoints
app.MapGet(""/"", () => ""Hello from Minimal API!"");

app.Run();
";
                break;

            case ApiType.gRPC:
                programContent = $@"var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var app = builder.Build();

// Configure the HTTP request pipeline
// TODO: Add your gRPC services here
// app.MapGrpcService<YourService>();
app.MapGet(""/"", () => ""Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909"");

app.Run();
";
                break;

            default:
                programContent = $@"var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet(""/"", () => ""Hello World!"");

app.Run();
";
                break;
        }

        await File.WriteAllTextAsync(programFile, programContent);
        Console.WriteLine($"  ✓ Created Program.cs for {config.SelectedApiType}");
    }

    private async Task CreateFolderStructureAsync(string projectPath, LayerDefinition definition, SolutionConfiguration config)
    {
        var foldersToCreate = definition.Folders.ToList();

        // Adjust folder structure for different API types
        if (definition.Type == LayerType.API)
        {
            switch (config.SelectedApiType)
            {
                case ApiType.MinimalAPI:
                    // Minimal API might not need Controllers folder, but keep it for consistency
                    // Could add Endpoints folder instead
                    break;

                case ApiType.gRPC:
                    // gRPC uses Services instead of Controllers
                    foldersToCreate = foldersToCreate.Select(f => f.Replace("Controllers", "Services")).ToList();
                    break;

                case ApiType.WebAPI:
                default:
                    // Keep default structure
                    break;
            }
        }

        foreach (var folder in foldersToCreate)
        {
            var fullPath = Path.Combine(projectPath, folder);
            Directory.CreateDirectory(fullPath);

            // Create .gitkeep file to preserve empty folders
            var gitkeepPath = Path.Combine(fullPath, ".gitkeep");
            await File.WriteAllTextAsync(gitkeepPath, string.Empty);
        }
        Console.WriteLine($"  ✓ Created folder structure ({foldersToCreate.Count} folders)");
    }

    private Task RemoveBoilerplateFilesAsync(string projectPath, LayerType layer)
    {
        var projectName = Path.GetFileName(projectPath);
        
        // For API projects, don't delete Program.cs (it's needed as entry point)
        // For other projects, remove any Program.cs that might have been created by templates
        var boilerplateFiles = Directory.GetFiles(projectPath, "*.cs", SearchOption.TopDirectoryOnly)
            .Where(f => 
            {
                var fileName = Path.GetFileName(f);
                // Always remove Class files
                if (fileName.StartsWith("Class"))
                    return true;
                
                // For API projects, keep Program.cs (it's the entry point)
                // For other projects, remove Program.cs if it exists (from templates)
                if (fileName.StartsWith("Program"))
                {
                    return layer != LayerType.API; // Only remove if NOT API project
                }
                
                return false;
            })
            .ToList();

        foreach (var file in boilerplateFiles)
        {
            File.Delete(file);
        }

        if (boilerplateFiles.Any())
        {
            Console.WriteLine($"  ✓ Removed {boilerplateFiles.Count} boilerplate file(s)");
        }

        return Task.CompletedTask;
    }

    private async Task AddProjectReferencesAsync(string solutionPath, SolutionConfiguration config, Dictionary<LayerType, string> projectPaths, Dictionary<LayerType, LayerDefinition> layerDefinitions)
    {
        // Note: projectPaths now contain paths within src/ folder
        
        // Update dependencies dynamically based on SharedKernel selection
        if (config.SelectedLayers.Contains(LayerType.SharedKernel))
        {
            // Domain can depend on SharedKernel
            if (config.SelectedLayers.Contains(LayerType.Domain))
            {
                var domainDef = layerDefinitions[LayerType.Domain];
                if (!domainDef.Dependencies.Contains(LayerType.SharedKernel))
                {
                    domainDef.Dependencies.Add(LayerType.SharedKernel);
                }
            }
            
            // Application can depend on SharedKernel (in addition to Domain)
            if (config.SelectedLayers.Contains(LayerType.Application))
            {
                var appDef = layerDefinitions[LayerType.Application];
                if (!appDef.Dependencies.Contains(LayerType.SharedKernel))
                {
                    appDef.Dependencies.Add(LayerType.SharedKernel);
                }
            }
        }
        
        // Validate dependency rules before adding references
        var validation = DependencyValidator.ValidateDependencyRules(layerDefinitions, config);
        if (!validation.IsValid)
        {
            throw new InvalidOperationException($"Dependency rule violation detected:\n{validation.ErrorMessage}");
        }

        foreach (var layer in config.SelectedLayers)
        {
            var definition = layerDefinitions[layer];
            var projectPath = projectPaths[layer];
            var projectName = $"{config.BaseNamespace}.{layer}";
            var projectFile = Path.Combine(projectPath, $"{projectName}.csproj");

            // Explicit check: SharedKernel must never have dependencies
            if (layer == LayerType.SharedKernel && definition.Dependencies.Any())
            {
                throw new InvalidOperationException("❌ CRITICAL: SharedKernel layer must not have any dependencies. This violates DDD principles.");
            }
            
            // Domain can only depend on SharedKernel (if SharedKernel is selected)
            if (layer == LayerType.Domain && definition.Dependencies.Any())
            {
                var invalidDeps = definition.Dependencies.Where(d => d != LayerType.SharedKernel).ToList();
                if (invalidDeps.Any())
                {
                    throw new InvalidOperationException($"❌ CRITICAL: Domain layer can only depend on SharedKernel, but found: {string.Join(", ", invalidDeps)}");
                }
            }

            foreach (var dependency in definition.Dependencies)
            {
                // Validate that this dependency is allowed
                if (!DependencyValidator.IsValidDependency(layer, dependency))
                {
                    throw new InvalidOperationException($"❌ CRITICAL: Invalid dependency detected: {layer} → {dependency}. This violates Clean Architecture principles.");
                }

                if (config.SelectedLayers.Contains(dependency))
                {
                    var dependencyProjectPath = projectPaths[dependency];
                    var dependencyProjectName = $"{config.BaseNamespace}.{dependency}";
                    var dependencyProjectFile = Path.Combine(dependencyProjectPath, $"{dependencyProjectName}.csproj");

                    var relativePath = Path.GetRelativePath(projectPath, dependencyProjectFile);
                    var relativePathNormalized = relativePath.Replace('\\', '/');

                    // Read current project file
                    var projectContent = await File.ReadAllTextAsync(projectFile);

                    // Add project reference if not already present
                    if (!projectContent.Contains($"<ProjectReference Include=\"{relativePathNormalized}\" />"))
                    {
                        var referenceTag = $"  <ItemGroup>\r\n    <ProjectReference Include=\"{relativePathNormalized}\" />\r\n  </ItemGroup>\r\n";
                        projectContent = projectContent.Replace("</Project>", $"{referenceTag}</Project>");
                        await File.WriteAllTextAsync(projectFile, projectContent);
                        Console.WriteLine($"  ✓ Added reference: {layer} → {dependency}");
                    }
                }
            }
        }

        Console.WriteLine("  ✓ Dependency rules validated and enforced");
    }

    private async Task AddProjectsToSolutionAsync(string solutionFile, Dictionary<LayerType, string> projectPaths, SolutionConfiguration config)
    {
        var solutionContent = await File.ReadAllTextAsync(solutionFile);
        var projectGuids = new Dictionary<string, Guid>();
        var projectEntries = new List<string>();
        var nestedProjects = new List<string>();

        // Create solution folders
        var srcFolderGuid = Guid.NewGuid();
        var testsFolderGuid = Guid.NewGuid();

        // Add solution folder entries
        projectEntries.Add($"Project(\"{{2150E333-8FDC-42A3-9474-1A3956D46DE8}}\") = \"src\", \"src\", \"{{{srcFolderGuid}}}\"");
        projectEntries.Add("EndProject");
        
        if (config.IncludeTests)
        {
            projectEntries.Add($"Project(\"{{2150E333-8FDC-42A3-9474-1A3956D46DE8}}\") = \"tests\", \"tests\", \"{{{testsFolderGuid}}}\"");
            projectEntries.Add("EndProject");
        }

        // Generate GUIDs for each project and check for duplicates
        // Order projects: SharedKernel first, then Domain, then others
        var orderedProjects = projectPaths.OrderBy(p => 
            p.Key == LayerType.SharedKernel ? 0 :
            p.Key == LayerType.Domain ? 1 :
            p.Key == LayerType.Application ? 2 :
            p.Key == LayerType.Infrastructure ? 3 : 4)
            .ToList();

        foreach (var (layer, projectPath) in orderedProjects)
        {
            var projectName = Path.GetFileName(projectPath);
            var projectFile = Path.Combine(projectPath, $"{projectName}.csproj");
            var relativePath = Path.GetRelativePath(Path.GetDirectoryName(solutionFile)!, projectFile);
            var relativePathNormalized = relativePath.Replace('\\', '/');
            
            // Check if project already exists in solution
            if (solutionContent.Contains($"= \"{projectName}\"") || 
                solutionContent.Contains($"\"{relativePathNormalized}\""))
            {
                Console.WriteLine($"  ⚠️  Project {projectName} already exists in solution, skipping...");
                continue;
            }

            projectGuids[projectName] = Guid.NewGuid();
            
            var guid = projectGuids[projectName];
            projectEntries.Add($"Project(\"{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}\") = \"{projectName}\", \"{relativePathNormalized}\", \"{{{guid}}}\"");
            projectEntries.Add("EndProject");
            
            // Add to nested projects (place in src solution folder)
            nestedProjects.Add($"		{{{guid}}} = {{{srcFolderGuid}}}");
        }

        if (projectEntries.Count == 0)
        {
            Console.WriteLine("  ⚠️  No new projects to add to solution");
            return;
        }

        var projectsSection = string.Join("\r\n", projectEntries);
        
        // Find the position right before "Global" keyword
        var globalIndex = solutionContent.IndexOf("Global", StringComparison.Ordinal);
        if (globalIndex == -1)
        {
            throw new InvalidOperationException("Solution file is missing 'Global' section");
        }
        
        solutionContent = solutionContent.Insert(globalIndex, $"{projectsSection}\r\n");

        // Add project configuration platforms
        var configEntries = new List<string>();
        foreach (var (projectName, guid) in projectGuids)
        {
            configEntries.Add($"		{{{guid}}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU");
            configEntries.Add($"		{{{guid}}}.Debug|Any CPU.Build.0 = Debug|Any CPU");
            configEntries.Add($"		{{{guid}}}.Release|Any CPU.ActiveCfg = Release|Any CPU");
            configEntries.Add($"		{{{guid}}}.Release|Any CPU.Build.0 = Release|Any CPU");
        }

        var configSection = string.Join("\r\n", configEntries);
        solutionContent = solutionContent.Replace("	EndGlobalSection", $"	{configSection}\r\n	EndGlobalSection");

        // Add NestedProjects section for solution folders
        if (nestedProjects.Any())
        {
            var nestedProjectsSection = string.Join("\r\n", nestedProjects);
            var nestedProjectsGuid = Guid.NewGuid();
            var nestedProjectsEntry = $"\r\n	GlobalSection(NestedProjects) = preSolution\r\n{nestedProjectsSection}\r\n	EndGlobalSection";
            solutionContent = solutionContent.Replace("	EndGlobalSection", $"{nestedProjectsEntry}\r\n	EndGlobalSection");
        }

        await File.WriteAllTextAsync(solutionFile, solutionContent);
        Console.WriteLine($"✓ Added {projectPaths.Count} project(s) to solution in 'src' folder");
    }

    private async Task AddTestProjectToSolutionAsync(string solutionFile, string testProjectName, string testProjectFile, SolutionConfiguration config)
    {
        var solutionContent = await File.ReadAllTextAsync(solutionFile);
        
        // Check if project already exists in solution by checking for the project name
        // Check both the project name and the project file path
        var projectFileName = Path.GetFileName(testProjectFile);
        if (solutionContent.Contains($"= \"{testProjectName}\"") || 
            solutionContent.Contains($"\"{projectFileName}\""))
        {
            Console.WriteLine($"  ⚠️  Project {testProjectName} already exists in solution, skipping...");
            return;
        }

        var testProjectGuid = Guid.NewGuid();
        var testRelativePath = Path.GetRelativePath(Path.GetDirectoryName(solutionFile)!, testProjectFile);
        var testRelativePathNormalized = testRelativePath.Replace('\\', '/');
        
        // Find the tests solution folder GUID
        var testsFolderGuidMatch = System.Text.RegularExpressions.Regex.Match(solutionContent, @"Project\(""\{2150E333-8FDC-42A3-9474-1A3956D46DE8\}\""\) = ""tests"".*""\{([^}]+)\}""");
        Guid testsFolderGuid;
        if (testsFolderGuidMatch.Success)
        {
            testsFolderGuid = Guid.Parse(testsFolderGuidMatch.Groups[1].Value);
        }
        else
        {
            // Create tests folder if it doesn't exist
            testsFolderGuid = Guid.NewGuid();
            var globalIdx = solutionContent.IndexOf("Global", StringComparison.Ordinal);
            var testsFolderEntry = $"Project(\"{{2150E333-8FDC-42A3-9474-1A3956D46DE8}}\") = \"tests\", \"tests\", \"{{{testsFolderGuid}}}\"\r\nEndProject\r\n";
            solutionContent = solutionContent.Insert(globalIdx, testsFolderEntry);
        }
        
        // Find the position right before "Global" keyword (after all existing projects)
        var globalIndex = solutionContent.IndexOf("Global", StringComparison.Ordinal);
        if (globalIndex == -1)
        {
            throw new InvalidOperationException("Solution file is missing 'Global' section");
        }

        // Insert the project entry right before "Global"
        var testProjectEntry = $"Project(\"{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}\") = \"{testProjectName}\", \"{testRelativePathNormalized}\", \"{{{testProjectGuid}}}\"\r\nEndProject\r\n";
        solutionContent = solutionContent.Insert(globalIndex, testProjectEntry);
        
        // Add test project configuration - find EndGlobalSection and insert before it
        var endGlobalSectionIndex = solutionContent.LastIndexOf("	EndGlobalSection", StringComparison.Ordinal);
        if (endGlobalSectionIndex == -1)
        {
            throw new InvalidOperationException("Solution file is missing 'EndGlobalSection'");
        }

        var testConfigEntries = $"		{{{testProjectGuid}}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU\r\n		{{{testProjectGuid}}}.Debug|Any CPU.Build.0 = Debug|Any CPU\r\n		{{{testProjectGuid}}}.Release|Any CPU.ActiveCfg = Release|Any CPU\r\n		{{{testProjectGuid}}}.Release|Any CPU.Build.0 = Release|Any CPU\r\n";
        solutionContent = solutionContent.Insert(endGlobalSectionIndex, testConfigEntries);
        
        // Add to NestedProjects section (place in tests solution folder)
        var nestedProjectsIndex = solutionContent.IndexOf("GlobalSection(NestedProjects)", StringComparison.Ordinal);
        if (nestedProjectsIndex != -1)
        {
            // NestedProjects section exists, add to it
            var nestedProjectsEndIndex = solutionContent.IndexOf("	EndGlobalSection", nestedProjectsIndex, StringComparison.Ordinal);
            if (nestedProjectsEndIndex != -1)
            {
                var nestedEntry = $"		{{{testProjectGuid}}} = {{{testsFolderGuid}}}\r\n";
                solutionContent = solutionContent.Insert(nestedProjectsEndIndex, nestedEntry);
            }
        }
        else
        {
            // Create NestedProjects section
            var nestedProjectsEntry = $"\r\n	GlobalSection(NestedProjects) = preSolution\r\n		{{{testProjectGuid}}} = {{{testsFolderGuid}}}\r\n	EndGlobalSection";
            solutionContent = solutionContent.Replace("	EndGlobalSection", $"{nestedProjectsEntry}\r\n	EndGlobalSection");
        }
        
        await File.WriteAllTextAsync(solutionFile, solutionContent);
    }

    private async Task CreateCQRSStructureAsync(Dictionary<LayerType, string> projectPaths, SolutionConfiguration config)
    {
        if (projectPaths.ContainsKey(LayerType.Application))
        {
            var appPath = projectPaths[LayerType.Application];
            var commandsPath = Path.Combine(appPath, "UseCases", "Commands");
            var queriesPath = Path.Combine(appPath, "UseCases", "Queries");

            Directory.CreateDirectory(commandsPath);
            Directory.CreateDirectory(queriesPath);

            await File.WriteAllTextAsync(Path.Combine(commandsPath, ".gitkeep"), string.Empty);
            await File.WriteAllTextAsync(Path.Combine(queriesPath, ".gitkeep"), string.Empty);

            Console.WriteLine($"✓ Created CQRS structure (Commands/Queries)");
        }
    }

    private async Task CreateEFCorePlaceholdersAsync(string infrastructurePath, SolutionConfiguration config)
    {
        // Add EF Core NuGet package to Infrastructure project
        var projectName = $"{config.BaseNamespace}.Infrastructure";
        var projectFile = Path.Combine(infrastructurePath, $"{projectName}.csproj");
        var projectContent = await File.ReadAllTextAsync(projectFile);

        // Add EF Core package reference if not already present
        if (!projectContent.Contains("Microsoft.EntityFrameworkCore"))
        {
            var packageReference = "    <PackageReference Include=\"Microsoft.EntityFrameworkCore\" Version=\"9.0.0\" />\r\n    <PackageReference Include=\"Microsoft.EntityFrameworkCore.SqlServer\" Version=\"9.0.0\" />\r\n";
            projectContent = projectContent.Replace("  </PropertyGroup>", $"  </PropertyGroup>\r\n\r\n  <ItemGroup>\r\n{packageReference}  </ItemGroup>");
            await File.WriteAllTextAsync(projectFile, projectContent);
            Console.WriteLine($"  ✓ Added EF Core NuGet packages");
        }

        // Create placeholder DbContext file
        var dbContextPath = Path.Combine(infrastructurePath, "Persistence", "DbContext");
        var dbContextFile = Path.Combine(dbContextPath, "ApplicationDbContext.cs");

        var dbContextContent = $@"using Microsoft.EntityFrameworkCore;

namespace {config.BaseNamespace}.Infrastructure.Persistence.DbContext;

public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
{{
    public ApplicationDbContext(Microsoft.EntityFrameworkCore.DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {{
    }}

    // Add DbSet properties here
    // Example: public DbSet<Entity> Entities {{ get; set; }} = null!;

    protected override void OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
    {{
        base.OnModelCreating(modelBuilder);
        
        // Apply entity configurations here
        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }}
}}
";

        await File.WriteAllTextAsync(dbContextFile, dbContextContent);
        Console.WriteLine($"✓ Created EF Core DbContext placeholder");
    }

    private async Task CreateTestProjectsAsync(string testsPath, string solutionFile, SolutionConfiguration config, Dictionary<LayerType, string> projectPaths)
    {
        // IMPORTANT: All test projects and test files are created in tests/ folder
        // This ensures clear separation: src/ = source code, tests/ = all test-related code
        var testProjects = new[] { "Unit", "Integration" };

        foreach (var testType in testProjects)
        {
            var testProjectName = $"{config.BaseNamespace}.{testType}Tests";
            // Ensure test project is created in tests folder, not src folder
            var testProjectPath = Path.Combine(testsPath, testProjectName);
            Directory.CreateDirectory(testProjectPath);

            var testProjectFile = Path.Combine(testProjectPath, $"{testProjectName}.csproj");

            var testProjectContent = $@"<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>{config.TargetFramework}</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
    <RootNamespace>{config.BaseNamespace}.{testType}Tests</RootNamespace>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""Microsoft.NET.Test.Sdk"" Version=""17.11.1"" />
    <PackageReference Include=""xunit"" Version=""2.9.2"" />
    <PackageReference Include=""xunit.runner.visualstudio"" Version=""2.8.2"" />
  </ItemGroup>

</Project>
";

            await File.WriteAllTextAsync(testProjectFile, testProjectContent);

            // Add references to main projects (in src folder)
            var testProjectContentWithRefs = testProjectContent;
            var projectReferences = new List<string>();
            foreach (var (layer, projectPath) in projectPaths)
            {
                var projectName = $"{config.BaseNamespace}.{layer}";
                var projectFile = Path.Combine(projectPath, $"{projectName}.csproj");
                var relativePath = Path.GetRelativePath(testProjectPath, projectFile);
                var relativePathNormalized = relativePath.Replace('\\', '/');
                projectReferences.Add($"    <ProjectReference Include=\"{relativePathNormalized}\" />");
            }
            
            if (projectReferences.Any())
            {
                var referencesSection = string.Join("\r\n", projectReferences);
                testProjectContentWithRefs = testProjectContentWithRefs.Replace("  </ItemGroup>", $"{referencesSection}\r\n  </ItemGroup>");
            }
            await File.WriteAllTextAsync(testProjectFile, testProjectContentWithRefs);

            // Create sample test file
            var testClassPath = Path.Combine(testProjectPath, $"{testType}Test.cs");
            var testClassContent = $@"using Xunit;

namespace {config.BaseNamespace}.{testType}Tests;

public class {testType}Test
{{
    [Fact]
    public void SampleTest()
    {{
        // Arrange
        
        // Act
        
        // Assert
        Assert.True(true);
    }}
}}
";
            await File.WriteAllTextAsync(testClassPath, testClassContent);

            // Add test project to solution
            await AddTestProjectToSolutionAsync(solutionFile, testProjectName, testProjectFile, config);
            Console.WriteLine($"✓ Created {testType} test project");
        }
    }

    private async Task GenerateConfigurationArtifactsAsync(string solutionPath, SolutionConfiguration config)
    {
        // .editorconfig
        var editorConfigContent = @"root = true

[*]
charset = utf-8
insert_final_newline = true
trim_trailing_whitespace = true

[*.cs]
indent_style = space
indent_size = 4
end_of_line = lf

[*.{csproj,vbproj,vcxproj,vcxproj.filters,proj,projitems,shproj}]
indent_style = space
indent_size = 2

[*.{json,js,ts}]
indent_style = space
indent_size = 2
";
        await File.WriteAllTextAsync(Path.Combine(solutionPath, ".editorconfig"), editorConfigContent);
        Console.WriteLine($"✓ Generated .editorconfig");

        // Directory.Build.props
        var buildPropsContent = $@"<Project>
  <PropertyGroup>
    <TargetFramework>{config.TargetFramework}</TargetFramework>
    <LangVersion>latest</LangVersion>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <TreatWarningsAsErrors>false</TreatWarningsAsErrors>
    <WarningsAsErrors />
    <WarningsNotAsErrors />
  </PropertyGroup>
</Project>
";
        await File.WriteAllTextAsync(Path.Combine(solutionPath, "Directory.Build.props"), buildPropsContent);
        Console.WriteLine($"✓ Generated Directory.Build.props");

        // .gitignore
        var gitignoreContent = @"## Ignore Visual Studio temporary files, build results, and
## files generated by popular Visual Studio add-ons.

# User-specific files
*.rsuser
*.suo
*.user
*.userosscache
*.sln.docstates

# Build results
[Dd]ebug/
[Dd]ebugPublic/
[Rr]elease/
[Rr]eleases/
x64/
x86/
[Ww][Ii][Nn]32/
[Aa][Rr][Mm]/
[Aa][Rr][Mm]64/
bld/
[Bb]in/
[Oo]bj/
[Ll]og/
[Ll]ogs/

# Visual Studio cache/options directory
.vs/

# .NET Core
project.lock.json
project.fragment.lock.json
artifacts/

# NuGet Packages
*.nupkg
*.snupkg
**/packages/*
!**/packages/build/
*.nuget.props
*.nuget.targets

# Rider
.idea/
*.sln.iml

# User-specific files
*.rsuser
*.suo
*.user
*.userosscache
*.sln.docstates
";
        await File.WriteAllTextAsync(Path.Combine(solutionPath, ".gitignore"), gitignoreContent);
        Console.WriteLine($"✓ Generated .gitignore");

        // README.md - Always generate comprehensive README
        var readmeContent = $@"# {config.SolutionName}

## Overview

This solution follows **Clean Architecture** principles with **Domain-Driven Design (DDD)** alignment. It provides a structured, maintainable foundation for building scalable .NET applications.

## Solution Structure

```
{config.SolutionName}/
├── src/                          # Source code projects
{string.Join("\n", config.SelectedLayers.Select(l => $"│   ├── {config.BaseNamespace}.{l}/"))}
{(config.IncludeTests ? "├── tests/                      # Test projects\n│   ├── " + config.BaseNamespace + ".UnitTests/\n│   └── " + config.BaseNamespace + ".IntegrationTests/" : "")}
├── {config.SolutionName}.sln
├── Directory.Build.props
├── .editorconfig
├── .gitignore
└── README.md
```

## Projects

### Source Projects (src/)

{string.Join("\n\n", config.SelectedLayers.Select(l => $"#### {config.BaseNamespace}.{l}\n\n{GetLayerDescription(l)}\n\n**Location**: `src/{config.BaseNamespace}.{l}/`"))}

{(config.SelectedLayers.Contains(LayerType.API) ? $"\n#### API Type\n\nThis solution uses **{config.SelectedApiType}** for the API layer.\n" : "")}

{(config.IncludeCQRS ? "\n#### CQRS Structure\n\nThis solution includes CQRS (Command Query Responsibility Segregation) structure:\n- **Commands**: Located in `Application/UseCases/Commands/`\n- **Queries**: Located in `Application/UseCases/Queries/`\n" : "")}

{(config.IncludeEFCore ? "\n#### Entity Framework Core\n\nEF Core infrastructure is configured in the Infrastructure layer:\n- **DbContext**: `Infrastructure/Persistence/DbContext/ApplicationDbContext.cs`\n- **Configurations**: `Infrastructure/Persistence/Configurations/`\n- **Migrations**: `Infrastructure/Persistence/Migrations/`\n" : "")}

{(config.IncludeTests ? $"\n### Test Projects (tests/)\n\n#### {config.BaseNamespace}.UnitTests\n\nUnit tests for all layers. Tests individual components in isolation.\n\n**Location**: `tests/{config.BaseNamespace}.UnitTests/`\n\n#### {config.BaseNamespace}.IntegrationTests\n\nIntegration tests that verify the interaction between layers and external dependencies.\n\n**Location**: `tests/{config.BaseNamespace}.IntegrationTests/`\n" : "")}

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
│SharedKernel │ ← Innermost layer (no dependencies){(config.SelectedLayers.Contains(LayerType.SharedKernel) ? " ✓" : "")}
└─────────────┘
       ↑
       │
┌─────────────┐
│Infrastructure│
└──────────────┘
```

### Dependency Rules

- ✅ **Application → Domain**: Application layer depends on Domain
{(config.SelectedLayers.Contains(LayerType.SharedKernel) ? "- ✅ **Application → SharedKernel**: Application can also depend on SharedKernel\n- ✅ **Domain → SharedKernel**: Domain can depend on SharedKernel\n" : "")}
- ✅ **Infrastructure → Application**: Infrastructure implements Application interfaces
- ✅ **API → Application**: API layer depends on Application
- ✅ **Domain → Nothing/SharedKernel**: Domain has no external dependencies (except SharedKernel if selected)
{(config.SelectedLayers.Contains(LayerType.SharedKernel) ? "- ✅ **SharedKernel → Nothing**: SharedKernel has no dependencies\n" : "")}

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

- .NET SDK {config.TargetFramework.Replace("net", "")} or later
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
   cd src/{config.BaseNamespace}.API
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

{(config.IncludeTests ? @"- **Unit Tests**: Test individual components in isolation
- **Integration Tests**: Test layer interactions and external dependencies
- Run tests frequently: `dotnet test`
" : "Add test projects to enable comprehensive testing.")}

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
**Generated on**: {DateTime.Now:yyyy-MM-dd}
";
            await File.WriteAllTextAsync(Path.Combine(solutionPath, "README.md"), readmeContent);
            Console.WriteLine($"✓ Generated README.md");
    }

    private string GetLayerDescription(LayerType layer)
    {
        return layer switch
        {
            LayerType.SharedKernel => "Shared domain concepts, entities, value objects, enums, constants, and common abstractions (DDD Shared Kernel pattern). Contains reusable domain elements shared across bounded contexts.",
            LayerType.Domain => "Core business logic, entities, and domain rules",
            LayerType.Application => "Use cases, interfaces, and application services",
            LayerType.Infrastructure => "Data access, external services, and infrastructure implementations",
            LayerType.API => "Web API controllers, middleware, and HTTP concerns",
            _ => string.Empty
        };
    }
}

