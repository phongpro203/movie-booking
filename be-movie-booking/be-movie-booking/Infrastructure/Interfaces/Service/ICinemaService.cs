using be_movie_booking.Domain.DTOs.Responses;
using be_movie_booking.Domain.Entities;

namespace be_movie_booking.Infrastructure.Interfaces.Service
{
    public interface ICinemaService
    {
        Task<IEnumerable<CinemaNameResponse>> GetAllCinemasNameAsync();
        Task<IEnumerable<Cinema>> GetCinemasAsync();
        Task<Cinema> CreateCinemaAsync(Cinema cinema);
        Task<Cinema?> UpdateCinemaAsync(int id, Cinema cinema);
        Task<bool> DeleteCinemaAsync(int id);
    }
}
