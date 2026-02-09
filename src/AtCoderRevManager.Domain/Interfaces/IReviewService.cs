using System.Collections.Generic;
using System.Threading.Tasks;
using AtCoderRevManager.Domain.Entities;

namespace AtCoderRevManager.Domain.Interfaces;

/// <summary>
/// Defines the business logic contract for managing review cycles and progress.
/// Acts as an intermediary between the API controller and the data repository.
/// </summary>
public interface IReviewService
{
    /// <summary>
    /// Creates a new review entry for a problem.
    /// </summary>
    Task<Review> RegisterReviewAsync(string userId, string problemId, string title, string contestName, int difficulty);

    /// <summary>
    /// Retrieves a single review detail.
    /// </summary>
    Task<Review?> GetReviewAsync(string id, string userId);

    /// <summary>
    /// Retrieves all reviews for a specific user.
    /// </summary>
    Task<IEnumerable<Review>> GetUserReviewsAsync(string userId);

    /// <summary>
    /// Updates the review status, solving progress, and reflection notes.
    /// </summary>
    Task UpdateReviewProgressAsync(string id, string userId, bool isSolved, string notes);

    /// <summary>
    /// Deletes a review entry.
    /// </summary>
    Task DeleteReviewAsync(string id, string userId);
}