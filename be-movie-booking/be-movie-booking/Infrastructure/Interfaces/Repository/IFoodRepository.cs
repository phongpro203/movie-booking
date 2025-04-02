using be_movie_booking.Domain.DTOs.Requests;
using be_movie_booking.Domain.Entities;

namespace be_movie_booking.Infrastructure.Interfaces.Repository
{
    public interface IFoodRepository : IRepository<Food>
    {
        Task<List<Food>> GetValidFoodsAsync(List<FoodRequest> foodItems);
    }
}
