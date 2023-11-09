using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlazorExplorer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //C1.Blazor.Core.LicenseManager.Key = License.Key;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<ViewBagService>();
            services.AddSingleton<WeatherExcelService>();
            services.AddSingleton<WeatherForecastService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            
            app.UseStaticFiles();

            app.UseRouting();
            // Allowed using of date, number formats for all available cultures.
            var allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures).Select(c=>c.Name)
                                .Append("zh-CN").ToArray();
            
            var localizationOptions = new RequestLocalizationOptions()
                .AddSupportedCultures(allCultures)
                .AddSupportedUICultures(allCultures)
                .SetDefaultCulture("en");
            app.UseRequestLocalization(localizationOptions);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }

    public static class License
    {
        public const string Key =
            "ACQBJAIkB31CAGwAYQB6AG8AcgBFAHgAcABsAG8AcgBlAHIAgmaNLhQAvEXcK9gm" +
            "UjG1Mka5XqjH2vf1pgW91zO3vNQGhA5tPC4b4k6L0uC27UTsVqZrWArxTXgid3pX" +
            "ExdpIKCldG8MEmSUXMgzKrFrm0Y2zt+MMegzhQjPVqX+VbX7gZFUsXQSgmR6IiHA" +
            "3NU+oHChxAADOFpvlq2jPonXMSGV5LihFijjNf/834bGigT9mBUpIJkoTW/LzcMC" +
            "ILQ4FFyeIy6s3d57jQwkr6ZpTQBxan1RIE0nYtBNpJUQcCuFvft1iMms7UJRHIdu" +
            "7ngLdxlkdQulDja8cuKOLQytfczqpIx3yOrTchFx47JONILtB+LdzgLjfZSznDEu" +
            "YhMuaFMOmYHCrwp/ZCKlt8yx1IY4HXYbK5SlkDWOziDCJffi1/1iG778bwYC6di9" +
            "9TkyYBoyj6rE0a8BCy47uiPIslSyTzaVY4F9AcVz7WJU13yPvKMES/V7rkv7RNsp" +
            "KVcT40BoMWk+9zS/czR69pjmKFBCnCUjbIOg2Q+EEUd7A/CT9r42cVDdwvGEIY+L" +
            "RrBAFa00DCR4/UxVxjcE/B1EWcatxUZFO6gmbTIpb04QOX/Zfwe/pOy/Z/FdRooC" +
            "UYe7wt2TMnwFRKU4G1inNmv789Xa3J2GIz+/t+QCsnLnkb8pOR65XIdXc+VvwqCh" +
            "z5wlg1lgbZbWHWcA8APweeggTY8wggVVMIIEPaADAgECAhBBA3jSJjZZehbbJsa9" +
            "EJSLMA0GCSqGSIb3DQEBBQUAMIG0MQswCQYDVQQGEwJVUzEXMBUGA1UEChMOVmVy" +
            "aVNpZ24sIEluYy4xHzAdBgNVBAsTFlZlcmlTaWduIFRydXN0IE5ldHdvcmsxOzA5" +
            "BgNVBAsTMlRlcm1zIG9mIHVzZSBhdCBodHRwczovL3d3dy52ZXJpc2lnbi5jb20v" +
            "cnBhIChjKTEwMS4wLAYDVQQDEyVWZXJpU2lnbiBDbGFzcyAzIENvZGUgU2lnbmlu" +
            "ZyAyMDEwIENBMB4XDTE0MTIxMTAwMDAwMFoXDTE1MTIyMjIzNTk1OVowgYYxCzAJ" +
            "BgNVBAYTAkpQMQ8wDQYDVQQIEwZNaXlhZ2kxGDAWBgNVBAcTD1NlbmRhaSBJenVt" +
            "aS1rdTEXMBUGA1UEChQOR3JhcGVDaXR5IGluYy4xGjAYBgNVBAsUEVRvb2xzIERl" +
            "dmVsb3BtZW50MRcwFQYDVQQDFA5HcmFwZUNpdHkgaW5jLjCCASIwDQYJKoZIhvcN" +
            "AQEBBQADggEPADCCAQoCggEBAMEC9splW7KVpAqRB6VVB7XJm8+a60x78HZWNmvA" +
            "js/ermp7pbh8M9HYuEVTRj8H4baXxFYq4i75e5kNr7s+nGbJ5UfM6CldqbeRsQ4z" +
            "vRb6npfrtcOBwfYOx0woeYQfI6VzLVzhPbGjUDK6qdHZLf/EcvSIKvRbu3ILOE3k" +
            "uux07gQkb++rrSBUyJKXU8nW8c+K+9sfWqHYPb8FF2IazWxhmfIdIoDNKyjr3r/w" +
            "In3jQejpqfamiHNsU/O26KS0EcvCS5CFOKDT6vvdnAkek3kyLWef+ca/cbFo74va" +
            "wYGtmw2wzw/hcwlANIQaCAxz+5JHQZ3SEg8LLSZ+C8E4T+8CAwEAAaOCAY0wggGJ" +
            "MAkGA1UdEwQCMAAwDgYDVR0PAQH/BAQDAgeAMCsGA1UdHwQkMCIwIKAeoByGGmh0" +
            "dHA6Ly9zZi5zeW1jYi5jb20vc2YuY3JsMGYGA1UdIARfMF0wWwYLYIZIAYb4RQEH" +
            "FwMwTDAjBggrBgEFBQcCARYXaHR0cHM6Ly9kLnN5bWNiLmNvbS9jcHMwJQYIKwYB" +
            "BQUHAgIwGQwXaHR0cHM6Ly9kLnN5bWNiLmNvbS9ycGEwEwYDVR0lBAwwCgYIKwYB" +
            "BQUHAwMwVwYIKwYBBQUHAQEESzBJMB8GCCsGAQUFBzABhhNodHRwOi8vc2Yuc3lt" +
            "Y2QuY29tMCYGCCsGAQUFBzAChhpodHRwOi8vc2Yuc3ltY2IuY29tL3NmLmNydDAf" +
            "BgNVHSMEGDAWgBTPmanqeyb0S8mOj9fwBSbv49KnnTAdBgNVHQ4EFgQUAFrwraXU" +
            "eDX1hBKoLAUO+FYbjo4wEQYJYIZIAYb4QgEBBAQDAgQQMBYGCisGAQQBgjcCARsE" +
            "CDAGAQEAAQH/MA0GCSqGSIb3DQEBBQUAA4IBAQCIwphaN45b5ViKsRfCA6hbVeqM" +
            "hNCJmL64mqxdYvw3gsF4oOCDoiM6kWhy1NoUd2Lpcr9PjfGJ55ZX4sZlqoxeflTp" +
            "HgnX8MPJuKpDBsk7WkhiEu65fBwb+g9wQHMMfgY0gjHIAQ2ZEf2PJ6TTSFcvj24w" +
            "h73dsyqcmj290fpwDtHaF2jE5pq/tQBOJ8vDkN46wt4qMhG6nIpOgc9KngyLLdEO" +
            "7V34KM1Nb+sN9JkqTWXNoNqlOf7QjYHwlw7Ku7W3kIGK+f1kP1xqTBpXwsBE5sLl" +
            "nujP+JAqV8aKlBJDKRI3IhlrG9CJUmqFNhmZ2f5QE5jAvPQeXevLnMYiqJrgMIIC" +
            "mjCCAYKgAwIBAgIEDu7u7jANBgkqhkiG9w0BAQUFADAPMQ0wCwYDVQQDDARHQy0x" +
            "MB4XDTIwMDQwOTExMjY1N1oXDTI1MDQwOTExMjY1N1owDzENMAsGA1UEAwwER0Mt" +
            "MTCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAKMVC4+oYpSxkJ6+FYUV" +
            "l/o4RnIexQehOUGsh+mlzOsVSfVq3OkEiI1HXqH27gwoTab4CBWpKTzMcKEluXVY" +
            "cmGJVbQQiCPYABFCiZg8w2l1SFn/dfwrK0jAzJgt4GKODVvh/ZsB1jUt8FSd4Wk4" +
            "pLV8cz14YbLcL/XaeN/FwRr9fu6UzZFbgT5eIReYA3Gy9Trcn6jQeR7AfFDFoibe" +
            "w5BXlBJrRfotY+7GTTBtSMG4c/iKfGIs6bQJ+G4Vp2bZK10lGi0AzfJWVQrw6WDM" +
            "AHG/AVMkPuXhQo7za1GQdgHB6Ya/U76G27mtu0ax+NeAvBk7XZNlCkcPZt9k+s1z" +
            "SlcCAwEAATANBgkqhkiG9w0BAQUFAAOCAQEAif96xs+PxPY2uZAhP8nSrhVdZ1if" +
            "d4kGOBHNvqzvWYVpgXXe4IvqUi3spqahfAQUVVcB4vMqoj0vsGiJHvprtVfXZk6R" +
            "ZMSm/Gd8SIrB4JBbUCxS+grGUANfGEyBjbd4BbdfGXnI+F0r9V+psydAcu6GbfW0" +
            "JKkt6hqfEsXtjP8uHYRbAJ+2jcNjyLnG/TkSAl9xNLaVhwSqE5HlUiONAWeymDcE" +
            "QFQMd3weTX8waBuycZpEhQAkiFkGijP7459GQbCMjnpjFKqD8Fc6yaDPUi8LwJRg" +
            "ZK5/gnlaQYoQT1TZSFM3xIGCTKoSG049aSeVlJLWKHLMRcu5MkCs1+cSYw==";
    }

}
