using AtCoderRevManager.Web.Components;
using AtCoderRevManager.Web.Services;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// --- Infrastructure & Aspire Integrations ---
builder.AddServiceDefaults();

// --- UI Frameworks & Components ---
// Register Fluent UI components for consistent design system usage.
builder.Services.AddFluentUIComponents();

// Configure Razor Components with Interactive Server mode.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// --- HTTP Clients & Service Discovery ---
// Register the typed HttpClient with Aspire Service Discovery ("apiservice").
// This abstracts the backend URL, enabling seamless connectivity across environments.
builder.Services.AddHttpClient<ReviewApiClient>(client =>
    client.BaseAddress = new("https+http://apiservice"));

var app = builder.Build();

// --- HTTP Request Pipeline ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

// Map Razor Components (Server-Side Rendering + Interactivity)
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Expose health check endpoints for Aspire orchestration.
app.MapDefaultEndpoints();

app.Run();