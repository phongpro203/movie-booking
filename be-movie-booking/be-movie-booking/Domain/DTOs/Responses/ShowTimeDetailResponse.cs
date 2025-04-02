using be_movie_booking.Domain.Entities;

namespace be_movie_booking.Domain.DTOs.Responses
{
    public class ShowTimeDetailResponse
    {
        public int Id { get; set; }

        public DateOnly ShowDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public MovieShowingResponse? Movie { get; set; }

        public string? CinemaName { get; set; }

        public string? RoomName { get; set; }

        public List<SeatResponse> Seats { get; set; } = new List<SeatResponse>();

    }
}
