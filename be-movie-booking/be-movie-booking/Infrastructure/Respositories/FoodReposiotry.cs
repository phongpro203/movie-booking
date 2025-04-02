using be_movie_booking.Domain.DTOs.Requests;
using be_movie_booking.Domain.Entities;
using be_movie_booking.Infrastructure.Interfaces.Repository;
using be_movie_booking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace be_movie_booking.Infrastructure.Respositories
{
    public class FoodReposiotry : Repository<Food>, IFoodRepository
    {
        public FoodReposiotry(MyDbContext context) : base(context) { }
        public async Task<List<Food>> GetValidFoodsAsync(List<FoodRequest> foodItems)
        {
            return await _dbSet
                .Where(f => foodItems.Select(food => food.Id).Contains(f.Id))
                .ToListAsync();
        }
    }
}
