using System;
using System.Collections.Generic;

namespace be_movie_booking.Domain.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public DateOnly? Birthday { get; set; }

    public int? AddressId { get; set; }

    public string Role { get; set; } = null!;

    public virtual Address? Address { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Reward> Rewards { get; set; } = new List<Reward>();
}
