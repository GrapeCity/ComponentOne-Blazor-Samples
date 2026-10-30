using C1.Blazor.Diagram;
using C1.Diagram.Parser;
using C1.Chart.Standard;
//using C1.Chart.Drawing;

using System.Drawing;
using System.Reflection;

namespace DiagramExplorer.Tools
{
    public class SampleTools
    {
        static Stream? GetResourceStream(string name)
        {
            var asm = Assembly.GetExecutingAssembly();
            return asm.GetManifestResourceStream($"{asm.GetName().Name}.Resources.{name}");
        }

        public static void LoadDiagramFromResource(FlexDiagram diagram, string name)
        {
            var stream = GetResourceStream(name);

            var text = "";
            if (stream != null)
            {
                using (var sr = new StreamReader(stream))
                    text = sr.ReadToEnd();
            }


            diagram.LoadMermaidGraph(text);
        }

        public static void DrawBlob(C1.Chart.IRenderEngine e, double left, double top, double width, double height, int seed = 1)
        {
            var rnd = new Random(seed);

            int f1 = 3 + rnd.Next(0, 3);
            int f2 = 5 + rnd.Next(0, 3);
            int f3 = 7 + rnd.Next(0, 3);

            double p1 = rnd.NextDouble() * Math.PI * 2;
            double p2 = rnd.NextDouble() * Math.PI * 2;
            double p3 = rnd.NextDouble() * Math.PI * 2;

            const int steps = 120;

            var rx = 0.5 * width;
            var ry = 0.5 * height;

            var cx = left + rx;
            var cy = top + ry;

            var xs = new double[steps];
            var ys = new double[steps];

            for (int i = 0; i < steps; i++)
            {
                double t = i * 2.0 * Math.PI / steps;
                double r = 1.0 + 0.11 * Math.Sin(f1 * t + p1) + 0.06 * Math.Cos(f2 * t + p2) + 0.04 * Math.Sin(f3 * t + p3);
                xs[i] = cx + rx * r * Math.Cos(t);
                ys[i] = cy + ry * r * Math.Sin(t);
            }

            e.DrawPolygon(xs, ys);
        }

        static Random rnd = new Random();

