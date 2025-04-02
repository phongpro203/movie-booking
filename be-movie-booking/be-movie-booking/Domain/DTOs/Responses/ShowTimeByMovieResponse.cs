namespace be_movie_booking.Domain.DTOs.Responses
{
    public class ShowTimeByMovieResponse
    {
        public int Id { get; set; }

        public DateOnly ShowDate { get; set; }

        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

    }
}
