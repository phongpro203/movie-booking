using be_movie_booking.Domain.Entities;
using be_movie_booking.Infrastructure.Interfaces.Repository;
using be_movie_booking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace be_movie_booking.Infrastructure.Respositories
{
    public class SeatReposiotry : Repository<Seat>, ISeatRepository
    {
        public SeatReposiotry(MyDbContext context) : base(context) { }
        public async Task<List<Seat>> GetValidSeatsAsync(List<int> seatIds)
        {
            return await _dbSet.Where(s => seatIds.Contains(s.Id))
                .Include(s => s.SeatType)
                .ToListAsync();
        }
        public async Task<IEnumerable<Seat>> GetSeatByRoomId(int roomId)
        {
            return await _dbSet.Where(s => s.RoomId == roomId).ToListAsync();
        }
    }
}
