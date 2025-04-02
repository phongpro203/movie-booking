using be_movie_booking.Domain.DTOs.Responses;
using be_movie_booking.Domain.Entities;

namespace be_movie_booking.Infrastructure.Interfaces.Service
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetAllMovieAsync();
        Task<PaginatedList<Movie>> GetMoviesAsync(int pageIndex, int pageSize);
        Task<Movie> CreateMovieAsync(Movie movie);
        Task<Movie?> UpdateMovieAsync(int id, Movie movie);
        Task<bool> DeleteMovieAsync(int id);
        Task<IEnumerable<MovieShowingResponse>> GetShowingMoviesAsync(int cinemaId, bool nowShowing);
        Task<MovieShowTimeResponse?> GetMovieDetailAcsync(int movieId, int cinemaId);
        Task<IEnumerable<MovieShowTimeResponse?>> GetMovieShowTimeByCinemaAcsync(int cinemaId);
    }
}
