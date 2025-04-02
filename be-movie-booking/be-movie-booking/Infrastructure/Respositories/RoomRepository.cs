using be_movie_booking.Domain.Entities;
using be_movie_booking.Infrastructure.Interfaces.Repository;
using be_movie_booking.Infrastructure.Persistence;

namespace be_movie_booking.Infrastructure.Respositories
{
    public class RoomRepository : Repository<Room>, IRoomReposiotry
    {
        public RoomRepository(MyDbContext context) : base(context) { }
    }
}
