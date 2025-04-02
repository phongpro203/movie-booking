using System;
using System.Collections.Generic;

namespace be_movie_booking.Domain.Entities;

public partial class Room
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int SeatCount { get; set; }

    public string RoomType { get; set; } = null!;

    public int CinemaId { get; set; }

    public virtual Cinema? Cinema { get; set; }

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();

    public virtual ICollection<ShowTime> ShowTimes { get; set; } = new List<ShowTime>();
}
