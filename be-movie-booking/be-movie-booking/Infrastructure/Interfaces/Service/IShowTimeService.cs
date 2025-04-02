using be_movie_booking.Domain.DTOs.Responses;
using be_movie_booking.Domain.Entities;

namespace be_movie_booking.Infrastructure.Interfaces.Service
{
    public interface IShowTimeService
    {
        Task<IEnumerable<ShowTimeByMovieResponse>> GetShowTimeByMovieSAsync(int movieId, int cinemaId);
        Task<ShowTimeDetailResponse?> GetShowTimeDetailAsync(int showTimeId);
        Task<PaginatedList<ShowTime>> GetShowtimeAsync(int pageIndex, int pageSize);
        Task<ShowTime> CreateShowTimeAsync(ShowTime showTime);
        Task<ShowTime?> UpdateShowTimeAsync(int id, ShowTime showTime);
        Task<bool> DeleteShowTimeAsync(int id);
    }
}
