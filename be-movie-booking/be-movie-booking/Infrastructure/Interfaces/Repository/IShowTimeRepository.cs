using be_movie_booking.Domain.DTOs.Responses;
using be_movie_booking.Domain.Entities;

namespace be_movie_booking.Infrastructure.Interfaces.Repository
{
    public interface IShowTimeRepository : IRepository<ShowTime>
    {
        Task<IEnumerable<ShowTimeByMovieResponse>> GetShowTimeByMovieSAsync(int movieId, int cinemaId);
        Task<ShowTimeDetailResponse?> GetShowTimeDetailAsync(int showTimeId);
        Task<(IEnumerable<ShowTime> Items, int TotalCount)> GetPagedAsync(int pageIndex, int pageSize);
        Task<bool> ValidateRoomAndCinema(int roomId, int cinemaId);
    }
}
