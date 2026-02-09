using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AtCoderRevManager.Domain.Entities;
using AtCoderRevManager.Domain.Interfaces;
using AtCoderRevManager.Infrastructure.Data;

namespace AtCoderRevManager.Infrastructure.Repositories;

public class SqlReviewRepository : IReviewRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<SqlReviewRepository> _logger;

    public SqlReviewRepository(AppDbContext context, ILogger<SqlReviewRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddAsync(Review review)
    {
        await _context.Reviews.AddAsync(review);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Review created. ID: {Id}", review.Id);
    }

    public async Task<Review?> GetByIdAsync(string id, string userId)
    {
        return await _context.Reviews
            .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);
    }

    public async Task<IEnumerable<Review>> GetAllByUserIdAsync(string userId)
    {
        return await _context.Reviews
            .AsNoTracking()
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task UpdateAsync(Review review)
    {
        _context.Reviews.Update(review);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Review updated. ID: {Id}", review.Id);
    }

    public async Task DeleteAsync(string id, string userId)
    {
        var rowsAffected = await _context.Reviews
            .Where(r => r.Id == id && r.UserId == userId)
            .ExecuteDeleteAsync();

        if (rowsAffected > 0)
        {
            _logger.LogInformation("Review deleted. ID: {Id}", id);
        }
    }
}