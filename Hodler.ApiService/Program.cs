using Hodler.ApiService;
using Hodler.Application;
using Hodler.Domain;
using Hodler.Integration.ExternalApis;
using Hodler.Integration.Repositories.Users.Entities;
using Hodler.ServiceDefaults;
using Mapster;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();
TypeAdapterConfig.GlobalSettings.Scan(typeof(Program).Assembly);

// todo extend webappbuilder instead
// Hodler Service Layers.
builder.Services
    .AddDomain()
    .AddCustomJwtAuthentication(builder.Configuration)
    .AddApplication()
    .AddExternalApis()
    .AddMailService(builder.Configuration);

// Hodler Service Core
builder.Services
    .AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(Program).Assembly); })
    .AddProblemDetails()
    .AddMvcCore()
    .AddApiExplorer();

// Hodler Service Infrastructure
builder.AddRedisDistributedCache(ServiceConstants.RedisCache);
builder.AddAuthentication(builder.Configuration);
builder.AddRepositories();
builder.AddSwagger();

// TODO: NEED A RETRY POLICY
// builder.Services.AddHostedService<PriceCatalogBroadcastService>();

// SignalR
builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(["application/octet-stream"]);
});
builder.Services.AddCors();

var app = builder.Build();
// SignalR
app.UseResponseCompression();
// Configure the HTTP request pipeline. 
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            $"Hodler API {ServiceConstants.SwaggerDocumentName}");
        c.RoutePrefix = "hodler"; // To serve the Swagger UI at the app's root
    });
}


app.MapDefaultEndpoints();
app.MapControllers();
app.MapIdentityApi<User>();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
// TODO: NEED A RETRY POLICY
// app.MapHub<PriceCatalogHub>("/priceCatalog");
app.Run();