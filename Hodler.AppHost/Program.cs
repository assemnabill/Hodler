using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

const string serviceName = "hodler";
const string apiServiceName = $"{serviceName}-api";
const string dbName = $"{serviceName}-db";

var postgresComponent = builder
    .AddPostgres("postgres")
    .WithDataVolume();

var hodlerDb = postgresComponent.AddDatabase(dbName);

var apiService = builder
    .AddProject<Hodler_ApiService>(apiServiceName)
    .WithReference(hodlerDb)
    .WithReference(cache)
    .WaitFor(hodlerDb);


builder.Build().Run();