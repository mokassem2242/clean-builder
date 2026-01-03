using CleanArchitectureGenerator.Models;

namespace CleanArchitectureGenerator.Services;

public class LayerConfigurationService
{
    public static Dictionary<LayerType, LayerDefinition> GetLayerDefinitions()
    {
        return new Dictionary<LayerType, LayerDefinition>
        {
            {
                LayerType.Domain,
                new LayerDefinition
                {
                    Type = LayerType.Domain,
                    ProjectType = "classlib",
                    IsMandatory = true,
                    Dependencies = new List<LayerType>(),
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
                    Dependencies = new List<LayerType> { LayerType.Domain },
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

