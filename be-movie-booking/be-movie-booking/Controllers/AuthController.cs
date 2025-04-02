using be_movie_booking.Domain.DTOs.Requests;
using be_movie_booking.Domain.Entities;
using be_movie_booking.Infrastructure.Interfaces.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace be_movie_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _userService.AuthenticateAndGenerateToken(request.username!, request.password!);
            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(token);
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] User _user)
        {
            var user = await _userService.Register(_user);
            return Ok(user);
        }
    }
}
