using be_movie_booking.Domain.DTOs.Responses;
using be_movie_booking.Domain.Entities;
using be_movie_booking.Infrastructure.Interfaces.Repository;
using be_movie_booking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace be_movie_booking.Infrastructure.Respositories
{
    public class CinemaRepository : Repository<Cinema>, ICinemaRepository
    {
        public CinemaRepository(MyDbContext context) : base(context) { }
        public new async Task<IEnumerable<Cinema>> GetAllAsync()
        {
            return await _dbSet.Include(c => c.Address).ToListAsync();
        }
        public async Task<IEnumerable<CinemaNameResponse>> GetAllCinemasNameAsync()
        {
            return await _dbSet
                .Select(c => new CinemaNameResponse
                {
                    Id = c.Id,
                    CinemaName = c.Name
                })
                .ToListAsync();
        }

    }
}
