using be_movie_booking.Domain.DTOs.Responses;
using be_movie_booking.Domain.Entities;

namespace be_movie_booking.Infrastructure.Interfaces.Repository
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<bool> SeatsAvailableAsync(List<int> seatIds, int showtimeId);
        Task RemoveExpiredBookingsAsync(int bookingId);
        Task PaymentSuccess(int bookingId);
        Task<BookingDetailResponse> GetBookingDetail(int bookingId);
        Task<IEnumerable<BookingDetailResponse>> GetBookings(int userId);

        IQueryable<Booking> GetConfirmedBookings();
    }
}
