using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging; // Added for Observability
using AtCoderRevManager.Domain.Entities;
using AtCoderRevManager.Domain.Interfaces;

namespace AtCoderRevManager.Infrastructure.Repositories;

/// <summary>
/// Infrastructure implementation of the review repository using Azure Cosmos DB SDK.
/// Handles CRUD operations with strict adherence to partition strategies and observability standards.
/// </summary>
public class CosmosReviewRepository : IReviewRepository
{
    private readonly Microsoft.Azure.Cosmos.Container _container;
    private readonly ILogger<CosmosReviewRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CosmosReviewRepository"/> class.
    /// </summary>
    /// <param name="cosmosClient">Singleton Cosmos DB client.</param>
    /// <param name="databaseName">Target database name.</param>
    /// <param name="containerName">Target container name.</param>
    /// <param name="logger">Logger for tracing database operations.</param>
    public CosmosReviewRepository(
        CosmosClient cosmosClient,
        string databaseName,
        string containerName,
        ILogger<CosmosReviewRepository> logger)
    {
        // Fail-fast if dependencies are missing.
        ArgumentNullException.ThrowIfNull(cosmosClient);
        ArgumentNullException.ThrowIfNull(logger);
        if (string.IsNullOrWhiteSpace(databaseName)) throw new ArgumentNullException(nameof(databaseName));
        if (string.IsNullOrWhiteSpace(containerName)) throw new ArgumentNullException(nameof(containerName));

        _container = cosmosClient.GetContainer(databaseName, containerName);
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task AddAsync(Review review)
    {
        try
        {
            // Scalability Strategy: Enforce PartitionKey to ensure data is distributed evenly across physical partitions.
            await _container.CreateItemAsync(review, new PartitionKey(review.UserId));
            _logger.LogInformation("Review created successfully. ID: {Id}, User: {UserId}", review.Id, review.UserId);
        }
        catch (CosmosException ex)
        {
            _logger.LogError(ex, "Failed to create review. ID: {Id}", review.Id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<Review?> GetByIdAsync(string id, string userId)
    {
        try
        {
            // Cost Optimization: Use Point Read (1 RU) instead of Query.
            // Direct access via ID and Partition Key is the most cost-effective retrieval method.
            ItemResponse<Review> response = await _container.ReadItemAsync<Review>(id, new PartitionKey(userId));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogWarning("Review not found. ID: {Id}, User: {UserId}", id, userId);
            return null;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Review>> GetAllByUserIdAsync(string userId)
    {
        // Performance Strategy: Single-Partition Query.
        // Scoping to PartitionKey prevents expensive cross-partition fan-out queries.
        var query = new QueryDefinition("SELECT * FROM c WHERE c.UserId = @userId")
            .WithParameter("@userId", userId);

        using FeedIterator<Review> iterator = _container.GetItemQueryIterator<Review>(
            query,
            requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(userId) }
        );

        var results = new List<Review>();
        while (iterator.HasMoreResults)
        {
            FeedResponse<Review> response = await iterator.ReadNextAsync();
            results.AddRange(response);

            // In a real scenario, we would log the Request Charge (RU) here for monitoring.
            // _logger.LogDebug("Query consumed {RUs} RUs", response.RequestCharge);
        }
        return results;
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Review review)
    {
        // Use Upsert for idempotency (Create or Replace).
        await _container.UpsertItemAsync(review, new PartitionKey(review.UserId));
        _logger.LogInformation("Review updated. ID: {Id}", review.Id);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(string id, string userId)
    {
        await _container.DeleteItemAsync<Review>(id, new PartitionKey(userId));
        _logger.LogInformation("Review deleted. ID: {Id}", id);
    }
}