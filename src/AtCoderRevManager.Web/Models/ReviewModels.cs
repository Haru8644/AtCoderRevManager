using System.ComponentModel.DataAnnotations;

namespace AtCoderRevManager.Web.Models;

// Response DTO: Immutable by design using positional records.
public record ReviewModel(
    string Id,
    string UserId,
    string ProblemId,
    string Title,
    string ContestName,
    int Difficulty,
    bool IsSolved,
    string Notes,
    DateTime CreatedAt
);

// Request DTO: Uses 'required' to enforce initialization at compile time.
public record CreateReviewRequest
{
    [Required]
    public required string ProblemId { get; init; }

    [Required]
    public required string Title { get; init; }

    public string ContestName { get; init; } = string.Empty;

    [Range(0, 5000)]
    public int Difficulty { get; init; }
}

public record UpdateProgressRequest(bool IsSolved, string Notes);