using be_movie_booking.Domain.Entities;
using be_movie_booking.Infrastructure.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace be_movie_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IFoodService _foodService;

        public FoodController(IFoodService foodService)
        {
            _foodService = foodService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var foods = await _foodService.GetAllFoodsAsync();
            return Ok(foods);
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(Food food)
        {
            var createdFood = await _foodService.CreateFoodAsync(food);
            return Ok(food);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update(int id, Food food)
        {
            var updatedFood = await _foodService.UpdateFoodAsync(id, food);
            if (updatedFood == null) return NotFound();
            return Ok(updatedFood);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _foodService.DeleteFoodAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
