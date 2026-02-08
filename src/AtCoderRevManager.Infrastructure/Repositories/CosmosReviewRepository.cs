using AtCoderRevManager.Domain.Entities;
using AtCoderRevManager.Domain.Interfaces;
using Microsoft.Azure.Cosmos;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;

namespace AtCoderRevManager.Infrastructure.Repositories;

/// <summary>
/// Implementation of the review repository using Azure Cosmos DB SDK.
/// Handles CRUD operations directly against the Cosmos container.
/// </summary>
public class CosmosReviewRepository : IReviewRepository
{
    private readonly Container _container;

    /// <summary>
    /// Initializes a new instance of the <see cref="CosmosReviewRepository"/> class.
    /// </summary>
    /// <param name="cosmosClient">The injected Cosmos DB client.</param>
    /// <param name="databaseName">Target database name.</param>
    /// <param name="containerName">Target container name.</param>
    public CosmosReviewRepository(CosmosClient cosmosClient, string databaseName, string containerName)
    {
        _container = cosmosClient.GetContainer(databaseName, containerName);
    }

    /// <inheritdoc />
    public async Task AddAsync(Review review)
    {
        // AZ-204: Always provide PartitionKey when creating items.
        await _container.CreateItemAsync(review, new PartitionKey(review.UserId));
    }

    /// <inheritdoc />
    public async Task<Review?> GetByIdAsync(string id, string userId)
    {
        try
        {
            // AZ-204: Point Read (ReadItemAsync) is the most efficient operation (1 RU).
            ItemResponse<Review> response = await _container.ReadItemAsync<Review>(id, new PartitionKey(userId));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Review>> GetAllByUserIdAsync(string userId)
    {
        // AZ-204: Single-Partition Query.
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
        }
        return results;
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Review review)
    {
        await _container.UpsertItemAsync(review, new PartitionKey(review.UserId));
    }

    /// <inheritdoc />
    public async Task DeleteAsync(string id, string userId)
    {
        await _container.DeleteItemAsync<Review>(id, new PartitionKey(userId));
    }
}