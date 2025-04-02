using be_movie_booking.Domain.Entities;

namespace be_movie_booking.Infrastructure.Interfaces.Service
{
    public interface ISeatService
    {
        Task<IEnumerable<Seat>> GetAllSeatsAsync(int roomId);
        Task<IEnumerable<Seat>> CreateSeatAsync(List<Seat> seat);
        Task<Seat?> UpdateSeatAsync(int id, Seat seat);
        Task<bool> DeleteSeatAsync(int id);
    }
}
