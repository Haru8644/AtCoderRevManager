var builder = DistributedApplication.CreateBuilder(args);

// Infrastructure
var sql = builder.AddSqlServer("sqlserver")
                 .WithDataVolume()
                 .AddDatabase("AtCoderRevDb");

// Services
var apiService = builder.AddProject<Projects.AtCoderRevManager_ApiService>("apiservice")
                        .WithReference(sql);

builder.AddProject<Projects.AtCoderRevManager_Web>("webfrontend")
       .WithExternalHttpEndpoints()
       .WithReference(apiService);

builder.Build().Run();