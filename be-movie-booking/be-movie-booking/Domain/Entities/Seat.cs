using System;
using System.Collections.Generic;

namespace be_movie_booking.Domain.Entities;

public partial class Seat
{
    public int Id { get; set; }

    public int RoomId { get; set; }

    public string SeatNumber { get; set; } = null!;

    public int SeatTypeId { get; set; }

    public virtual ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();

    public virtual Room? Room { get; set; } 

    public virtual SeatType? SeatType { get; set; } 
}
