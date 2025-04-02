using be_movie_booking.Domain.Entities;
using be_movie_booking.Infrastructure.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace be_movie_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class SeatController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSeats(int roomId)
        {
            var seats = await _seatService.GetAllSeatsAsync(roomId);
            return Ok(seats);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSeat([FromBody] List<Seat> seat)
        {
            var createdSeat = await _seatService.CreateSeatAsync(seat);
            return Ok(new
            {
                message = "Thêm thành công"
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeat(int id, [FromBody] Seat seat)
        {
            var updatedSeat = await _seatService.UpdateSeatAsync(id, seat);
            if (updatedSeat == null) return NotFound();
            return Ok(updatedSeat);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeat(int id)
        {
            var deleted = await _seatService.DeleteSeatAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
