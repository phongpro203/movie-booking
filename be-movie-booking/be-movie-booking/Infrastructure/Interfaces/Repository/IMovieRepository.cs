using be_movie_booking.Domain.DTOs.Responses;
using be_movie_booking.Domain.Entities;

namespace be_movie_booking.Infrastructure.Interfaces.Repository
{
    public interface IMovieRepository : IRepository<Movie>
    {
        Task<IEnumerable<MovieShowingResponse>> GetShowingMoviesAsync(int cinemaId, bool nowShowing);
        Task<MovieShowTimeResponse?> GetMovieDetailAcsync(int movieId, int cinemaId);
        Task<IEnumerable<MovieShowTimeResponse?>> GetMovieShowTimeByCinemaAcsync(int cinemaId);
    }
}
