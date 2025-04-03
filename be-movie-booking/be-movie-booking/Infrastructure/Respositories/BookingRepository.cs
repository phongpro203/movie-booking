using be_movie_booking.Domain.DTOs.Responses;
using be_movie_booking.Domain.Entities;
using be_movie_booking.Infrastructure.Interfaces.Repository;
using be_movie_booking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace be_movie_booking.Infrastructure.Respositories
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        public BookingRepository(MyDbContext context) : base(context) { }

        public async Task<bool> SeatsAvailableAsync(List<int> seatIds, int showtimeId)
        {
            return !await _context.BookingSeats.AnyAsync(bs => seatIds.Contains(bs.SeatId) && bs.Booking.ShowTimeId == showtimeId);
        }
        public async Task RemoveExpiredBookingsAsync(int bookingId)
        {

            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking != null && booking.Status == "Pending") // Kiểm tra nếu chưa thanh toán
            {
                _context.BookingSeats.RemoveRange(_context.BookingSeats.Where(bs => bs.BookingId == bookingId));
                _context.BookingFoods.RemoveRange(_context.BookingFoods.Where(bf => bf.BookingId == bookingId));
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }

        }
        public async Task PaymentSuccess(int bookingId)
        {
            var booking = await GetByIdAsync(bookingId);
            if (booking == null)
            {
                throw new Exception("Không tìm thấy đơn đặt vé.");
            }

            booking.Status = "booked";  // Cập nhật trạng thái

            if (booking.PointUsed > 0) // Chỉ truy vấn khi user thực sự dùng điểm
            {
                var userRewards = await _context.Rewards
                    .Where(r => r.UserId == booking.UserId && r.PointCount > 0) // Chỉ lấy rewards còn điểm
                    .OrderBy(r => r.EarnedDate) // Trừ từ reward cũ nhất
                    .ToListAsync();

                int remainingPointsToUse = booking.PointUsed; // Số điểm user đã chọn dùng

                foreach (var rw in userRewards)
                {
                    if (remainingPointsToUse <= 0) break; // Đã trừ đủ điểm thì dừng

                    int pointsToDeduct = Math.Min(remainingPointsToUse, rw.PointCount);
                    rw.PointCount -= pointsToDeduct; // Giảm điểm trực tiếp
                    remainingPointsToUse -= pointsToDeduct;
                }
            }

            // tặng điểm sau khi mua vé
            var reward = new Reward();
            reward.UserId = booking.UserId;
            reward.PointCount = booking.TotalPrice / 20;
            reward.EarnedDate = DateOnly.FromDateTime(DateTime.Today);
            reward.ExpiryDate = DateOnly.FromDateTime(DateTime.Today).AddMonths(3);
            _context.Rewards.Add(reward);

            await SaveChangesAsync();
        }
        public async Task<BookingDetailResponse> GetBookingDetail(int bookingId)
        {
            var booking = await  _dbSet.Where(b => b.Id == bookingId).Select(b => new BookingDetailResponse
            {
                Id = b.Id,
                BookingDate = b.BookingDate,
                Status = b.Status,
                Showtime = $"{b.ShowTime.ShowDate:dd/MM/yyyy} {b.ShowTime.StartTime:HH:mm}",
                TotalPrice = b.TotalPrice,
                Voucher = b.Voucher.Name,
                movieName = b.ShowTime.Movie.Title,
                foods = b.BookingFoods.Select(f => new FoodDTO
                {
                    name = f.Food.Name,
                    quantity = f.FoodQuantity,
                }).ToList(),
                seats = b.BookingSeats.Select(s => s.Seat.SeatNumber).ToList(),
            }).FirstOrDefaultAsync(b => b.Id == bookingId);
            if(booking == null)
            {
                throw new Exception("Không tìm thấy booking");
            }    
            return booking;
        }
        public async Task<IEnumerable<BookingDetailResponse>> GetBookings(int userId)
        {
            var booking = await _dbSet.Where(b => b.UserId == userId).Select(b => new BookingDetailResponse
            {
                Id = b.Id,
                BookingDate = b.BookingDate,
                Status = b.Status,
                Showtime = $"{b.ShowTime.ShowDate:dd/MM/yyyy} {b.ShowTime.StartTime:HH:mm}",
                TotalPrice = b.TotalPrice,
                Voucher = b.Voucher.Name,
                movieName = b.ShowTime.Movie.Title,
                foods = b.BookingFoods.Select(f => new FoodDTO
                {
                    name = f.Food.Name,
                    quantity = f.FoodQuantity,
                }).ToList(),
                seats = b.BookingSeats.Select(s => s.Seat.SeatNumber).ToList(),
            }).ToListAsync();
            return booking;
        }
        public IQueryable<Booking> GetConfirmedBookings()
        {
            return _context.Bookings.Where(b => b.Status == "booked");
        }

    }
}
