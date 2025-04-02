using be_movie_booking.Domain.DTOs.Responses;
using be_movie_booking.Domain.Entities;
using be_movie_booking.Infrastructure.Interfaces.Repository;
using be_movie_booking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace be_movie_booking.Infrastructure.Respositories
{
    public class RewardRepository : Repository<Reward>, IRewardRepository
    {
        public RewardRepository(MyDbContext context) : base(context) { }
        public async Task<RewardResponse?> GetRewardByUserAsync(int userId)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            return await _dbSet.Where(r => r.UserId == userId && r.EarnedDate <= today && r.ExpiryDate > today)
                   .GroupBy(r => new { r.UserId }) // Gom nhóm theo UserId
                   .Select(g => new RewardResponse
                   {
                       PointCount = g.Sum(r => r.PointCount), // Tổng PointCount
                       ExpiryDate = g.Max(r => r.ExpiryDate), // Ngày hết hạn lớn nhất
                       EarnedDate = g.Min(r => r.EarnedDate), // Ngày nhận nhỏ nhất
                   }).FirstOrDefaultAsync();

        }
        public async Task<IEnumerable<Reward>> GetRewardsByUserAsync(int id)
        {
            return await _dbSet.Where(r => r.UserId == id).ToListAsync();   
        }
    }
}
