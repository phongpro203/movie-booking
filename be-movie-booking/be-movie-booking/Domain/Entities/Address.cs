using System;
using System.Collections.Generic;

namespace be_movie_booking.Domain.Entities;

public partial class Address
{
    public int Id { get; set; }

    public string? HouseNumber { get; set; }

    public string? Street { get; set; }

    public string? Ward { get; set; }

    public string? District { get; set; }

    public string? City { get; set; }

    public virtual ICollection<Cinema> Cinemas { get; set; } = new List<Cinema>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
