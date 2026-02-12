using System.Net.Http.Json;
using AtCoderRevManager.Web.Models;

namespace AtCoderRevManager.Web.Services;

/// <summary>
/// Typed HttpClient for Review API operations.
/// Configured via Service Discovery in Program.cs.
/// </summary>
public class ReviewApiClient(HttpClient httpClient)
{
    // C# 12 Primary Constructor eliminates the need for explicit field declaration and constructor body.

    public async Task<List<ReviewModel>> GetReviewsAsync(string userId)
    {
        // "api/users/{userId}/reviews" path matches the Controller route.
        return await httpClient.GetFromJsonAsync<List<ReviewModel>>($"api/users/{userId}/reviews")
               ?? [];
    }

    public async Task CreateReviewAsync(string userId, CreateReviewRequest request)
    {
        var response = await httpClient.PostAsJsonAsync($"api/users/{userId}/reviews", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateProgressAsync(string userId, string id, bool isSolved, string notes)
    {
        var request = new UpdateProgressRequest(isSolved, notes);
        var response = await httpClient.PutAsJsonAsync($"api/users/{userId}/reviews/{id}/progress", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteReviewAsync(string userId, string id)
    {
        var response = await httpClient.DeleteAsync($"api/users/{userId}/reviews/{id}");
        response.EnsureSuccessStatusCode();
    }
}