using be_movie_booking.Domain.Entities;
using Microsoft.AspNetCore.Identity.Data;

namespace be_movie_booking.Infrastructure.Interfaces.Service
{
    public interface IUserService
    {
        Task<string?> AuthenticateAndGenerateToken(string username, string password);
        Task<User> Register(User user);
        Task<User?> GetInfo(int id);
    }
}
