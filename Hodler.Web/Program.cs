using Hodler.ServiceDefaults;
using Hodler.Web;
using Hodler.Web.Components;
using Hodler.Web.Components.Shared.Services;
using Microsoft.AspNetCore.Identity;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();
builder.AddRedisOutputCache("cache");

// Add services to the container.
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRadzenComponents();

builder.Services
    .AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();

builder.Services.AddHttpClient<HodlerApiClient>(client =>
{
    // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
    // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
    client.BaseAddress = new("https+http://hodler-api");
});

builder.Services.AddScoped<ISessionService, SessionService>();

// ConfigureServices(builder.Services);
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();

// todo: auth
// void ConfigureServices(IServiceCollection services)
// {
//     services.AddBlazoredLocalStorage();
//     services.AddAuthorizationCore();
//     // services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
//     services.AddScoped<IAuthService, AuthService>();
// }