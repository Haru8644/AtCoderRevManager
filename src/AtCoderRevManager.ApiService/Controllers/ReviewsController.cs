using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using AtCoderRevManager.Domain.Entities;
using AtCoderRevManager.Domain.Interfaces;

namespace AtCoderRevManager.ApiService.Controllers;

[ApiController]
[Route("api/users/{userId}/reviews")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _service;

    public ReviewsController(IReviewService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ReviewResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReviewResponse>> CreateReview(string userId, [FromBody] CreateReviewRequest request)
    {
        var command = new RegisterReviewCommand(
            userId,
            request.ProblemId,
            request.Title,
            request.ContestName,
            request.Difficulty
        );

        var review = await _service.RegisterReviewAsync(command);

        var response = ReviewResponse.FromEntity(review);
        return CreatedAtAction(nameof(GetReview), new { userId = review.UserId, id = review.Id }, response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ReviewResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReviewResponse>> GetReview(string userId, string id)
    {
        var review = await _service.GetReviewAsync(id, userId);

        if (review == null) return NotFound();

        return Ok(ReviewResponse.FromEntity(review));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReviewResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ReviewResponse>>> GetUserReviews(string userId)
    {
        var reviews = await _service.GetUserReviewsAsync(userId);
        return Ok(reviews.Select(ReviewResponse.FromEntity));
    }

    [HttpPut("{id}/progress")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProgress(string userId, string id, [FromBody] UpdateProgressRequest request)
    {
        try
        {
            await _service.UpdateReviewProgressAsync(id, userId, request.IsSolved, request.Notes);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteReview(string userId, string id)
    {
        await _service.DeleteReviewAsync(id, userId);
        return NoContent();
    }
}

// --- DTO Definitions ---

public record CreateReviewRequest(
    [Required] string ProblemId,
    [Required] string Title,
    string ContestName,
    [Range(0, 5000)] int Difficulty
);

public record UpdateProgressRequest(bool IsSolved, string Notes);

public record ReviewResponse(
    string Id,
    string UserId,
    string ProblemId,
    string Title,
    string ContestName,
    int Difficulty,
    bool IsSolved,
    string Notes,
    DateTime CreatedAt
)
{
    public static ReviewResponse FromEntity(Review r) =>
        new(r.Id, r.UserId, r.ProblemId, r.Title, r.ContestName, r.Difficulty, r.IsSolved, r.Notes, r.CreatedAt);
}