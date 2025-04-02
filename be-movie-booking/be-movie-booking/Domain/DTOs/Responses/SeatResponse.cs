namespace be_movie_booking.Domain.DTOs.Responses
{
    public class SeatResponse
    {
        public int Id { get; set; }
        public string SeatNumber { get; set; } = null!;
        public string Status { get; set; } = string.Empty;
        public int price { get; set; }
        public string SeatType { get; set; } = string.Empty;
    }
}
