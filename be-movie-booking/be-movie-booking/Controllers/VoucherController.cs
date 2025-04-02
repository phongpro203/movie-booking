using be_movie_booking.Domain.Entities;
using be_movie_booking.Infrastructure.Interfaces.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace be_movie_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService _voucherService;

        public VoucherController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        // Lấy tất cả voucher
        [HttpGet]
        public async Task<IActionResult> GetVouchers()
        {
            var vouchers = await _voucherService.GetAllVoucherAsync();
            return Ok(vouchers);
        }

        // Lấy voucher theo ID

        // Thêm voucher mới
        [HttpPost]
        public async Task<IActionResult> CreateVoucher([FromBody] Voucher voucher)
        {
            if (voucher == null) return BadRequest(new { message = "Dữ liệu không hợp lệ" });

            var createdVoucher = await _voucherService.CreateVoucherAsync(voucher);
            return Ok(voucher);
        }

        // Cập nhật voucher
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVoucher(int id, [FromBody] Voucher voucher)
        {
            if (voucher == null) return BadRequest(new { message = "Dữ liệu không hợp lệ" });

            var updatedVoucher = await _voucherService.UpdateVoucherAsync(id, voucher);
            if (updatedVoucher == null) return NotFound(new { message = "Voucher không tồn tại" });

            return Ok(updatedVoucher);
        }

        // Xóa voucher
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVoucher(int id)
        {
            var deleted = await _voucherService.DeleteVoucherAsync(id);
            if (!deleted) return NotFound(new { message = "Voucher không tồn tại" });

            return NoContent();
        }
    }
}
