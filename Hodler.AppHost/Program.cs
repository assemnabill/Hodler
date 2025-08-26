using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddDockerComposeEnvironment("compose");

var cache = builder
    .AddRedis("cache")
    .WithContainerName("redis-cache")
    .WithImageTag("7-alpine"); // ARM-compatible tag

const string serviceName = "hodler";
const string apiServiceName = $"{serviceName}-api";
const string dbName = $"{serviceName}-db";

var postgresComponent = builder
    .AddPostgres("postgres", password: builder.AddParameter("postgres-password"))
    .WithContainerName(dbName)
    .WithDataVolume($"{dbName}-vol")
    .WithImageTag("15-alpine"); // ARM-compatible tag

var hodlerDb = postgresComponent
    .AddDatabase(dbName);

var apiService = builder
    .AddProject<Hodler_ApiService>(apiServiceName)
    .WithReference(hodlerDb)
    .WithReference(cache)
    .WaitFor(hodlerDb);


builder.Build().Run();