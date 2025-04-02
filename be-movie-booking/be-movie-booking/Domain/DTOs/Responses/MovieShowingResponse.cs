namespace be_movie_booking.Domain.DTOs.Responses
{
    public class MovieShowingResponse
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public int Duration { get; set; }

        public string Genre { get; set; } = null!;

        public string? Poster { get; set; }

    }
}
