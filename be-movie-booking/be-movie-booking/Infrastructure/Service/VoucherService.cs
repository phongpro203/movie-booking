using AutoMapper;
using be_movie_booking.Domain.Entities;
using be_movie_booking.Infrastructure.Interfaces.Repository;
using be_movie_booking.Infrastructure.Interfaces.Service;
using Microsoft.EntityFrameworkCore;

namespace be_movie_booking.Infrastructure.Service
{
    public class VoucherService : IVoucherService
    {
        private readonly IRepository<Voucher> _voucherRepository;
        private readonly IMapper _mapper;

        public VoucherService(IRepository<Voucher> voucherRepository, IMapper mapper)
        {
            _voucherRepository = voucherRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Voucher>> GetAllVoucherAsync()
        {
            return await _voucherRepository.GetAllAsync();
        }
        public async Task<IEnumerable<Voucher>> GetVouchersAsync()
        {
            return await _voucherRepository.GetAllAsync();
        }


        public async Task<Voucher> CreateVoucherAsync(Voucher voucher)
        {
            await _voucherRepository.AddAsync(voucher);
            await _voucherRepository.SaveChangesAsync();
            return voucher;
        }

        public async Task<Voucher?> UpdateVoucherAsync(int id, Voucher voucher)
        {
            var existingVoucher = await _voucherRepository.GetByIdAsync(id);
            if (existingVoucher == null) return null;

            _mapper.Map(voucher, existingVoucher);
            await _voucherRepository.UpdateAsync(existingVoucher);

            return existingVoucher;
        }

        public async Task<bool> DeleteVoucherAsync(int id)
        {
            var voucher = await _voucherRepository.GetByIdAsync(id);
            if (voucher == null) return false;

            await _voucherRepository.DeleteAsync(id);
            await _voucherRepository.SaveChangesAsync();
            return true;
        }
    }
}
