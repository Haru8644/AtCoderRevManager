using System.Collections.Generic;
using System.Threading.Tasks;
using AtCoderRevManager.Domain.Entities;

namespace AtCoderRevManager.Domain.Interfaces;

/// <summary>
/// Defines the contract for data access operations related to reviews.
/// </summary>
public interface IReviewRepository
{
    Task AddAsync(Review review);
    Task<Review?> GetByIdAsync(string id, string userId);
    Task<IEnumerable<Review>> GetAllByUserIdAsync(string userId);
    Task UpdateAsync(Review review);
    Task DeleteAsync(string id, string userId);
}