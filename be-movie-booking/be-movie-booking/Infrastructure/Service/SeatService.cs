using AutoMapper;
using be_movie_booking.Domain.Entities;
using be_movie_booking.Infrastructure.Interfaces.Repository;
using be_movie_booking.Infrastructure.Interfaces.Service;

namespace be_movie_booking.Infrastructure.Service
{
    public class SeatService : ISeatService
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IMapper _mapper;

        public SeatService(ISeatRepository seatRepository, IMapper mapper)
        {
            _seatRepository = seatRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Seat>> GetAllSeatsAsync(int roomId)
        {
            return await _seatRepository.GetSeatByRoomId(roomId);
        }


        public async Task<IEnumerable<Seat>> CreateSeatAsync(List<Seat> seat)
        {
            await _seatRepository.AddRangeAsync(seat);
            await _seatRepository.SaveChangesAsync();
            return seat;
        }

        public async Task<Seat?> UpdateSeatAsync(int id, Seat seat)
        {
            var existingSeat = await _seatRepository.GetByIdAsync(id);
            if (existingSeat == null) return null;

            _mapper.Map(seat, existingSeat);
            await _seatRepository.UpdateAsync(existingSeat);
            await _seatRepository.SaveChangesAsync();
            return existingSeat;
        }

        public async Task<bool> DeleteSeatAsync(int id)
        {
            var seat = await _seatRepository.GetByIdAsync(id);
            if (seat == null) return false;

            await _seatRepository.DeleteAsync(id);
            await _seatRepository.SaveChangesAsync();
            return true;
        }
    }
}
