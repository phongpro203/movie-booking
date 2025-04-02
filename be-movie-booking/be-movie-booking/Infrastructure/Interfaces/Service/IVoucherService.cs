using be_movie_booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace be_movie_booking.Infrastructure.Interfaces.Service
{
    public interface IVoucherService
    {
        Task<IEnumerable<Voucher>> GetAllVoucherAsync();
        Task<Voucher> CreateVoucherAsync(Voucher voucher);
        Task<Voucher?> UpdateVoucherAsync(int id, Voucher voucher);
        Task<bool> DeleteVoucherAsync(int id);
    }
}
