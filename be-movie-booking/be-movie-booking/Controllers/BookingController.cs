using be_movie_booking.Domain.DTOs.Requests;
using be_movie_booking.Domain.DTOs.Responses;
using be_movie_booking.Infrastructure.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VNPAY.NET.Utilities;

namespace be_movie_booking.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IVnpayService _vpnpayService;
        public BookingController(IBookingService bookingService, IVnpayService vpnpayService)
        {
            _bookingService = bookingService;
            _vpnpayService = vpnpayService;
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request)
        {
            try
            {
                var ipAddress = NetworkHelper.GetIpAddress(HttpContext);
                var booking = await _bookingService.CheckoutAsync(request.UserId, request.ShowTimeId, request.SeatIds, request.FoodItems, request.VoucherId, request.pointCount);
                var urlPayment = _vpnpayService.CreatePaymentUrl(booking.TotalPrice, booking.Id, ipAddress);
                return Ok(urlPayment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("Delete")]
        public async Task RemoveExpiredBookingsAsync(int bookingId)
        {
            await _bookingService.RemoveExpiredBookingsAsync(bookingId);
        }

        [HttpGet("booking-detail")]
        public async Task<ActionResult<BookingDetailResponse>> GetBookingDetail(int bookingId)
        {   
            var booking = await _bookingService.GetBookingDetail(bookingId);
            if (booking == null)
                return NotFound();
            return Ok(booking);
        }
        [Authorize(Roles = "admin")]
        [HttpGet("statics")]
        public async Task<IActionResult> GetBookingStatistics([FromQuery] string StartDate, [FromQuery] string EndDate)
        {
            // Kiểm tra tham số đầu vào
            if (string.IsNullOrEmpty(StartDate) || string.IsNullOrEmpty(EndDate))
            {
                return BadRequest(new { Message = "StartDate and EndDate are required." });
            }

            if (!DateTime.TryParse(StartDate, out DateTime startDate) || !DateTime.TryParse(EndDate, out DateTime endDate))
            {
                return BadRequest(new { Message = "Invalid date format. Please use YYYY-MM-DD." });
            }

            if (startDate > endDate)
            {
                return BadRequest(new { Message = "StartDate must be earlier than or equal to EndDate." });
            }

            try
            {
                var result = await _bookingService.GetBookingStatisticsAsync(StartDate, EndDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while fetching booking statistics.",
                    Error = ex.Message
                });
            }
        }

    }
}
