using DiagramExplorer.Components;
using DiagramExplorer.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<DataService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5089/");
});
builder.Services.AddSingleton<SampleService>();


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode().AddAdditionalAssemblies(typeof(DiagramExplorer.Components.Pages.Home).Assembly); ;

app.Run();
