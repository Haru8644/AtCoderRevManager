using System.Collections.Generic;
using System.Threading.Tasks;
using AtCoderRevManager.Domain.Entities;

namespace AtCoderRevManager.Domain.Interfaces;

/// <summary>
/// Defines the business logic contract for managing review cycles.
/// </summary>
public interface IReviewService
{
    /// <summary>
    /// Registers a new review entry.
    /// </summary>
    Task<Review> RegisterReviewAsync(RegisterReviewCommand command);

    /// <summary>
    /// Retrieves a single review by ID and UserID.
    /// </summary>
    Task<Review?> GetReviewAsync(string id, string userId);

    /// <summary>
    /// Retrieves all reviews for a specific user.
    /// </summary>
    Task<IEnumerable<Review>> GetUserReviewsAsync(string userId);

    /// <summary>
    /// Updates the progress status of a review.
    /// </summary>
    Task UpdateReviewProgressAsync(string id, string userId, bool isSolved, string notes);

    /// <summary>
    /// Deletes a review entry.
    /// </summary>
    Task DeleteReviewAsync(string id, string userId);
}

public record RegisterReviewCommand(string UserId, string ProblemId, string Title, string ContestName, int Difficulty);