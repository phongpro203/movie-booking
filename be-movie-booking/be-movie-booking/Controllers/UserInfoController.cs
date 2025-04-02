using be_movie_booking.Domain.DTOs.Responses;
using be_movie_booking.Domain.Entities;
using be_movie_booking.Infrastructure.Interfaces.Repository;
using be_movie_booking.Infrastructure.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace be_movie_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserInfoController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;
        private readonly IRewardService _rewardService;
        public UserInfoController(IBookingService bookingService, IUserService userService, IRewardService rewardService)
        {
            _bookingService = bookingService;
            _userService = userService;
            _rewardService = rewardService;
        }
        [HttpGet("user-info")]
        public async Task<ActionResult<IEnumerable<User>>> GetUserInfo(int userId)
        {
            var user = await _userService.GetInfo(userId);
            return Ok(user);
        }
        [HttpGet("booking-info")]
        public async Task<ActionResult<IEnumerable<BookingDetailResponse>>> GetBookings(int userId)
        {
            var bookings = await _bookingService.GetBookings(userId);
            return Ok(bookings);
        }
        [HttpGet("reward-info")]
        public async Task<ActionResult<IEnumerable<Reward>>> GetRewardsByUserAsync(int userId)
        {
            var rewards = await _rewardService.GetRewardsByUserAsync(userId);
            return Ok(rewards);
        }
    }
}
