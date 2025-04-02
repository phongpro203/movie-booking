using be_movie_booking.Domain.Entities;
using System.Text.Json.Serialization;

namespace be_movie_booking.Domain.DTOs.Responses
{
    public class MovieShowTimeResponse
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Description { get; set; } = null!;

        public int Duration { get; set; }

        public string Genre { get; set; } = null!;

        public string? Poster { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Trailer { get; set; }

        public DateOnly ReleaseDate { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public virtual List<ReviewResponse> Reviews { get; set; } = new List<ReviewResponse>();

        public virtual List<ShowTimeByMovieResponse> ShowTimes { get; set; } = new List<ShowTimeByMovieResponse>();
    }
}
