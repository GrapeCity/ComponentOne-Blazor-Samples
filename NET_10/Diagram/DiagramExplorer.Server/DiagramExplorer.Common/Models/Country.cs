using DiagramExplorer.Services;
using System.Drawing;

namespace DiagramExplorer.Models
{
    public class Country
    {
        public required string World { get; set; }
        public required string Continent { get; set; }
        public required string Region { get; set; }

        public required string Name { get; set; }
        public required string Code { get; set; }

        Bitmap? flag;

        public string FlagImagePath => $"images/Flags/{Code}.png";
    }
}
