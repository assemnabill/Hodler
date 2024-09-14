
var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

const string apiServiceName = "hodler-api";
const string frontendComponentName = "hodler-web";
const string dbName = "hodler-db";

var postgresComponent = builder
    .AddPostgres("postgres")
    .WithDataVolume()
    .WithPgWeb();

var hodlerDb = postgresComponent.AddDatabase(dbName);

var apiService = builder
    .AddProject<Projects.Hodler_ApiService>(apiServiceName)
    .WithReference(hodlerDb);

builder
    .AddProject<Projects.Hodler_Web>(frontendComponentName)
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService);

builder.AddProject<Projects.Hodler_Integration_DbMigration>("hodler-db-manager")
    .WithReference(hodlerDb);

builder.Build().Run();