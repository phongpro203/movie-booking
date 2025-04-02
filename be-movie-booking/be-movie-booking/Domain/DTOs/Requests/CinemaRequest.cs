using be_movie_booking.Domain.Entities;

namespace be_movie_booking.Domain.DTOs.Requests
{
    public class CinemaRequest
    {
        public string name { get; set; } = null!;
        public Address? address { get; set; }
    }
}
