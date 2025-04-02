using System;
using System.Collections.Generic;

namespace be_movie_booking.Domain.Entities;

public partial class Voucher
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? Discount { get; set; }

    public string? Conditions { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly ExpirationDate { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
