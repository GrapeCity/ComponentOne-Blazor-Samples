using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorExplorer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton<ViewBagService>();
            builder.Services.AddLocalization();
            
            SetSpecificCulture();

            await builder.Build().RunAsync();
        }
        private static void SetSpecificCulture()
        {
            var culture = CultureInfo.CurrentCulture;
            if (!culture.IsNeutralCulture)
            {
                // Current culture is specific. Nothing needs to do.
                return;
            }

            try
            {
                CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture(culture.Name);
            }
            catch (Exception ex) when (ex is CultureNotFoundException || ex is ArgumentException)
            {
                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-us");
            }
        }
    }
}
