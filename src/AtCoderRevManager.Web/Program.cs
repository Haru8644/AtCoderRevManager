using AtCoderRevManager.Web;
using AtCoderRevManager.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

// Render Razor Components
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Map health check endpoints require by Aspire
app.MapDefaultEndpoints();

app.Run();