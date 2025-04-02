using System;
using System.Collections.Generic;

namespace be_movie_booking.Domain.Entities;

public partial class Cinema
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? AddressId { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<MovieCinema> MovieCinemas { get; set; } = new List<MovieCinema>();

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

    public virtual ICollection<ShowTime> ShowTimes { get; set; } = new List<ShowTime>();
}
