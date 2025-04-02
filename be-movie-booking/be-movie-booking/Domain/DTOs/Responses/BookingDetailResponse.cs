using be_movie_booking.Domain.Entities;

namespace be_movie_booking.Domain.DTOs.Responses
{
    public class BookingDetailResponse
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Status { get; set; } = null!;

        public int TotalPrice { get; set; }

        public DateTime BookingDate { get; set; }

        public string? Voucher { get; set; }

        public TimeOnly Showtime { get; set; }
        public string movieName { get; set; }

        public List<string> seats { get; set; } = new List<string>();
        public List<FoodDTO> foods { get; set; } = new List<FoodDTO>();

    }
}
