using be_movie_booking.Domain.Entities;

namespace be_movie_booking.Domain.DTOs.Responses
{
    public class RewardResponse
    {
        public int PointCount { get; set; }

        public DateOnly ExpiryDate { get; set; }

        public DateOnly EarnedDate { get; set; }

    }
}
