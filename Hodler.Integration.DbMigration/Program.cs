using Hodler.Integration.DbMigration;
using Hodler.Integration.Repositories.Portfolios.Context;
using Hodler.Integration.Repositories.Users.Context;
using Hodler.ServiceDefaults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<PortfolioDbContext>(
    "hodler-db",
    null,
    optionsBuilder => optionsBuilder.UseNpgsql(npgsqlBuilder =>
        npgsqlBuilder.MigrationsAssembly(typeof(PortfolioDbContext).Assembly.GetName().Name)
    )
);

builder.AddNpgsqlDbContext<UserDbContext>(
    "hodler-db",
    null,
    optionsBuilder => optionsBuilder.UseNpgsql(npgsqlBuilder =>
        npgsqlBuilder.MigrationsAssembly(typeof(UserDbContext).Assembly.GetName().Name)
    )
);

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(HodlerDbInitializer.ActivitySourceName));

builder.Services.AddSingleton<HodlerDbInitializer>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<HodlerDbInitializer>());
// builder.Services.AddHealthChecks()
//     .AddCheck<HodlerDbInitializerHealthCheck>("DbInitializer", null);

var app = builder.Build();

app.MapDefaultEndpoints();

await app.RunAsync();