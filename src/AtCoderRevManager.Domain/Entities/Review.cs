using System;

namespace AtCoderRevManager.Domain.Entities;

public class Review
{
    public string Id { get; private set; } = Guid.NewGuid().ToString();
    public string UserId { get; private set; }
    public string ProblemId { get; private set; }
    public string Title { get; private set; }
    public string ContestName { get; private set; }
    public int Difficulty { get; private set; }
    public bool IsSolved { get; private set; }
    public string Notes { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public int ReviewCount { get; private set; }
    public DateTime NextReviewDate { get; private set; }

    public Review(string userId, string problemId, string title, string contestName, int difficulty)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        ArgumentException.ThrowIfNullOrWhiteSpace(problemId);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        UserId = userId;
        ProblemId = problemId;
        Title = title;
        ContestName = contestName ?? string.Empty;
        Difficulty = difficulty;
        IsSolved = false;
        Notes = string.Empty;
        CreatedAt = DateTime.UtcNow;

        ReviewCount = 0;
        NextReviewDate = DateTime.UtcNow.AddDays(1);
    }

    public void UpdateProgress(bool isSolved, string notes)
    {
        IsSolved = isSolved;
        Notes = notes ?? string.Empty;

        if (isSolved)
        {
            ReviewCount++;
            int daysToAdd = ReviewCount switch
            {
                1 => 1,
                2 => 3,
                3 => 7,
                4 => 21,
                _ => 30
            };
            NextReviewDate = DateTime.UtcNow.AddDays(daysToAdd);
        }
        else
        {
            ReviewCount = 0;
            NextReviewDate = DateTime.UtcNow.AddDays(1);
        }
    }
}