  /*      public static Bitmap CreateFlexChart(int w, int h, Color foreColor, Color backColor)
        {
            var pts = new List<Point>();
            for (var i = 0; i < 6; i++)
                pts.Add(new Point(i, rnd.Next(100)));

            var chart = new FlexChart()
            {
                Binding = "Y",
                BindingX = "X",
                DataSource = pts,
                //ForeColor = foreColor, 
                //BackColor = backColor 
            };
            chart.Series.Add(new Series());
            var ms = new MemoryStream();
            chart.SavePng(ms, w, h);
            return new Bitmap(ms);
        }

        public static Bitmap CreateFlexPie(int w, int h, Color foreColor, Color backColor)
        {
            var pts = new List<Point>();
            for (var i = 0; i < 4; i++)
                pts.Add(new Point(i, rnd.Next(100)));

            var chart = new FlexPie()
            {
                Binding = "Y",
                BindingName = "X",
                DataSource = pts,
                //ForeColor = foreColor,
                //BackColor = backColor
            };
            //chart.Legend.Position = C1.Chart.Position.None;
            var ms = new MemoryStream();
            chart.SavePng(ms, w, h);
            return new Bitmap(ms);
        }

        public static Bitmap CreateRadarChart(int w, int h, Color foreColor, Color backColor)
        {
            var chart = new FlexRadar()
            {
                //ForeColor = foreColor, 
                //BackColor = backColor 
            };
            chart.Binding = "Value";
            chart.BindingX = "Name";

            for (int iser = 0; iser < 3; iser++)
            {
                var data = new List<object>();
                for (var i = 0; i < 6; i++)
                    data.Add(new { Name = $"S{i}", Value = rnd.NextDouble() });
                var ser = new RadarSeries() { Name = $"ser {iser}", DataSource = data };

                chart.Series.Add(ser);
            }
            //chart.Margin = new Padding(0);
            chart.Legend.Position = C1.Chart.Position.None;
            var ms = new MemoryStream();
            chart.SavePng(ms, w, h);
            return new Bitmap(ms);
        }

        public static Bitmap CreateTreeMap(int w, int h, Color foreColor, Color backColor)
        {
            var treeMap = new TreeMap()
            {
                Binding = "Value",
                BindingName = "Name",
                //ForeColor = foreColor, 
                //BackColor = backColor 
            };

            treeMap.DataSource = new object[] {
                            new { Name = "Group1", Value = 15 },
                            new { Name = "Group2", Value = 12},
                            new { Name = "Group3", Value = 8},
                        };
            //treeMap.Margin = new Padding(0);
            var ms = new MemoryStream();
            treeMap.SavePng(ms, w, h);
            return new Bitmap(ms);
        }


        public static Bitmap CreateSunburstChart(int w, int h, Color foreColor, Color backColor)
        {
            var sunburst = new Sunburst()
            {
                Binding = "sales",
                BindingName = "type",
                ChildItemsPath = "items",
                //ForeColor = foreColor,
                //BackColor = backColor
            };
            sunburst.Offset = 0.2;
            sunburst.DataLabel.Position = C1.Chart.PieLabelPosition.None;
            sunburst.Legend.Position = C1.Chart.Position.None;
            sunburst.DataSource = CreateHierarchicalData();
            //sunburst.Margin = new Padding(0);
            var ms = new MemoryStream();
            sunburst.SavePng(ms, w, h);
            return new Bitmap(ms);
        }

        public class SalesDataItem
        {
            public string? type { get; set; }
            public double sales { get; set; }

            public SalesDataItem[]? items { get; set; }
        }

        static int rand() => rnd.Next(10, 100);

        public static SalesDataItem[] CreateHierarchicalData()
        {
            var data = new SalesDataItem[] {
                    new SalesDataItem {
                        type = "Electronics",
                        items = new SalesDataItem[] {
                            new SalesDataItem{
                                type = "Camera",
                                items = new SalesDataItem[]
                                {
                                        new SalesDataItem{ type = "Digital", sales = rand() },
                                        new SalesDataItem{ type = "Film", sales = rand() },
                                }
                            },
                            new SalesDataItem{
                                type = "Headphones",
                                items = new SalesDataItem[]
                                {
                                        new SalesDataItem{ type = "Earbud", sales = rand() },
                                        new SalesDataItem{ type = "Over-ear", sales = rand() },
                                        new SalesDataItem{ type = "On-ear", sales = rand() },
                                }
                            }
                        }
                    },
                    new SalesDataItem{
                        type = "Computers\n& Tablets",
                        items = new SalesDataItem[]
                        {
                            new SalesDataItem
                            {
                                type = "Desktops",
                                items = new SalesDataItem[]
                                {
                                    new SalesDataItem{ type = "All-in-ones", sales = rand() },
                                    new SalesDataItem{ type = "Minis", sales = rand() },
                                }
                            },
                            new SalesDataItem
                            {
                                type = "Laptops",
                                items = new SalesDataItem[]
                                {
                                    new SalesDataItem{ type = "2 in 1", sales = rand() },
                                    new SalesDataItem{ type = "Traditional", sales = rand() }
                                }
                            },
                        }
                    }
                };
            return data;
        }

        public static Bitmap CreateDiagram(int w, int h, Color foreColor, Color backColor)
        {
            var diagram = new C1.Diagram.Standard.FlexDiagram()
            {
                //ForeColor = foreColor, 
                //BackColor = backColor 
            };
            diagram.Legend.Position = C1.Chart.Position.None;
            var nodes = diagram.Nodes;
            var edges = diagram.Edges;

            for (var i = 0; i < 3; i++)
                nodes.Add(new C1.Diagram.Standard.Node() { Shape = C1.Diagram.Shape.Circle, LegendItem = $"{i + 1}" });
            edges.Add(new C1.Diagram.Standard.Edge() { Source = nodes[0], Target = nodes[1] });
            edges.Add(new C1.Diagram.Standard.Edge() { Source = nodes[0], Target = nodes[2] });
            var ms = new MemoryStream();
            diagram.SavePng(ms, w, h);
            return new Bitmap(ms);
        }*/


    }
}
