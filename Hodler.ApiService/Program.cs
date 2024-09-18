using Hodler.ApiService;
using Hodler.Application;
using Hodler.Domain;
using Hodler.Integration.Repositories;
using Hodler.Integration.ExternalApis;
using Hodler.Integration.Repositories.User.Entities;
using Hodler.ServiceDefaults;
using Mapster;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();
TypeAdapterConfig.GlobalSettings.Scan(typeof(Program).Assembly);

// Hodler Service Layers.
builder.Services
    .AddDomain()
    .AddApplication()
    .AddExternalApis()
    .AddRepositories();

// Hodler Service Core
builder.Services
    .AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(Program).Assembly); })
    .AddProblemDetails()
    .AddMvcCore()
    .AddApiExplorer();

// Hodler Service Infrastructure
builder.AddRedisDistributedCache("cache");
builder.AddAuthentication(builder.Configuration);
builder.AddDbContexts();
builder.AddSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            $"Hodler API {ServiceConstants.ApiVersion}");
        c.RoutePrefix = "hodler"; // To serve the Swagger UI at the app's root
    });
}


app.MapDefaultEndpoints();
app.MapControllers();
app.MapIdentityApi<User>();
app.UseAuthentication();
app.UseAuthorization();
app.Run();