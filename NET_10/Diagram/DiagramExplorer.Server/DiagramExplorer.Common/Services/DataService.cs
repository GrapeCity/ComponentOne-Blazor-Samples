using DiagramExplorer.Models;
using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace DiagramExplorer.Services;

public class DataService
{
    public DataService(HttpClient http)
    {
        Http = http;
    }

    protected HttpClient Http { get; set; } = default!;

    readonly List<Animal> animalData = [];

    public async Task<List<Animal>> GetAnimalData()
    {
        if (animalData.Count == 0)
        {
            var text = await Http.GetStringAsync(GetDataPath("data/animals10.csv"));

            var ss = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < ss.Length; i++)
            {
                var values = ss[i].Split(',');
                animalData.Add(new Animal { Name = values[0], NameScientific = values[1], Phylum = values[2], Class = values[3], Order = values[4] });
            }
        }

        return animalData;
    }

     List<Country> countryData = new List<Country>(100);

    public async Task<List<Country>> GetCountryData()
    {
        if (countryData.Count == 0)
        {
            var text = await Http.GetStringAsync(GetDataPath("data/countries.csv"));

            var ss = text.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < ss.Length; i++)
            {
                var values = ss[i].Split(',');
                if(values.Length >= 4)
                    countryData.Add(new Country { World = "World", Continent = values[0], Region = values[1], Name = values[2], Code = values[3] });
            }
        }

        return countryData;
    }


    public List<OrgNode> GetOrgChartData()
    {
        var company = new OrgNode
        {
            Name = "Acme Corporation",
            Department = "CEO",
            Childs = new List<OrgNode>
                {
                    new OrgNode
                    {
                        FirstName = "Sarah",
                        LastName = "Mitchell",
                        Department = "CEO",
                        Name = "Sarah Mitchell",
                        JobTitle = "CEO",
                        Childs = new List<OrgNode>
                        {
                            new OrgNode
                            {
                                FirstName = "David",
                                LastName = "Chen",
                                Name = "David Chen",
                                Department = "Engineering",
                                JobTitle = "VP Engineering",
                                Childs = new List<OrgNode>
                                {
                                    new OrgNode
                                    {
                                        FirstName = "Amanda",
                                        LastName = "Rodriguez",
                                        Department = "Engineering",
                                        Name = "Amanda Rodriguez",
                                        JobTitle = "Engineering Manager",
                                        Childs = new List<OrgNode>
                                        {
                                            new OrgNode
                                            {
                                                FirstName = "John",
                                                LastName = "Smith",
                                                Department = "Engineering",
                                                Name = "John Smith",
                                                JobTitle = "Senior Software Engineer"
                                            },
                                            new OrgNode
                                            {
                                                FirstName = "Lisa",
                                                LastName = "Wang",
                                                Department = "Engineering",
                                                Name = "Lisa Wang",
                                                JobTitle = "Software Engineer"
                                            },
                                            new OrgNode
                                            {
                                                FirstName = "Michael",
                                                LastName = "Brown",
                                                Department = "Engineering",
                                                Name = "Michael Brown",
                                                JobTitle = "Software Engineer"
                                            }
                                        }
                                    },
                                    new OrgNode
                                    {
                                        FirstName = "Robert",
                                        LastName = "Taylor",
                                        Department = "Engineering",
                                        Name = "Robert Taylor",
                                        JobTitle = "QA Manager",
                                        Childs = new List<OrgNode>
                                        {
                                            new OrgNode
                                            {
                                                FirstName = "Emma",
                                                LastName = "Wilson",
                                                Department = "Engineering",
                                                Name = "Emma Wilson",
                                                JobTitle = "QA Engineer"
                                            },
                                            new OrgNode
                                            {
                                                FirstName = "Chris",
                                                LastName = "Anderson",
                                                Department = "Engineering",
                                                Name = "Chris Anderson",
                                                JobTitle = "QA Engineer"
                                            }
                                        }
                                    }
                                }
                            },
                            new OrgNode
                            {
                                FirstName = "Jennifer",
                                LastName = "Williams",
                                Name = "Jennifer Williams",
                                JobTitle = "VP Product",
                                Department = "Product",
                                Childs = new List<OrgNode>
                                {
                                    new OrgNode
                                    {
                                        FirstName = "Andrew",
                                        LastName = "Miller",
                                        Department = "Product",
                                        Name = "Andrew Miller",
                                        JobTitle = "Senior Product Manager",
                                        Childs = new List<OrgNode>
                                        {
                                            new OrgNode
                                            {
                                                FirstName = "Nicole",
                                                LastName = "Harris",
                                                Department = "Product",
                                                Name = "Nicole Harris",
                                                JobTitle = "Product Manager"
                                            },
                                            new OrgNode
                                            {
                                                FirstName = "Brian",
                                                LastName = "Clark",
                                                Department = "Product",
                                                Name = "Brian Clark",
                                                JobTitle = "Product Manager"
                                            }
                                        }
                                    },
                                    new OrgNode
                                    {
                                        FirstName = "Sophia",
                                        LastName = "Lewis",
                                        Department = "Product",
                                        Name = "Sophia Lewis",
                                        JobTitle = "Design Lead",
                                        Childs = new List<OrgNode>
                                        {
                                            new OrgNode
                                            {
                                                FirstName = "Mark",
                                                LastName = "Robinson",
                                                Department = "Product",
                                                Name = "Mark Robinson",
                                                JobTitle = "UX Designer"
                                            },
                                            new OrgNode
                                            {
                                                FirstName = "Laura",
                                                LastName = "Walker",
                                                Department = "Product",
                                                Name = "Laura Walker",
                                                JobTitle = "UI Designer"
                                            }
                                        }
                                    }
                                }
                            },
                            new OrgNode
                            {
                                FirstName = "Thomas",
                                LastName = "Anderson",
                                Name = "Thomas Anderson",
                                JobTitle = "VP Sales & Marketing",
                                Department = "Sales",
                                Childs = new List<OrgNode>
                                {
                                    new OrgNode
                                    {
                                        FirstName = "Karen",
                                        LastName = "White",
                                        Name = "Karen White",
                                        Department = "Sales",
                                        JobTitle = "Sales Director",
                                        Childs = new List<OrgNode>
                                        {
                                            new OrgNode
                                            {
                                                FirstName = "Paul",
                                                LastName = "Martinez",
                                                Department = "Sales",
                                                Name = "Paul Martinez",
                                                JobTitle = "Account Executive"
                                            },
                                            new OrgNode
                                            {
                                                FirstName = "Rebecca",
                                                LastName = "Young",
                                                Department = "Sales",
                                                Name = "Rebecca Young",
                                                JobTitle = "Account Executive"
                                            },
                                            new OrgNode
                                            {
                                                FirstName = "Frank",
                                                LastName = "Adams",
                                                Department = "Sales",
                                                Name = "Frank Adams",
                                                JobTitle = "Sales Engineer"
                                            }
                                        }
                                    },
                                    new OrgNode
                                    {
                                        FirstName = "Linda",
                                        LastName = "Campbell",
                                        Department = "Sales",
                                        Name = "Linda Campbell",
                                        JobTitle = "Marketing Director",
                                        Childs = new List<OrgNode>
                                        {
                                            new OrgNode
                                            {
                                                FirstName = "Eric",
                                                LastName = "Baker",
                                                Department = "Sales",
                                                Name = "Eric Baker",
                                                JobTitle = "Content Manager"
                                            },
                                            new OrgNode
                                            {
                                                FirstName = "Hannah",
                                                LastName = "Collins",
                                                Department = "Sales",
                                                Name = "Hannah Collins",
                                                JobTitle = "Marketing Specialist"
                                            }
                                        }
                                    }
                                }
                            },
                        }
                    }
                }
        };

        return new List<OrgNode> { company };
    }

    public static Stream? GetResourceStream(string name)
    {
        var asm = Assembly.GetExecutingAssembly();
        return asm.GetManifestResourceStream($"{asm.GetName().Name}.Resources.{name}");
    }

    public string GetDataPath(string name)=> $"{Http.BaseAddress.AbsoluteUri}_content/DiagramExplorer.Common/{name}";
}
