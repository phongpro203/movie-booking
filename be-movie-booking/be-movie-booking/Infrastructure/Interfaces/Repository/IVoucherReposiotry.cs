using be_movie_booking.Domain.DTOs.Requests;
using be_movie_booking.Domain.Entities;

namespace be_movie_booking.Infrastructure.Interfaces.Repository
{
    public interface IVoucherReposiotry : IRepository<Voucher>
    {
        Task<Voucher?> GetValidVoucherAsync(int? VoucherId);
    }
}
