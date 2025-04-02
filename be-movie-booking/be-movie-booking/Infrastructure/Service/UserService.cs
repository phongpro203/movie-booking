using be_movie_booking.Domain.Entities;
using be_movie_booking.Infrastructure.Interfaces.Repository;
using be_movie_booking.Infrastructure.Interfaces.Service;
using Microsoft.AspNetCore.Identity.Data;

namespace be_movie_booking.Infrastructure.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly JWTService _jwtService;
        public UserService(IUserRepository userRepository , JWTService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }
        public async Task<string?> AuthenticateAndGenerateToken(string username, string password)
        {
            var user = await _userRepository.ValidateUser(username, password);
            if (user != null)
            {
                return _jwtService.GenerateJwtToken(user.Id, user.Username, user.Role);
            }
            return null;
        }
        public async Task<User> Register(User user)
        {
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            return user;
        }
        public async Task<User?> GetInfo(int id)
        {
            return await _userRepository.GetInfo(id);
        }
    }
}
