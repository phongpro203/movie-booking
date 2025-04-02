using be_movie_booking.Domain.Entities;
using be_movie_booking.Infrastructure.Interfaces.Repository;
using be_movie_booking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace be_movie_booking.Infrastructure.Respositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MyDbContext context) : base(context) { }
        public async Task<User?> ValidateUser(string username, string password)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }
        public async Task<User?> GetInfo(int id)
        {
            return await _dbSet.Include(u => u.Address).FirstOrDefaultAsync(u => u.Id == id);
        }

    }
}
