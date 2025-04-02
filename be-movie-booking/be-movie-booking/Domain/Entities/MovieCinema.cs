using System;
using System.Collections.Generic;

namespace be_movie_booking.Domain.Entities;

public partial class MovieCinema
{
    public int Id { get; set; }

    public int CinemaId { get; set; }

    public int MovieId { get; set; }

    public virtual Cinema Cinema { get; set; } = null!;

    public virtual Movie Movie { get; set; } = null!;
}
