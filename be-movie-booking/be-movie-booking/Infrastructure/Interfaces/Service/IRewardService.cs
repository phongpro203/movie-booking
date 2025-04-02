using be_movie_booking.Domain.DTOs.Responses;
using be_movie_booking.Domain.Entities;

namespace be_movie_booking.Infrastructure.Interfaces.Service
{
    public interface IRewardService
    {
        Task<RewardResponse?> GetRewardByUserAsync(int userId);
        Task<IEnumerable<Reward>> GetRewardsByUserAsync(int id);
    }
}
