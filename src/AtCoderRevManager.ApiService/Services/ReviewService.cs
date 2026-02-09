using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AtCoderRevManager.Domain.Entities;
using AtCoderRevManager.Domain.Interfaces;

namespace AtCoderRevManager.ApiService.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _repository;
    private readonly ILogger<ReviewService> _logger;

    public ReviewService(IReviewRepository repository, ILogger<ReviewService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Review> RegisterReviewAsync(RegisterReviewCommand command)
    {
        _logger.LogInformation("Creating review. User: {UserId}, Problem: {ProblemId}", command.UserId, command.ProblemId);

        var newReview = new Review(
            command.UserId,
            command.ProblemId,
            command.Title,
            command.ContestName,
            command.Difficulty
        );

        await _repository.AddAsync(newReview);
        return newReview;
    }

    public async Task<Review?> GetReviewAsync(string id, string userId)
    {
        return await _repository.GetByIdAsync(id, userId);
    }

    public async Task<IEnumerable<Review>> GetUserReviewsAsync(string userId)
    {
        return await _repository.GetAllByUserIdAsync(userId);
    }

    public async Task UpdateReviewProgressAsync(string id, string userId, bool isSolved, string notes)
    {
        var review = await _repository.GetByIdAsync(id, userId);
        if (review == null)
        {
            _logger.LogWarning("Update target not found. ID: {Id}, User: {UserId}", id, userId);
            throw new KeyNotFoundException($"Review with ID {id} not found.");
        }

        review.UpdateProgress(isSolved, notes);
        await _repository.UpdateAsync(review);

        _logger.LogInformation("Review progress updated. ID: {Id}", id);
    }

    public async Task DeleteReviewAsync(string id, string userId)
    {
        _logger.LogInformation("Deleting review. ID: {Id}", id);
        await _repository.DeleteAsync(id, userId);
    }
}