using CleanArchitectureGenerator.Models;

namespace CleanArchitectureGenerator.Services;

/// <summary>
/// Validates that dependency rules follow Clean Architecture principles.
/// Ensures dependencies point inward (toward Domain) and Domain has no dependencies.
/// </summary>
public static class DependencyValidator
{
    /// <summary>
    /// Validates that the dependency configuration follows Clean Architecture rules.
    /// </summary>
    /// <param name="layerDefinitions">The layer definitions to validate</param>
    /// <param name="config">Solution configuration to check for SharedKernel</param>
    /// <returns>Validation result with any violations</returns>
    public static ValidationResult ValidateDependencyRules(Dictionary<LayerType, LayerDefinition> layerDefinitions, SolutionConfiguration? config = null)
    {
        var violations = new List<string>();
        var hasSharedKernel = config?.SelectedLayers.Contains(LayerType.SharedKernel) ?? 
                             layerDefinitions.ContainsKey(LayerType.SharedKernel) && 
                             layerDefinitions[LayerType.SharedKernel].Dependencies.Any() == false;

        // Rule 1: SharedKernel MUST NOT have any dependencies
        if (layerDefinitions.ContainsKey(LayerType.SharedKernel))
        {
            var sharedKernelDefinition = layerDefinitions[LayerType.SharedKernel];
            if (sharedKernelDefinition.Dependencies.Any())
            {
                violations.Add($"❌ VIOLATION: SharedKernel layer must not have dependencies, but found: {string.Join(", ", sharedKernelDefinition.Dependencies)}");
            }
        }

        // Rule 2: Domain can only depend on SharedKernel (if SharedKernel exists)
        var domainDefinition = layerDefinitions[LayerType.Domain];
        var invalidDomainDeps = domainDefinition.Dependencies.Where(d => d != LayerType.SharedKernel).ToList();
        if (invalidDomainDeps.Any())
        {
            violations.Add($"❌ VIOLATION: Domain layer can only depend on SharedKernel, but found: {string.Join(", ", invalidDomainDeps)}");
        }

        // Rule 3: Application can depend on Domain and SharedKernel
        var applicationDefinition = layerDefinitions[LayerType.Application];
        var invalidAppDeps = applicationDefinition.Dependencies.Where(d => d != LayerType.Domain && d != LayerType.SharedKernel).ToList();
        if (invalidAppDeps.Any())
        {
            violations.Add($"❌ VIOLATION: Application layer can only depend on Domain and SharedKernel, but found: {string.Join(", ", invalidAppDeps)}");
        }

        // Rule 4: Infrastructure can only depend on Application (and transitively Domain/SharedKernel)
        var infrastructureDefinition = layerDefinitions[LayerType.Infrastructure];
        var invalidInfraDeps = infrastructureDefinition.Dependencies.Where(d => d != LayerType.Application).ToList();
        if (invalidInfraDeps.Any())
        {
            violations.Add($"❌ VIOLATION: Infrastructure layer can only depend on Application, but found: {string.Join(", ", invalidInfraDeps)}");
        }

        // Rule 5: API can only depend on Application (and transitively Domain/SharedKernel)
        var apiDefinition = layerDefinitions[LayerType.API];
        var invalidApiDeps = apiDefinition.Dependencies.Where(d => d != LayerType.Application).ToList();
        if (invalidApiDeps.Any())
        {
            violations.Add($"❌ VIOLATION: API layer can only depend on Application, but found: {string.Join(", ", invalidApiDeps)}");
        }

        // Rule 6: No circular dependencies (should not happen with current structure, but check anyway)
        if (HasCircularDependencies(layerDefinitions))
        {
            violations.Add("❌ VIOLATION: Circular dependencies detected!");
        }

        if (violations.Any())
        {
            return ValidationResult.Failure(string.Join("\n", violations));
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Validates that a specific dependency is allowed according to Clean Architecture rules.
    /// </summary>
    public static bool IsValidDependency(LayerType fromLayer, LayerType toLayer)
    {
        // SharedKernel cannot depend on anything
        if (fromLayer == LayerType.SharedKernel)
        {
            return false;
        }

        // Domain can only depend on SharedKernel
        if (fromLayer == LayerType.Domain)
        {
            return toLayer == LayerType.SharedKernel;
        }

        // Application can depend on Domain and SharedKernel
        if (fromLayer == LayerType.Application)
        {
            return toLayer == LayerType.Domain || toLayer == LayerType.SharedKernel;
        }

        // Infrastructure can only depend on Application
        if (fromLayer == LayerType.Infrastructure)
        {
            return toLayer == LayerType.Application;
        }

        // API can only depend on Application
        if (fromLayer == LayerType.API)
        {
            return toLayer == LayerType.Application;
        }

        return false;
    }

    /// <summary>
    /// Gets the expected dependencies for a layer according to Clean Architecture rules.
    /// </summary>
    public static List<LayerType> GetExpectedDependencies(LayerType layer)
    {
        return layer switch
        {
            LayerType.SharedKernel => new List<LayerType>(), // SharedKernel has no dependencies
            LayerType.Domain => new List<LayerType>(), // Domain has no dependencies by default (can depend on SharedKernel if selected)
            LayerType.Application => new List<LayerType> { LayerType.Domain }, // Can also depend on SharedKernel if selected
            LayerType.Infrastructure => new List<LayerType> { LayerType.Application },
            LayerType.API => new List<LayerType> { LayerType.Application },
            _ => new List<LayerType>()
        };
    }

    private static bool HasCircularDependencies(Dictionary<LayerType, LayerDefinition> layerDefinitions)
    {
        // With our current structure, circular dependencies should not be possible,
        // but we check anyway for safety
        var visited = new HashSet<LayerType>();
        var recursionStack = new HashSet<LayerType>();

        foreach (var layer in layerDefinitions.Keys)
        {
            if (HasCycle(layer, layerDefinitions, visited, recursionStack))
            {
                return true;
            }
        }

        return false;
    }

    private static bool HasCycle(LayerType layer, Dictionary<LayerType, LayerDefinition> layerDefinitions, 
        HashSet<LayerType> visited, HashSet<LayerType> recursionStack)
    {
        if (recursionStack.Contains(layer))
        {
            return true; // Cycle detected
        }

        if (visited.Contains(layer))
        {
            return false; // Already processed
        }

        visited.Add(layer);
        recursionStack.Add(layer);

        var definition = layerDefinitions[layer];
        foreach (var dependency in definition.Dependencies)
        {
            if (HasCycle(dependency, layerDefinitions, visited, recursionStack))
            {
                return true;
            }
        }

        recursionStack.Remove(layer);
        return false;
    }
}

