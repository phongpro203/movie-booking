using be_movie_booking.Domain.DTOs.Responses;
using be_movie_booking.Domain.Entities;
using be_movie_booking.Infrastructure.Interfaces.Repository;
using be_movie_booking.Infrastructure.Interfaces.Service;
using Microsoft.EntityFrameworkCore;

namespace be_movie_booking.Infrastructure.Service
{
    public class RewardService : IRewardService
    {
        private readonly IRewardRepository _rewardRepository;

        public RewardService(IRewardRepository rewardRepository)
        {
            _rewardRepository = rewardRepository;
        }
        public async Task<RewardResponse?> GetRewardByUserAsync(int userId)
        {
            return await _rewardRepository.GetRewardByUserAsync(userId); 
        }
        public async Task<IEnumerable<Reward>> GetRewardsByUserAsync(int id)
        {
            return await _rewardRepository.GetRewardsByUserAsync(id);
        }
    }
}
