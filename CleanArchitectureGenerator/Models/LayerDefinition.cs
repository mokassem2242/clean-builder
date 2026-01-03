namespace CleanArchitectureGenerator.Models;

public class LayerDefinition
{
    public LayerType Type { get; set; }
    public string ProjectType { get; set; } = string.Empty;
    public List<string> Folders { get; set; } = new();
    public bool IsMandatory { get; set; }
    public List<LayerType> Dependencies { get; set; } = new();
}

