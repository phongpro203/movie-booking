using be_movie_booking.Domain.Entities;
using be_movie_booking.Infrastructure.Interfaces.Repository;
using be_movie_booking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace be_movie_booking.Infrastructure.Respositories
{
    public class VoucherRepository : Repository<Voucher>, IVoucherReposiotry
    {
        public VoucherRepository(MyDbContext context) : base(context) { }
        public async Task<Voucher?> GetValidVoucherAsync(int? VoucherId)
        {
            return await _dbSet.Where(v => v.Id == VoucherId).FirstOrDefaultAsync();
        }
    }
}
