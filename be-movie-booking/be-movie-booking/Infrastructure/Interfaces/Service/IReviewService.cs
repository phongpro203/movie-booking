using be_movie_booking.Domain.Entities;

namespace be_movie_booking.Infrastructure.Interfaces.Service
{
    public interface IReviewService
    {
        Task<Review> RatingMovie(Review revew);
    }
}
