using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorExplorer
{
    public class Country
    {
        public override string ToString()
        {
            return Name;
        }

        public string Code { get; set; }
        public string Name { get; set; }

        public static List<Country> GetCountries()
        {

            var assembly = typeof(Country).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("BlazorExplorer.Data.countries.json");
            var json = new System.IO.StreamReader(stream).ReadToEnd();
            var countries = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

            var listContries = new List<Country>();

            foreach (var item in countries)
            {
                listContries.Add(new Country() { Code = item.Key, Name = item.Value.ToString() });
            }
            return listContries.OrderBy(c => c.Name).ToList();
        }

        public string FlagUrl
        {
            get
            {
				return $"https://raw.githubusercontent.com/GrapeCity/ComponentOne-Blazor-Samples/master/Images/samples/flags/{Code}.png";
            }
        }
    }
}
