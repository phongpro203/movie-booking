using System;
using System.Collections.Generic;

namespace be_movie_booking.Domain.Entities;

public partial class SeatType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Price { get; set; }

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
}
