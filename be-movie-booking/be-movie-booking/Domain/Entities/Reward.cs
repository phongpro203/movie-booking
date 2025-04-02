using System;
using System.Collections.Generic;

namespace be_movie_booking.Domain.Entities;

public partial class Reward
{
    public int Id { get; set; }

    public int PointCount { get; set; }

    public DateOnly ExpiryDate { get; set; }

    public DateOnly EarnedDate { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
