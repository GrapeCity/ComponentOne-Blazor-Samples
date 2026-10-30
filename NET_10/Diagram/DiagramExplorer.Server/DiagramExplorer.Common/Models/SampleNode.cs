using System.Collections.Generic;
namespace DiagramExplorer.Models;

public class SampleNode
{
    public string Id { get; set; } = "";
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public List<SampleNode> Children { get; set; } = [];
    public bool IsLeaf => Children.Count == 0;
}
