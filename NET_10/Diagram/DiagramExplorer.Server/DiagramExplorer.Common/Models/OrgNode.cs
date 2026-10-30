using DiagramExplorer.Services;
using System.Drawing;

namespace DiagramExplorer.Models;

// Simple hierarchical organization node
public class OrgNode
{
    Bitmap? image = null;
    bool imageLoaded = false;

    public string Name { get; set; } = "";

    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";

    public string Department { get; set; } = "";

    public string JobTitle { get; set; } = "";
    public List<OrgNode> Childs { get; set; } = new List<OrgNode>();

    public string ImagePath => $"images/Humans/{Name.Replace(' ', '_')}.png";
}
