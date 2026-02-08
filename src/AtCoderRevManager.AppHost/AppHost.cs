var builder = DistributedApplication.CreateBuilder(args);

// ========================================================================
// 1. Infrastructure Definitions
// ========================================================================

// Cosmos DB Emulator setup.
// Enables data persistence to retain data across emulator restarts.
var cosmos = builder.AddAzureCosmosDB("cosmos")
                    .RunAsEmulator(emulator =>
                    {
                        emulator.WithDataVolume();
                        emulator.WithGatewayPort(8081); // Standard port for local development
                    });

// Define Database and Container.
// Using 'AddCosmosDatabase' ensures type safety over the generic 'AddDatabase'.
var db = cosmos.AddCosmosDatabase("AtCoderRevDb");

// Partition Key Strategy: "/userId"
// Choosen to optimize read queries which are heavily scoped to a single user.
// This aligns with the single-partition query best practices in Azure Cosmos DB.
var reviews = db.AddContainer("reviews", "/userId");

// ========================================================================
// 2. Service Registrations
// ========================================================================

// Backend API Service.
// Injects the Cosmos DB connection string reference.
var apiService = builder.AddProject<Projects.AtCoderRevManager_ApiService>("apiservice")
                        .WithReference(reviews);

// Frontend Web Application.
// Exposes external HTTP endpoints for browser access and connects to the backend API.
builder.AddProject<Projects.AtCoderRevManager_Web>("webfrontend")
       .WithExternalHttpEndpoints()
       .WithReference(apiService);

// ========================================================================
// 3. Application Execution
// ========================================================================
builder.Build().Run();