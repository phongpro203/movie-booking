using be_movie_booking.Domain.DTOs.Responses;
using be_movie_booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace be_movie_booking.Infrastructure.Interfaces.Repository
{
    public interface ICinemaRepository : IRepository<Cinema>
    {
        Task<IEnumerable<CinemaNameResponse>> GetAllCinemasNameAsync();
    }
}
