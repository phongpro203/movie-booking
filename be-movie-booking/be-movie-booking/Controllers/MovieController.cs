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
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }
        [HttpGet("all-movie")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAllMovies()
        {
            var movies = await _movieService.GetAllMovieAsync();
            return Ok(movies);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAllMovies(int pageIndex, int pageSize)
        {
            var movies = await _movieService.GetMoviesAsync(pageIndex,pageSize);
            return Ok(movies);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Movie>> CreateMovie(Movie movie)
        {
            var createdMovie = await _movieService.CreateMovieAsync(movie);
            return Ok(movie);
        }

        // Cập nhật phim
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateMovie(int id, Movie movie)
        {
            var updatedMovie = await _movieService.UpdateMovieAsync(id, movie);
            if (updatedMovie == null) return NotFound();

            return NoContent();
        }

        // Xóa phim
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var isDeleted = await _movieService.DeleteMovieAsync(id);
            if (!isDeleted) return NotFound();

            return NoContent();
        }

        [HttpGet("movie-showing")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMoviesShowing(int cinemaId, bool nowShowing)
        {
            var movies = await _movieService.GetShowingMoviesAsync(cinemaId, nowShowing);
            return Ok(movies);
        }
        [HttpGet("movie-detail")]
        public async Task<ActionResult<MovieShowTimeResponse?>> GetMovieDetailAcsync(int movieId, int cinemaId)
        {
            var movie = await _movieService.GetMovieDetailAcsync(movieId,cinemaId);
            if (movie == null) {
                return NotFound();
            }
            return Ok(movie);
        }
        [HttpGet("movie-by-cinema")]
        public async Task<ActionResult<IEnumerable<MovieShowTimeResponse?>>> GetMovieShowTimeByCinemaAcsync(int cinemaId)
        {
            var movies = await _movieService.GetMovieShowTimeByCinemaAcsync(cinemaId);
            if (movies == null)
            {
                return NotFound();
            }
            return Ok(movies);
        }
    }
}
