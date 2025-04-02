using be_movie_booking.Domain.Entities;

namespace be_movie_booking.Domain.DTOs.Requests
{
    public class RegitsterRequest
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Name { get; set; } = null!;

        public DateOnly? Birthday { get; set; }

        public virtual Address? Address { get; set; }

    }
}
