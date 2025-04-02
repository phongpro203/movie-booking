using System;
using System.Collections.Generic;

namespace be_movie_booking.Domain.Entities;

public partial class Review
{
    public int Id { get; set; }

    public string? Comment { get; set; }

    public int? Rate { get; set; }

    public int UserId { get; set; }

    public int MovieId { get; set; }

    public virtual Movie? Movie { get; set; }

    public virtual User? User { get; set; }
}
