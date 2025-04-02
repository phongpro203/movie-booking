using AutoMapper;
using be_movie_booking.Domain.Entities;
using be_movie_booking.Infrastructure.Interfaces.Repository;
using be_movie_booking.Infrastructure.Interfaces.Service;

namespace be_movie_booking.Infrastructure.Service
{
    public class FoodService : IFoodService
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IMapper _mapper;
        public FoodService(IFoodRepository foodRepository, IMapper mapper)
        {
            _foodRepository = foodRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Food>> GetAllFoodsAsync()
        {
            return await _foodRepository.GetAllAsync();
        }
        public async Task<Food> CreateFoodAsync(Food food)
        {
            await _foodRepository.AddAsync(food);
            await _foodRepository.SaveChangesAsync();
            return food;
        }

        public async Task<Food?> UpdateFoodAsync(int id, Food food)
        {
            var existingFood = await _foodRepository.GetByIdAsync(id);
            if (existingFood == null) return null;

            _mapper.Map(food, existingFood);

            await _foodRepository.UpdateAsync(existingFood);
            await _foodRepository.SaveChangesAsync();
            return existingFood;
        }

        public async Task<bool> DeleteFoodAsync(int id)
        {
            var food = await _foodRepository.GetByIdAsync(id);
            if (food == null) return false;

            await _foodRepository.DeleteAsync(id);
            await _foodRepository.SaveChangesAsync();
            return true;
        }
    }
}
