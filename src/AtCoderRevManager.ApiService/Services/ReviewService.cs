using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging; 
using AtCoderRevManager.Domain.Entities;
using AtCoderRevManager.Domain.Interfaces;

namespace AtCoderRevManager.ApiService.Services;

/// <summary>
/// Application service that handles business logic for review management.
/// Orchestrates data flow between the API layer and the repository with observability.
/// </summary>
public class ReviewService : IReviewService
{
    private readonly IReviewRepository _repository;
    private readonly ILogger<ReviewService> _logger;

    // Inject Repository and Logger.
    // Logging is crucial for cloud-native applications to trace behavior in production.
    public ReviewService(IReviewRepository repository, ILogger<ReviewService> logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(logger);

        _repository = repository;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<Review> RegisterReviewAsync(string userId, string problemId, string title, string contestName, int difficulty)
    {
        _logger.LogInformation("Creating new review for User: {UserId}, Problem: {ProblemId}", userId, problemId);

        // Business Rule: Create a valid domain entity enforcing invariants.
        var newReview = new Review(userId, problemId, title, contestName, difficulty);

        await _repository.AddAsync(newReview);
        return newReview;
    }

    /// <inheritdoc />
    public async Task<Review?> GetReviewAsync(string id, string userId)
    {
        return await _repository.GetByIdAsync(id, userId);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Review>> GetUserReviewsAsync(string userId)
    {
        return await _repository.GetAllByUserIdAsync(userId);
    }

    /// <inheritdoc />
    public async Task UpdateReviewProgressAsync(string id, string userId, bool isSolved, string notes)
    {
        // Fail-fast: Ensure the entity exists before attempting mutation to prevent data inconsistency.
        var review = await _repository.GetByIdAsync(id, userId);
        if (review == null)
        {
            _logger.LogWarning("Attempted to update non-existent review. ID: {Id}, User: {UserId}", id, userId);
            throw new KeyNotFoundException($"Review with ID {id} not found.");
        }

        // Apply domain logic (state transition).
        review.UpdateProgress(isSolved, notes);

        await _repository.UpdateAsync(review);

        _logger.LogInformation("Review progress updated. ID: {Id}, Solved: {IsSolved}", id, isSolved);
    }

    /// <inheritdoc />
    public async Task DeleteReviewAsync(string id, string userId)
    {
        _logger.LogInformation("Deleting review. ID: {Id}, User: {UserId}", id, userId);
        await _repository.DeleteAsync(id, userId);
    }
}