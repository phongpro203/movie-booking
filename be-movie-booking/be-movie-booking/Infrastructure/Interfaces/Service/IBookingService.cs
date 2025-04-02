using be_movie_booking.Domain.DTOs.Requests;
using be_movie_booking.Domain.DTOs.Responses;
using be_movie_booking.Domain.Entities;

namespace be_movie_booking.Infrastructure.Interfaces.Service
{
    public interface IBookingService
    {
        Task<Booking> CheckoutAsync(int userId, int showTimeId, List<int> seatIds, List<FoodRequest> foodItems, int? voucherId, int pointCount);
        Task RemoveExpiredBookingsAsync(int bookingId);
        Task PaymentSuccess(int bookingId);
        Task<BookingDetailResponse> GetBookingDetail(int bookingId);
        Task<IEnumerable<BookingDetailResponse>> GetBookings(int userId);
        Task<object> GetBookingStatisticsAsync(string StartDate, string EndDate);
    }
}
