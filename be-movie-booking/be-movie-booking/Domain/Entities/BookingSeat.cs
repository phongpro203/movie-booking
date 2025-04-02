﻿using System;
using System.Collections.Generic;

namespace be_movie_booking.Domain.Entities;

public partial class BookingSeat
{
    public int Id { get; set; }

    public int BookingId { get; set; }

    public int SeatId { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Seat Seat { get; set; } = null!;
}
