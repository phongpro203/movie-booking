namespace be_movie_booking.Domain.DTOs.Responses
{
    public class ReviewResponse
    {
        public int id { get; set; }
        public string? Comment { get; set; }

        public int? Rate { get; set; }

        public int userId { get; set; }

        public string? userName { get; set; }

    }
}
