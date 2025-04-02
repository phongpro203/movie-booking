using be_movie_booking.Domain.DTOs.Responses;
using be_movie_booking.Domain.Entities;
using be_movie_booking.Infrastructure.Interfaces.Service;
using be_movie_booking.Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace be_movie_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CinemaController : ControllerBase
    {
        private readonly ICinemaService _cinemaService;
        public CinemaController(ICinemaService cinemaService)
        {
            _cinemaService = cinemaService;
        }
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetCinemas()
        {
            var cinemas = await _cinemaService.GetCinemasAsync();
            return Ok(cinemas);
        }


        // Thêm cinema mới
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCinema([FromBody] Cinema cinema)
        {
            if (cinema == null) return BadRequest(new { message = "Dữ liệu không hợp lệ" });

            var createdCinema = await _cinemaService.CreateCinemaAsync(cinema);
            return Ok(cinema);
        }

        // Cập nhật cinema
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCinema(int id, [FromBody] Cinema cinema)
        {
            if (cinema == null) return BadRequest(new { message = "Dữ liệu không hợp lệ" });

            var updatedCinema = await _cinemaService.UpdateCinemaAsync(id, cinema);
            if (updatedCinema == null) return NotFound(new { message = "Cinema không tồn tại" });

            return Ok(updatedCinema);
        }

        // Xóa cinema
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCinema(int id)
        {
            var deleted = await _cinemaService.DeleteCinemaAsync(id);
            if (!deleted) return NotFound(new { message = "Cinema không tồn tại" });

            return NoContent();
        }
        [HttpGet("all-cinema-name")]
        public async Task<ActionResult<IEnumerable<CinemaNameResponse>>> GetAllCinemasName()
        {
            var cinemas = await _cinemaService.GetAllCinemasNameAsync();
            return Ok(cinemas);
        }
    }
}
