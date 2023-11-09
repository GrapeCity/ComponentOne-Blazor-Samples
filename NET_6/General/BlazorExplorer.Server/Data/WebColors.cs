using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace BlazorExplorer
{
    public class WebColor
    {
        Color _clr = Color.Empty;
        string _val;

        public string Name { get; set; }
        public string Group { get; set; }
        public string Value
        {
            get
            {
                return _val;
            }
            set
            {
                if (value != _val)
                {
                    _val = value;
                    _clr = Color.FromArgb(255, Color.FromArgb(Convert.ToInt32(_val, 16)));
                }
            }
        }

        public Color Color
        {
            get
            {
                return _clr;
            }
        }

        public byte Red
        {
            get { return _clr.R; }
        }

        public byte Green
        {
            get { return _clr.G; }
        }


        public byte Blue
        {
            get { return _clr.B; }
        }
    }

    public class WebColors
    {
        static WebColor[] _clrs;

        static WebColor[] ReadColors()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BlazorExplorer.Data.WebColors.json"))
            {
                using (var sr = new StreamReader(stream))
                {
                    return System.Text.Json.JsonSerializer.Deserialize<WebColor[]>(sr.ReadToEnd());
                }
            }
        }

        public static WebColor[] GetColors()
        {
            if (_clrs == null)
            {
                _clrs = ReadColors();
            }

            return _clrs;
        }
    }
}
