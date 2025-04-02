using System;
using System.Collections.Generic;

namespace be_movie_booking.Domain.Entities;

public partial class Movie
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Duration { get; set; }

    public string Genre { get; set; } = null!;

    public string? Poster { get; set; }

    public string? Trailer { get; set; }

    public DateOnly ReleaseDate { get; set; }

    public virtual ICollection<MovieCinema> MovieCinemas { get; set; } = new List<MovieCinema>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<ShowTime> ShowTimes { get; set; } = new List<ShowTime>();
}
