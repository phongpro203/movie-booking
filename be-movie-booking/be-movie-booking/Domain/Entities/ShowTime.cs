using System;
using System.Collections.Generic;

namespace be_movie_booking.Domain.Entities;

public partial class ShowTime
{
    public int Id { get; set; }

    public DateOnly ShowDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public int MovieId { get; set; }

    public int RoomId { get; set; }

    public int CinemaId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Cinema? Cinema { get; set; } 

    public virtual Movie? Movie { get; set; }

    public virtual Room? Room { get; set; }
}
