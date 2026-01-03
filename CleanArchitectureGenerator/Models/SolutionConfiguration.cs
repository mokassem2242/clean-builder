namespace CleanArchitectureGenerator.Models;

public class SolutionConfiguration
{
    public string SolutionName { get; set; } = string.Empty;
    public string BaseNamespace { get; set; } = string.Empty;
    public string TargetFramework { get; set; } = "net9.0";
    public List<LayerType> SelectedLayers { get; set; } = new();
    public ApiType SelectedApiType { get; set; } = ApiType.WebAPI;
    public bool IncludeCQRS { get; set; }
    public bool IncludeEFCore { get; set; }
    public bool IncludeTests { get; set; }
    public bool IncludeReadme { get; set; }
}

public enum LayerType
{
    SharedKernel,
    Domain,
    Application,
    Infrastructure,
    API
}

public enum ApiType
{
    WebAPI,      // Traditional ASP.NET Core Web API with Controllers
    MinimalAPI,  // ASP.NET Core Minimal API
    gRPC         // gRPC service
}

