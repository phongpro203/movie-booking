using Azure.Core;
using be_movie_booking.Domain.DTOs.Requests;
using be_movie_booking.Domain.DTOs.Responses;
using be_movie_booking.Domain.Entities;
using be_movie_booking.Infrastructure.Interfaces.Repository;
using be_movie_booking.Infrastructure.Interfaces.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace be_movie_booking.Infrastructure.Service
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IFoodRepository _foodRepository;
        private readonly IVoucherReposiotry _voucherReposiotry;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IRewardRepository _rewardRepository;

        public BookingService(IBookingRepository bookingRepository, ISeatRepository seatRepository
            , IFoodRepository foodRepository, IVoucherReposiotry voucherReposiotry, IServiceScopeFactory serviceScopeFactory, IRewardRepository rewardRepository)
        {
            _bookingRepository = bookingRepository;
            _seatRepository = seatRepository;
            _foodRepository = foodRepository;
            _serviceScopeFactory = serviceScopeFactory;
            _voucherReposiotry = voucherReposiotry;
            _rewardRepository = rewardRepository;
        }

        public async Task<Booking> CheckoutAsync(int userId, int showTimeId, List<int> seatIds, List<FoodRequest> foodItems, int? voucherId, int pointCount)
        {
            using var transaction = await _bookingRepository.BeginTransactionAsync(); // Thêm transaction
            try
            {
                // Kiểm tra & lấy dữ liệu hợp lệ
                var validSeats = await _seatRepository.GetValidSeatsAsync(seatIds);
                var validFoods = await _foodRepository.GetValidFoodsAsync(foodItems);
                var voucherValid = await _voucherReposiotry.GetValidVoucherAsync(voucherId);

                if (!validSeats.Any())
                {
                    throw new Exception("Ghế không hợp lệ.");
                }

                // Kiểm tra ghế có bị đặt trước không
                if (!await _bookingRepository.SeatsAvailableAsync(seatIds, showTimeId))
                {
                    throw new Exception("Some seats are already booked.");
                }

                var totalPrice = validSeats.Sum(s => s.SeatType!.Price) +
                                 validFoods.Sum(f => f.Price * foodItems.First(item => item.Id == f.Id).Quantity);

                // Áp dụng voucher nếu có
                if (voucherValid != null)
                {
                    totalPrice = (int)(totalPrice * (1 - voucherValid.Discount.GetValueOrDefault() / 100f));
                }

                if (pointCount > 0)
                {
                    totalPrice -= pointCount;
                }

                // Tạo booking
                var booking = new Booking
                {
                    UserId = userId,
                    ShowTimeId = showTimeId,
                    Status = "Pending",
                    BookingDate = DateTime.UtcNow.AddHours(7),
                    TotalPrice = totalPrice,
                    VoucherId = voucherId,
                    PointUsed = pointCount,
                    BookingSeats = seatIds.Select(seatId => new BookingSeat { SeatId = seatId }).ToList(),
                    BookingFoods = foodItems.Select(food => new BookingFood { FoodId = food.Id, FoodQuantity = food.Quantity }).ToList()
                };

                // Lưu vào DB
                await _bookingRepository.AddAsync(booking);


                await _bookingRepository.SaveChangesAsync();

                // Commit transaction nếu tất cả thành công
                await transaction.CommitAsync();

                // Đặt Task xóa sau 5 phút
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await Task.Delay(TimeSpan.FromMinutes(5));
                        using var scope = _serviceScopeFactory.CreateScope();
                        var bookingService = scope.ServiceProvider.GetRequiredService<IBookingService>();
                        await bookingService.RemoveExpiredBookingsAsync(booking.Id);
                    }
                    catch (Exception ex)
                    {
                        // Log lỗi nếu cần
                        Console.WriteLine($"Lỗi khi xóa booking hết hạn: {ex.Message}");
                    }
                });

                return booking;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Hủy transaction nếu lỗi
                Console.WriteLine($"Lỗi khi đặt vé: {ex.Message}");
                throw;
            }

        }
        public async Task RemoveExpiredBookingsAsync(int bookingId)
        {
            await _bookingRepository.RemoveExpiredBookingsAsync(bookingId);
        }
        public async Task PaymentSuccess(int bookingId)
        {
            await _bookingRepository.PaymentSuccess(bookingId);
        }
        public async Task<BookingDetailResponse> GetBookingDetail(int bookingId)
        {
            return await _bookingRepository.GetBookingDetail(bookingId);
        }
        public async Task<IEnumerable<BookingDetailResponse>> GetBookings(int userId)
        {
            return await _bookingRepository.GetBookings(userId);
        }
        public async Task<object> GetBookingStatisticsAsync(string StartDate, string EndDate)
        {
            // Chuyển đổi StartDate và EndDate từ chuỗi sang DateTime
            if (!DateTime.TryParse(StartDate, out DateTime startDate) || !DateTime.TryParse(EndDate, out DateTime endDate))
            {
                return new { Error = "Invalid date format" };
            }
            startDate = startDate.ToLocalTime();
            endDate = endDate.ToLocalTime().AddDays(1).AddTicks(-1);

            // Lọc danh sách bookings trong khoảng thời gian yêu cầu
            var bookings = _bookingRepository.GetConfirmedBookings()
                .Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate)
                .Include(b => b.BookingSeats) // Load danh sách ghế đặt
                .Include(b => b.BookingFoods);

            var stats = new
            {
                TotalRevenue = bookings.Sum(b => b.TotalPrice),
                TotalTickets = bookings.SelectMany(b => b.BookingSeats).Count(),
                TotalFoods = bookings.SelectMany(b => b.BookingFoods).Sum(f => f.FoodQuantity),
                TotalBookings = bookings.Count()
            };

            var dailyData = bookings
                .AsEnumerable() // Chuyển sang xử lý phía client
                .GroupBy(b => b.BookingDate.Date)
                .Select(g => new
                {
                    Date = g.Key.ToString("yyyy-MM-dd"), // Định dạng ngày
                    Revenue = g.Sum(b => b.TotalPrice),
                    Tickets = g.SelectMany(b => b.BookingSeats).Count(),
                    Foods = g.SelectMany(b => b.BookingFoods).Sum(f => f.FoodQuantity),
                    Bookings = g.Count()
                })
                .OrderBy(x => x.Date)
                .ToList();

            return new { Stats = stats, Data = dailyData };
        }

    }
}
