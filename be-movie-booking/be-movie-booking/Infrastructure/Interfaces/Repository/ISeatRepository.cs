using be_movie_booking.Domain.Entities;

namespace be_movie_booking.Infrastructure.Interfaces.Repository
{
    public interface ISeatRepository : IRepository<Seat>
    {
        Task<List<Seat>> GetValidSeatsAsync(List<int> seatIds);
        Task<IEnumerable<Seat>> GetSeatByRoomId(int roomId);
    }
}
