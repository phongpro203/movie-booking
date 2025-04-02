namespace be_movie_booking.Domain.DTOs.Requests
{
    public class CheckoutRequest
    {
        public int UserId { get; set; }
        public int ShowTimeId { get; set; }
        public List<int> SeatIds { get; set; } = new List<int>();
        public List<FoodRequest> FoodItems { get; set; } = new List<FoodRequest>();
        public int? VoucherId { get; set; }
        public int pointCount { get; set; }
    }
}
