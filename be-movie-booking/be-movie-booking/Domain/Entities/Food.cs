using System;
using System.Collections.Generic;

namespace be_movie_booking.Domain.Entities;

public partial class Food
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Detail { get; set; } = null!;

    public int Price { get; set; }

    public virtual ICollection<BookingFood> BookingFoods { get; set; } = new List<BookingFood>();
}
