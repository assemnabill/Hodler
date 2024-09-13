var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

const string apiServiceName = "hodler-api";
const string dbName = "hodler-db";

var postgresComponent = builder
    .AddPostgres("postgres")
    .WithDataVolume()
    .WithPgWeb()
    .WithPgAdmin();

var hodlerDb = postgresComponent.AddDatabase(dbName);

var apiService = builder
    .AddProject<Projects.Hodler_ApiService>(apiServiceName)
    .WithReference(hodlerDb);

builder
    .AddProject<Projects.Hodler_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService);

builder.AddProject<Projects.Hodler_Integration_DbMigration>("catalogdbmanager")
    .WithReference(hodlerDb);

builder.Build().Run();