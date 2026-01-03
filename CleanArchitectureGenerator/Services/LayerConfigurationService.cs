using CleanArchitectureGenerator.Models;

namespace CleanArchitectureGenerator.Services;

public class LayerConfigurationService
{
    public static Dictionary<LayerType, LayerDefinition> GetLayerDefinitions()
    {
        return new Dictionary<LayerType, LayerDefinition>
        {
            {
                LayerType.SharedKernel,
                new LayerDefinition
                {
                    Type = LayerType.SharedKernel,
                    ProjectType = "classlib",
                    IsMandatory = false,
                    Dependencies = new List<LayerType>(), // SharedKernel has no dependencies
                    Folders = new List<string>
                    {
                        "Entities",
                        "ValueObjects",
                        "Enums",
                        "Constants",
                        "Exceptions",
                        "Interfaces",
                        "Common"
                    }
                }
            },
            {
                LayerType.Domain,
                new LayerDefinition
                {
                    Type = LayerType.Domain,
                    ProjectType = "classlib",
                    IsMandatory = true,
                    Dependencies = new List<LayerType>(), // Will be updated based on SharedKernel selection
                    Folders = new List<string>
                    {
                        "Entities",
                        "ValueObjects",
                        "Aggregates",
                        "Specifications",
                        "DomainServices",
                        "DomainEvents",
                        "Exceptions",
                        "Common"
                    }
                }
            },
            {
                LayerType.Application,
                new LayerDefinition
                {
                    Type = LayerType.Application,
                    ProjectType = "classlib",
                    IsMandatory = false,
                    Dependencies = new List<LayerType> { LayerType.Domain }, // Will be updated based on SharedKernel selection
                    Folders = new List<string>
                    {
                        "UseCases",
                        "Interfaces",
                        "DTOs",
                        "Validators",
                        "Mappings",
                        "Common"
                    }
                }
            },
            {
                LayerType.Infrastructure,
                new LayerDefinition
                {
                    Type = LayerType.Infrastructure,
                    ProjectType = "classlib",
                    IsMandatory = false,
                    Dependencies = new List<LayerType> { LayerType.Application },
                    Folders = new List<string>
                    {
                        "Persistence/DbContext",
                        "Persistence/Configurations",
                        "Persistence/Migrations",
                        "Repositories",
                        "Services",
                        "Messaging"
                    }
                }
            },
            {
                LayerType.API,
                new LayerDefinition
                {
                    Type = LayerType.API,
                    ProjectType = "webapi",
                    IsMandatory = false,
                    Dependencies = new List<LayerType> { LayerType.Application },
                    Folders = new List<string>
                    {
                        "Controllers",
                        "Filters",
                        "Middleware",
                        "Contracts",
                        "Extensions"
                    }
                }
            }
        };
    }
}

