using be_movie_booking.Domain.Entities;

namespace be_movie_booking.Infrastructure.Interfaces.Service
{
    public interface IFoodService
    {
        Task<IEnumerable<Food>> GetAllFoodsAsync();
        Task<Food> CreateFoodAsync(Food food);
        Task<Food?> UpdateFoodAsync(int id, Food food);
        Task<bool> DeleteFoodAsync(int id);
    }
}
