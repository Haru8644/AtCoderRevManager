using System;

namespace AtCoderRevManager.Domain.Entities;

/// <summary>
/// Represents a review record for a competitive programming problem.
/// </summary>
public class Review
{
    public string Id { get; private set; } = Guid.NewGuid().ToString();

    // Partition Key
    public string UserId { get; private set; }

    public string ProblemId { get; private set; }
    public string Title { get; private set; }
    public string ContestName { get; private set; }
    public int Difficulty { get; private set; }
    public bool IsSolved { get; private set; }
    public string Notes { get; private set; }
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Review"/> class.
    /// Enforces invariants: UserId, ProblemId, Title, and ContestName must not be empty.
    /// </summary>
    public Review(string userId, string problemId, string title, string contestName, int difficulty)
    {
        if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentNullException(nameof(userId));
        if (string.IsNullOrWhiteSpace(problemId)) throw new ArgumentNullException(nameof(problemId));
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentNullException(nameof(title));
        if (string.IsNullOrWhiteSpace(contestName)) throw new ArgumentNullException(nameof(contestName));

        UserId = userId;
        ProblemId = problemId;
        Title = title;
        ContestName = contestName;
        Difficulty = difficulty;

        IsSolved = false;
        Notes = string.Empty;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the review progress and reflection notes.
    /// </summary>
    public void UpdateProgress(bool isSolved, string notes)
    {
        IsSolved = isSolved;
        Notes = notes ?? string.Empty;
    }
}