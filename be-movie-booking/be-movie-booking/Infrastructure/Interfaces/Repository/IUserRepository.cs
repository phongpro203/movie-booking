using be_movie_booking.Domain.Entities;

namespace be_movie_booking.Infrastructure.Interfaces.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> ValidateUser(string username, string password);
        Task<User?> GetInfo(int id);
    }
}
