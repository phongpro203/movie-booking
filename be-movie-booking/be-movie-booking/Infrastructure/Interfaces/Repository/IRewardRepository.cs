using be_movie_booking.Domain.DTOs.Responses;
using be_movie_booking.Domain.Entities;

namespace be_movie_booking.Infrastructure.Interfaces.Repository
{
    public interface IRewardRepository : IRepository<Reward>
    {
        Task<RewardResponse?> GetRewardByUserAsync(int userId);

        Task<IEnumerable<Reward>> GetRewardsByUserAsync(int id);
    }
}
