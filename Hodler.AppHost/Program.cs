var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

const string serviceName = "hodler";
const string apiServiceName = $"{serviceName}-api";
const string webFrontendName = $"{serviceName}-web";
const string dbName = $"{serviceName}-db";

var postgresComponent = builder
    .AddPostgres("postgres")
    .WithDataVolume()
    .WithPgWeb();

var hodlerDb = postgresComponent.AddDatabase(dbName);

var apiService = builder
    .AddProject<Projects.Hodler_ApiService>(apiServiceName)
    .WithReference(hodlerDb);

builder
    .AddProject<Projects.Hodler_Web>(webFrontendName)
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService);

builder
    .AddProject<Projects.Hodler_Integration_DbMigration>($"{serviceName}-db-manager")
    .WithReference(hodlerDb);

builder.Build().Run();