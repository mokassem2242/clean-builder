using CleanArchitectureGenerator.Models;
using CleanArchitectureGenerator.Services;

namespace CleanArchitectureGenerator.Services;

public class CLIInterface
{
    public SolutionConfiguration PromptForConfiguration()
    {
        var config = new SolutionConfiguration();

        Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   Clean Architecture Solution Generator                    ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
        Console.WriteLine();

        // Solution Name
        while (true)
        {
            Console.Write("Solution Name (e.g., Company.Product): ");
            var solutionName = Console.ReadLine();
            var validation = InputValidator.ValidateSolutionName(solutionName);
            if (validation.IsValid)
            {
                config.SolutionName = solutionName!;
                break;
            }
            Console.WriteLine($"❌ {validation.ErrorMessage}");
        }

        // Base Namespace
        while (true)
        {
            Console.Write($"Base Namespace (default: {config.SolutionName}): ");
            var namespaceInput = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(namespaceInput))
            {
                config.BaseNamespace = config.SolutionName;
                break;
            }
            var validation = InputValidator.ValidateNamespace(namespaceInput);
            if (validation.IsValid)
            {
                config.BaseNamespace = namespaceInput;
                break;
            }
            Console.WriteLine($"❌ {validation.ErrorMessage}");
        }

        // .NET Version
        while (true)
        {
            Console.Write(".NET Version (default: net9.0): ");
            var versionInput = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(versionInput))
            {
                config.TargetFramework = "net9.0";
                break;
            }
            var validation = InputValidator.ValidateDotNetVersion(versionInput);
            if (validation.IsValid)
            {
                config.TargetFramework = versionInput;
                break;
            }
            Console.WriteLine($"❌ {validation.ErrorMessage}");
        }

        Console.WriteLine();

        // Layer Selection
        Console.WriteLine("Select layers to include (Domain is mandatory):");
        Console.WriteLine("Note: SharedKernel is optional and contains shared domain concepts (DDD pattern)");
        var layerDefinitions = LayerConfigurationService.GetLayerDefinitions();

        // Order layers: SharedKernel first (if selected), then Domain, then others
        var layersInOrder = Enum.GetValues<LayerType>()
            .OrderBy(l => l == LayerType.SharedKernel ? 0 : 
                         l == LayerType.Domain ? 1 : 
                         l == LayerType.Application ? 2 :
                         l == LayerType.Infrastructure ? 3 : 4)
            .ToList();

        foreach (var layer in layersInOrder)
        {
            var definition = layerDefinitions[layer];
            var defaultSelection = definition.IsMandatory ? "Y" : "N";
            var mandatory = definition.IsMandatory ? " (mandatory)" : "";

            while (true)
            {
                Console.Write($"  Include {layer} layer? {mandatory} (Y/n, default: {defaultSelection}): ");
                var input = Console.ReadLine()?.Trim().ToUpperInvariant();
                
                if (string.IsNullOrWhiteSpace(input))
                {
                    input = defaultSelection;
                }

                if (input == "Y" || input == "YES")
                {
                    config.SelectedLayers.Add(layer);
                    break;
                }
                else if (input == "N" || input == "NO")
                {
                    if (definition.IsMandatory)
                    {
                        Console.WriteLine($"  ⚠️  {layer} is mandatory and must be included.");
                        continue;
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("  Please enter Y or N.");
                }
            }
        }

        Console.WriteLine();

        // API Type Selection (if API layer is selected)
        if (config.SelectedLayers.Contains(LayerType.API))
        {
            config.SelectedApiType = PromptApiType();
            Console.WriteLine();
        }

        // Optional Features
        Console.WriteLine("Optional Features:");
        
        config.IncludeCQRS = PromptYesNo("Include CQRS structure (Commands/Queries)?", false);
        config.IncludeEFCore = PromptYesNo("Include EF Core infrastructure setup?", false);
        config.IncludeTests = PromptYesNo("Include test projects (Unit/Integration)?", false);
        // README is always generated - no need to prompt
        config.IncludeReadme = true;

        return config;
    }

    private static bool PromptYesNo(string prompt, bool defaultValue)
    {
        var defaultText = defaultValue ? "Y" : "N";
        while (true)
        {
            Console.Write($"  {prompt} (Y/n, default: {defaultText}): ");
            var input = Console.ReadLine()?.Trim().ToUpperInvariant();
            
            if (string.IsNullOrWhiteSpace(input))
            {
                return defaultValue;
            }

            if (input == "Y" || input == "YES")
            {
                return true;
            }
            else if (input == "N" || input == "NO")
            {
                return false;
            }
            else
            {
                Console.WriteLine("  Please enter Y or N.");
            }
        }
    }

    public void DisplaySummary(SolutionConfiguration config)
    {
        Console.WriteLine();
        Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                    Configuration Summary                     ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
        Console.WriteLine($"Solution Name:     {config.SolutionName}");
        Console.WriteLine($"Base Namespace:    {config.BaseNamespace}");
        Console.WriteLine($".NET Version:      {config.TargetFramework}");
        Console.WriteLine($"Selected Layers:   {string.Join(", ", config.SelectedLayers)}");
        if (config.SelectedLayers.Contains(LayerType.API))
        {
            Console.WriteLine($"API Type:          {config.SelectedApiType}");
        }
        Console.WriteLine($"CQRS:              {(config.IncludeCQRS ? "Yes" : "No")}");
        Console.WriteLine($"EF Core:           {(config.IncludeEFCore ? "Yes" : "No")}");
        Console.WriteLine($"Test Projects:     {(config.IncludeTests ? "Yes" : "No")}");
        Console.WriteLine($"README:            {(config.IncludeReadme ? "Yes" : "No")}");
        Console.WriteLine();
    }

    private static ApiType PromptApiType()
    {
        Console.WriteLine("Select API Type:");
        Console.WriteLine("  1. Web API (Traditional ASP.NET Core with Controllers) - Default");
        Console.WriteLine("  2. Minimal API (ASP.NET Core Minimal API)");
        Console.WriteLine("  3. gRPC (gRPC Service)");
        Console.Write("Enter choice (1-3, default: 1): ");

        while (true)
        {
            var input = Console.ReadLine()?.Trim();
            
            if (string.IsNullOrWhiteSpace(input))
            {
                return ApiType.WebAPI;
            }

            switch (input)
            {
                case "1":
                    return ApiType.WebAPI;
                case "2":
                    return ApiType.MinimalAPI;
                case "3":
                    return ApiType.gRPC;
                default:
                    Console.Write("Invalid choice. Please enter 1, 2, or 3 (default: 1): ");
                    break;
            }
        }
    }

    public bool ConfirmGeneration()
    {
        Console.Write("Proceed with generation? (Y/n, default: Y): ");
        var input = Console.ReadLine()?.Trim().ToUpperInvariant();
        return string.IsNullOrWhiteSpace(input) || input == "Y" || input == "YES";
    }
}

