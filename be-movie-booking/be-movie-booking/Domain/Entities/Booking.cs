using System;
using System.Collections.Generic;

namespace be_movie_booking.Domain.Entities;

public partial class Booking
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ShowTimeId { get; set; }

    public string Status { get; set; } = null!;

    public int TotalPrice { get; set; }

    public DateTime BookingDate { get; set; }

    public int? VoucherId { get; set; }
    public int PointUsed { get; set; } = 0;

    public virtual ICollection<BookingFood> BookingFoods { get; set; } = new List<BookingFood>();

    public virtual ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();

    public virtual ShowTime ShowTime { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual Voucher? Voucher { get; set; }
}
