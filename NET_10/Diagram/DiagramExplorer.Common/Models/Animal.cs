using DiagramExplorer.Services;
using System.Drawing;

namespace DiagramExplorer.Models
{
    public class Animal
    {
        Bitmap? image = null;
        bool imageLoaded = false;

        public string Name { get; set; } = "";
        public string NameScientific { get; set; } = "";

        public string Phylum { get; set; } = "";

        public string Class { get; set; } = "";

        public string Order { get; set; } = "";

        public string? ImagePath => Class != "Class" ? $"images/Animals/{Name.Replace(' ', '_')}.png" : null;

        public override string ToString() => Name;
    }
}
