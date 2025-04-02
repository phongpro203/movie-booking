using be_movie_booking.Domain.DTOs.Responses;
using be_movie_booking.Domain.Entities;
using be_movie_booking.Infrastructure.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace be_movie_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowTimeController : ControllerBase
    {
        private readonly IShowTimeService _showTimeService;
        private readonly IFoodService _foodService;
        private readonly IVoucherService _voucherService;
        private readonly IRewardService _rewardService;
        public ShowTimeController(IShowTimeService showTimeService, IFoodService foodService, IVoucherService voucherService, IRewardService rewardService)
        {
            _showTimeService = showTimeService;
            _foodService = foodService;
            _voucherService = voucherService;
            _rewardService = rewardService;
        }
        [HttpGet("ShowTime-by-movieId")]
        public async Task<ActionResult<IEnumerable<ShowTimeByMovieResponse>>> GetShowTimeByMovieSAsync(int movieId, int cinemaId)
        {
            var showTime = await _showTimeService.GetShowTimeByMovieSAsync(movieId, cinemaId);
            return Ok(showTime);
        }
        [Authorize]
        [HttpGet("showtime-detail")]
        public async Task<ActionResult> GetShowTimeDetailAsync(int showTimeId, int userId)
        {
            var showTime = await _showTimeService.GetShowTimeDetailAsync(showTimeId);
            var food = await _foodService.GetAllFoodsAsync();
            var vouchers = await _voucherService.GetAllVoucherAsync();
            var reward = await _rewardService.GetRewardByUserAsync(userId);

            return Ok(new
            {
                showTime,
                food,
                vouchers,
                reward
            });
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageIndex, int pageSize)
        {
            var showTimes = await _showTimeService.GetShowtimeAsync(pageIndex,pageSize);
            return Ok(showTimes);
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([FromBody] ShowTime showTime)
        {
            var createdShowTime = await _showTimeService.CreateShowTimeAsync(showTime);
            return Ok(showTime);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update(int id, [FromBody] ShowTime showTime)
        {
            var updatedShowTime = await _showTimeService.UpdateShowTimeAsync(id, showTime);
            if (updatedShowTime == null) return NotFound();
            return Ok(updatedShowTime);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _showTimeService.DeleteShowTimeAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
